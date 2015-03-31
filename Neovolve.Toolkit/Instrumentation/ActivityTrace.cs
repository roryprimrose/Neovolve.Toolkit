namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="ActivityTrace"/>
    ///   class is used to trace related sets of activities in applications.
    /// </summary>
    public class ActivityTrace : RecordTrace, IActivityWriter
    {
        /// <summary>
        ///   Stores the Context value.
        /// </summary>
        private ActivityTraceContext _context;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ActivityTrace" /> class.
        /// </summary>
        public ActivityTrace()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        public ActivityTrace(String traceSourceName)
            : base(traceSourceName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="resolver">
        /// The <see cref="TraceSource"/> resolver.
        /// </param>
        public ActivityTrace(String traceSourceName, ITraceSourceResolver resolver)
            : base(traceSourceName, resolver)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTrace"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        public ActivityTrace(TraceSource source)
            : base(source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <param name="throwOnSourceLoadFailure">
        /// If set to <c>true</c> and exception will be thrown if the source cannot be loaded.
        /// </param>
        /// <remarks>
        /// This is used to support tracing within this Toolkit. It allows for exception throwing to be avoided
        ///   if the <see cref="TraceSource"/> is not able to be loaded.
        /// </remarks>
        internal ActivityTrace(String traceSourceName, ITraceSourceResolver resolver, Boolean throwOnSourceLoadFailure)
            : base(traceSourceName, resolver, throwOnSourceLoadFailure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTrace"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private ActivityTrace(ActivityTraceContext context)
        {
            Debug.Assert(context != null, "No context has been provided");

            Context = context;

            IsInitialized = true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.
        /// </param>
        /// <returns>
        /// True if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            ActivityTrace compare = obj as ActivityTrace;

            if (compare == null)
            {
                return false;
            }

            return GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return Context.GetHashCode();
        }

        /// <summary>
        /// Resumes the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        public void ResumeActivity(String message, params Object[] arguments)
        {
            Write(TraceEventType.Resume, message, arguments);
        }

        /// <summary>
        /// Starts the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        public void StartActivity(String message, params Object[] arguments)
        {
            Write(TraceEventType.Start, message, arguments);
        }

        /// <summary>
        /// Stops the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        public void StopActivity(String message, params Object[] arguments)
        {
            Write(TraceEventType.Stop, message, arguments);
        }

        /// <summary>
        /// Suspends the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        public void SuspendActivity(String message, params Object[] arguments)
        {
            Write(TraceEventType.Suspend, message, arguments);
        }

        /// <summary>
        /// Initializes this instance with the specified context.
        /// </summary>
        /// <param name="context">
        /// The trace context.
        /// </param>
        /// <overloads>
        /// <summary>
        /// Initializes this instance.
        ///   </summary>
        /// </overloads>
        protected override void Initialize(RecordTraceInitializationContext context)
        {
            Debug.Assert(IsInitialized == false, "This instance has already been initialized");

            TraceSource source = GetSourceFromContext(context);
            ActivityTraceContext activityContext = new ActivityTraceContext
                                                   {
                                                       Source = source
                                                   };

            if (source != null)
            {
                // A source is available so the activity is in a stopped state
                activityContext.State = ActivityTraceState.Stopped;
            }

            Context = activityContext;
        }

        /// <summary>
        /// Writes the internal.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event.
        /// </param>
        /// <param name="getMessage">
        /// The get message function.
        /// </param>
        protected override void WriteInternal(TraceEventType eventType, Func<String> getMessage)
        {
            EnsureInitialized();

            ValidateEventType(eventType);

            // Check if this event is starting the activity
            if (eventType == TraceEventType.Start)
            {
                StartActivity();
            }

            UpdateState(eventType);

            base.WriteInternal(eventType, getMessage);

            if (eventType == TraceEventType.Stop)
            {
                StopActivity();
            }
        }

        /// <summary>
        /// Starts the activity.
        /// </summary>
        private void StartActivity()
        {
            // Store the current activity id and generate a new one
            Context.PreviousActivityId = Trace.CorrelationManager.ActivityId;
            Context.ActivityId = Guid.NewGuid();

            // Check if there is a previous activity
            if (Context.PreviousActivityId.Equals(Guid.Empty) == false)
            {
                // Check if there is a source to write to
                if (Context.State != ActivityTraceState.Disabled)
                {
                    Debug.Assert(Context.Source != null, "No Context.Source instance is available");

                    // Transfer from the previous activity
                    Context.Source.TraceTransfer(0, String.Empty, Context.ActivityId);
                }
            }

            // Start this activity
            // Store the context of this item in the operation
            Trace.CorrelationManager.StartLogicalOperation(Context);
            Trace.CorrelationManager.ActivityId = Context.ActivityId;
        }

        /// <summary>
        /// Stops the activity.
        /// </summary>
        private void StopActivity()
        {
            if (Context.PreviousActivityId.Equals(Guid.Empty) == false)
            {
                // Check if there is a source to write to
                if (Context.State != ActivityTraceState.Disabled)
                {
                    Debug.Assert(Context.Source != null, "No Context.Source instance is available");

                    // Transfer back to the previous activity
                    Context.Source.TraceTransfer(0, String.Empty, Context.PreviousActivityId);
                }
            }

            // Stop this activity
            Trace.CorrelationManager.StopLogicalOperation();
            Trace.CorrelationManager.ActivityId = Context.PreviousActivityId;

            Context.PreviousActivityId = Guid.Empty;
            Context.ActivityId = Guid.Empty;
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event.
        /// </param>
        private void UpdateState(TraceEventType eventType)
        {
            if (eventType == TraceEventType.Start)
            {
                Context.State = ActivityTraceState.Running;
            }
            else if (eventType == TraceEventType.Resume)
            {
                Context.State = ActivityTraceState.Running;
            }
            else if (eventType == TraceEventType.Stop)
            {
                Context.State = ActivityTraceState.Stopped;
            }
            else if (eventType == TraceEventType.Suspend)
            {
                Context.State = ActivityTraceState.Suspended;
            }
            else
            {
                Debug.WriteLine("There is no state change for this trace message type");
            }
        }

        /// <summary>
        /// Validates the type of the event.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event.
        /// </param>
        private void ValidateEventType(TraceEventType eventType)
        {
            // Check if the activity is disabled
            if (State == ActivityTraceState.Disabled)
            {
                return;
            }

            // Check to see if a valid event type is provided based on the running state of the activity
            if (State == ActivityTraceState.Stopped && eventType != TraceEventType.Start)
            {
                // We are attempting to write an event when the activity isn't running
                throw new InvalidOperationException(Resources.TraceActivityNotRunningExceptionMessage);
            }

            if (State == ActivityTraceState.Running && eventType == TraceEventType.Start)
            {
                // We are attempting to start an activity that is already running
                throw new InvalidOperationException(Resources.TraceActivityRunningExceptionMessage);
            }

            if (State == ActivityTraceState.Running && eventType == TraceEventType.Resume)
            {
                // We are attempting to resume an activity that is already running
                throw new InvalidOperationException(Resources.TraceActivityRunningExceptionMessage);
            }

            if (State == ActivityTraceState.Suspended && eventType != TraceEventType.Resume)
            {
                throw new InvalidOperationException(Resources.TraceActivitySuspendedExceptionMessage);
            }

            if (State != ActivityTraceState.Stopped && Context.ActivityId.Equals(Trace.CorrelationManager.ActivityId) == false)
            {
                // The stack of activities appears to be corrupted
                throw new InvalidOperationException(Resources.TraceActivityStackCorruptedExceptionMessage);
            }
        }

        /// <summary>
        ///   Gets the current <see cref = "ActivityTrace" />.
        /// </summary>
        /// <value>
        ///   The current <see cref = "ActivityTrace" />.
        /// </value>
        public static ActivityTrace Current
        {
            get
            {
                if (Trace.CorrelationManager.LogicalOperationStack.Count <= 0)
                {
                    return new ActivityTrace(ActivityTraceContext.Empty);
                }

                Object operationReference = Trace.CorrelationManager.LogicalOperationStack.Peek();

                ActivityTraceContext context = operationReference as ActivityTraceContext;

                if (context == null)
                {
                    return new ActivityTrace(ActivityTraceContext.Empty);
                }

                // No context is available
                // Return a default empty/disabled instance
                return new ActivityTrace(context);
            }
        }

        /// <summary>
        ///   Gets the activity id.
        /// </summary>
        /// <value>
        ///   The activity id.
        /// </value>
        public Guid ActivityId
        {
            get
            {
                return Context.ActivityId;
            }
        }

        /// <summary>
        ///   Gets the state of the activity.
        /// </summary>
        /// <value>
        ///   The state of the activity.
        /// </value>
        public ActivityTraceState State
        {
            get
            {
                if (Enabled == false)
                {
                    return ActivityTraceState.Disabled;
                }

                return Context.State;
            }
        }

        /// <summary>
        ///   Gets the trace source.
        /// </summary>
        /// <value>
        ///   The trace source.
        /// </value>
        protected override TraceSource Source
        {
            get
            {
                return Context.Source;
            }
        }

        /// <summary>
        ///   Gets or sets the context.
        /// </summary>
        /// <value>
        ///   The context.
        /// </value>
        private ActivityTraceContext Context
        {
            get
            {
                EnsureInitialized();

                return _context;
            }

            set
            {
                _context = value;
            }
        }
    }
}