namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Statements;
    using System.Activities.Validation;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Markup;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="Retry"/>
    ///   class is used to provide the logic for retrying a child activity when an exception occurs.
    /// </summary>
    [ContentProperty("Body")]
    public sealed class Retry : NativeActivity
    {
        /// <summary>
        ///   Defines the default retry interval.
        /// </summary>
        private static readonly TimeSpan _defaultRetryInterval = new TimeSpan(0, 0, 0, 1);

        /// <summary>
        ///   Stores the attempt count.
        /// </summary>
        private readonly Variable<Int32> _attemptCount = new Variable<Int32>();

        /// <summary>
        ///   Stores the duration of the delay.
        /// </summary>
        private readonly Variable<TimeSpan> _delayDuration = new Variable<TimeSpan>();

        /// <summary>
        ///   Storres the internal delay activity.
        /// </summary>
        private readonly Delay _internalDelay;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Retry" /> class.
        /// </summary>
        public Retry()
        {
            _internalDelay = new Delay
                             {
                                 Duration = new InArgument<TimeSpan>(_delayDuration)
                             };
            Body = new ActivityAction();
            MaxAttempts = 5;
            ExceptionType = typeof(TimeoutException);
            RetryInterval = _defaultRetryInterval;
        }

        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddDelegate(Body);
            metadata.AddImplementationChild(_internalDelay);
            metadata.AddImplementationVariable(_attemptCount);
            metadata.AddImplementationVariable(_delayDuration);

            RuntimeArgument maxAttemptsArgument = new RuntimeArgument("MaxAttempts", typeof(Int32), ArgumentDirection.In, true);
            RuntimeArgument retryIntervalArgument = new RuntimeArgument("RetryInterval", typeof(TimeSpan), ArgumentDirection.In, true);

            metadata.Bind(MaxAttempts, maxAttemptsArgument);
            metadata.Bind(RetryInterval, retryIntervalArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        maxAttemptsArgument, 
                                                        retryIntervalArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);

            if (Body == null || Body.Handler == null)
            {
                ValidationError validationError = new ValidationError(Resources.Activity_NoChildActivitiesDefined, true, "Body");

                metadata.AddValidationError(validationError);
            }

            if (typeof(Exception).IsAssignableFrom(ExceptionType) == false)
            {
                ValidationError validationError = new ValidationError(Resources.Retry_InvalidExceptionType, false, "ExceptionType");

                metadata.AddValidationError(validationError);
            }
        }

        /// <summary>
        /// Runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        protected override void Execute(NativeActivityContext context)
        {
            ExecuteAttempt(context);
        }

        /// <summary>
        /// Shoulds the retry action.
        /// </summary>
        /// <param name="exceptionType">
        /// The catch exception.
        /// </param>
        /// <param name="thrownException">
        /// The thrown exception.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private static Boolean ShouldRetryAction(Type exceptionType, Exception thrownException)
        {
            if (exceptionType == null)
            {
                return false;
            }

            return exceptionType.IsAssignableFrom(thrownException.GetType());
        }

        /// <summary>
        /// Actions the failed attempt.
        /// </summary>
        /// <param name="faultcontext">
        /// The faultcontext.
        /// </param>
        /// <param name="propagatedexception">
        /// The propagatedexception.
        /// </param>
        /// <param name="propagatedfrom">
        /// The propagatedfrom.
        /// </param>
        private void ActionFailed(NativeActivityFaultContext faultcontext, Exception propagatedexception, ActivityInstance propagatedfrom)
        {
            Int32 currentAttemptCount = _attemptCount.Get(faultcontext);

            currentAttemptCount++;

            _attemptCount.Set(faultcontext, currentAttemptCount);

            Int32 maxAttempts = MaxAttempts.Get(faultcontext);

            if (currentAttemptCount >= maxAttempts)
            {
                // There are no further attempts to make
                return;
            }

            if (ShouldRetryAction(ExceptionType, propagatedexception) == false)
            {
                return;
            }

            faultcontext.CancelChild(propagatedfrom);
            faultcontext.HandleFault();

            TimeSpan retryInterval = RetryInterval.Get(faultcontext);

            if (retryInterval == TimeSpan.Zero)
            {
                ExecuteAttempt(faultcontext);
            }
            else
            {
                // We are going to wait before trying again
                _delayDuration.Set(faultcontext, retryInterval);

                faultcontext.ScheduleActivity(_internalDelay, DelayCompleted);
            }
        }

        /// <summary>
        /// Delays the completed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="completedinstance">
        /// The completedinstance.
        /// </param>
        private void DelayCompleted(NativeActivityContext context, ActivityInstance completedinstance)
        {
            ExecuteAttempt(context);
        }

        /// <summary>
        /// Executes the attempt.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private void ExecuteAttempt(NativeActivityContext context)
        {
            if (Body == null)
            {
                return;
            }

            context.ScheduleAction(Body, null, ActionFailed);
        }

        /// <summary>
        ///   Gets or sets the body of the activity.
        /// </summary>
        /// <value>
        ///   The body of the activity.
        /// </value>
        [Browsable(false)]
        public ActivityAction Body
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the type of the exception.
        /// </summary>
        /// <value>
        ///   The type of the exception.
        /// </value>
        [DefaultValue(typeof(TimeoutException))]
        public Type ExceptionType
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the max attempts.
        /// </summary>
        /// <value>
        ///   The max attempts.
        /// </value>
        public InArgument<Int32> MaxAttempts
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the retry interval.
        /// </summary>
        /// <value>
        ///   The retry interval.
        /// </value>
        public InArgument<TimeSpan> RetryInterval
        {
            get;
            set;
        }
    }
}