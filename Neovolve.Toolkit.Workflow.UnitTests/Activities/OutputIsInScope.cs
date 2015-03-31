namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Neovolve.Toolkit.Workflow.ExecutionProperties;

    /// <summary>
    /// The <see cref="OutputIsInScope&lt;T&gt;"/>
    ///   class is used to determine whether the activity is executing within a failure scope.
    /// </summary>
    /// <typeparam name="T">
    /// The type of failure code.
    /// </typeparam>
    public sealed class OutputIsInScope<T> : NativeActivity<Boolean> where T : struct
    {
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

            Object injector = context.Properties.Find(BusinessFailureTracker<T>.Name);
            Boolean isInScope = injector != null;

            Result.Set(context, isInScope);
        }
    }
}