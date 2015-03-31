namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Validation;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows.Markup;
    using Neovolve.Toolkit.Workflow.ExecutionProperties;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="BusinessFailureScope&lt;T&gt;"/>
    ///   class is used to provide scope management of business failures in a workflow.
    /// </summary>
    /// <typeparam name="T">
    /// The type of failure code.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// This activity uses the
    ///     <see cref="BusinessFailureTracker{T}"/> workflow execution property with the name of 
    ///     <see cref="BusinessFailureTracker{T}.Name">BusinessFailureTracker{T}.Name</see>
    ///     to obtain <see cref="BusinessFailure{T}"/> values.
    ///     A <see cref="BusinessFailureException{T}"/> instance is then created with these failures.
    ///     No exception will be thrown if the execution property does not contain any failure values.
    ///   </para>
    /// <para>
    /// The <see cref="BusinessFailureTracker{T}"/> execution property is typically populated by
    ///     executions of child <see cref="BusinessFailureEvaluator{T}"/> activities.
    ///   </para>
    /// </remarks>
    [ContentProperty("Activities")]
    public sealed class BusinessFailureScope<T> : NativeActivity where T : struct
    {
        /// <summary>
        ///   The activity execution index.
        /// </summary>
        private readonly Variable<Int32> _activityExecutionIndex = new Variable<Int32>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BusinessFailureScope{T}" /> class.
        /// </summary>
        public BusinessFailureScope()
        {
            Activities = new Collection<Activity>();
            Variables = new Collection<Variable>();
        }

        /// <summary>
        /// Creates and validates a description of the activity’s arguments, variables, child activities, and activity delegates.
        /// </summary>
        /// <param name="metadata">
        /// The activity’s metadata that encapsulates the activity’s arguments, variables, child activities, and activity delegates.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.SetChildrenCollection(Activities);
            metadata.SetVariablesCollection(Variables);
            metadata.AddImplementationVariable(_activityExecutionIndex);

            CacheMetadataValidation(metadata);
        }

        /// <summary>
        /// Performs the execution of the activity.
        /// </summary>
        /// <param name="context">
        /// The execution context under which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            if (Activities == null)
            {
                return;
            }

            if (Activities.Count <= 0)
            {
                return;
            }

            ExecutionProperties executionProperties = context.Properties;

            if (executionProperties.Find(BusinessFailureTracker<T>.Name) == null)
            {
                BusinessFailureTracker<T> tracker = new BusinessFailureTracker<T>();

                executionProperties.Add(BusinessFailureTracker<T>.Name, tracker);
            }

            Activity activity = Activities[0];

            ExecuteChildActivity(context, activity);
        }

        /// <summary>
        /// The complete scope.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private static void CompleteScope(NativeActivityContext context)
        {
            BusinessFailureTracker<T> tracker = context.Properties.Find(BusinessFailureTracker<T>.Name) as BusinessFailureTracker<T>;

            Contract.Assume(tracker != null);

            if (tracker.Failures.Count == 0)
            {
                return;
            }

            throw new BusinessFailureException<T>(tracker.Failures);
        }

        /// <summary>
        /// Caches the metadata validation.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        private void CacheMetadataValidation(NativeActivityMetadata metadata)
        {
            Type evaluatorType = typeof(BusinessFailureEvaluator<Int32>);

            Contract.Assume(evaluatorType.IsGenericType);

            Type genericEvaluatorType = evaluatorType.GetGenericTypeDefinition();
            Type genericTypeToMatch = typeof(T);

            if (Activities == null)
            {
                return;
            }

            // Find any child activities that are BusinessFailureEvaluators with a different type
            foreach (Activity activity in Activities)
            {
                Type activityType = activity.GetType();

                if (activityType.IsGenericType == false)
                {
                    continue;
                }

                if (activityType.GetGenericTypeDefinition().Equals(genericEvaluatorType) == false)
                {
                    continue;
                }

                Type genericArgument = activityType.GetGenericArguments().Single();

                if (genericArgument.Equals(genericTypeToMatch) == false)
                {
                    ValidationError validationError = new ValidationError(
                        Resources.BusinessFailureScope_EvaluatorWithDifferentTypeDetected, true, "Activities");

                    metadata.AddValidationError(validationError);

                    break;
                }
            }
        }

        /// <summary>
        /// Executes the child activity.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="activity">
        /// The activity.
        /// </param>
        private void ExecuteChildActivity(NativeActivityContext context, Activity activity)
        {
            context.ScheduleActivity(activity, OnActivityCompleted);
        }

        /// <summary>
        /// Called when an activity has completed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="completedInstance">
        /// The completed instance.
        /// </param>
        private void OnActivityCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            Int32 currentIndex = _activityExecutionIndex.Get(context);

            Contract.Assume(currentIndex >= 0);

            if ((currentIndex >= Activities.Count) || (Activities[currentIndex] != completedInstance.Activity))
            {
                currentIndex = Activities.IndexOf(completedInstance.Activity);
            }

            Int32 nextActivityIndex = currentIndex + 1;

            if (nextActivityIndex != Activities.Count)
            {
                Activity activity = Activities[nextActivityIndex];

                ExecuteChildActivity(context, activity);

                _activityExecutionIndex.Set(context, nextActivityIndex);
            }
            else
            {
                CompleteScope(context);
            }
        }

        /// <summary>
        ///   Gets the activities.
        /// </summary>
        /// <value>
        ///   The branches.
        /// </value>
        [Browsable(false)]
        [DependsOn("Variables")]
        public Collection<Activity> Activities
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the variables.
        /// </summary>
        /// <value>
        ///   The variables.
        /// </value>
        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get;
            private set;
        }
    }
}