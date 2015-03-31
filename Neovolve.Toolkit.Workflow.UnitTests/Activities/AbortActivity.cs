namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="AbortActivity"/>
    ///   class is a workflow activity that is used to abort the current workflow.
    /// </summary>
    public sealed class AbortActivity : NativeActivity
    {
        /// <summary>
        /// When implemented in a derived class, runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            Exception reason = Reason.Get(context);

            context.Abort(reason);
        }

        /// <summary>
        ///   Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        public InArgument<Exception> Reason
        {
            get;
            set;
        }
    }
}