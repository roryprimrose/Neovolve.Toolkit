namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Validation;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Windows.Markup;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="IfThen"/>
    ///   class is a workflow activity that executes the child activity only if a condition is valid.
    /// </summary>
    [ContentProperty("Body")]
    public sealed class IfThen : NativeActivity
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IfThen" /> class.
        /// </summary>
        public IfThen()
        {
            Body = new ActivityAction();
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

            RuntimeArgument conditionArgument = new RuntimeArgument("Condition", typeof(Boolean), ArgumentDirection.In, true);

            metadata.Bind(Condition, conditionArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        conditionArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);

            if (Body == null || Body.Handler == null)
            {
                ValidationError validationError = new ValidationError(Resources.Activity_NoChildActivitiesDefined, true, "Body");

                metadata.AddValidationError(validationError);
            }
        }

        /// <summary>
        /// Runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            Boolean condition = context.GetValue(Condition);

            if (condition == false)
            {
                return;
            }

            if (Body == null)
            {
                return;
            }

            context.ScheduleAction(Body);
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
        ///   Gets or sets the condition.
        /// </summary>
        /// <value>
        ///   The condition.
        /// </value>
        public InArgument<Boolean> Condition
        {
            get;
            set;
        }
    }
}