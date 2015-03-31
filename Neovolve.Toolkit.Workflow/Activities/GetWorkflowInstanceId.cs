namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="GetWorkflowInstanceId"/>
    ///   class is used to obtain the instance id of the executing workflow.
    /// </summary>
    public sealed class GetWorkflowInstanceId : CodeActivity<Guid>
    {
        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            // Nothing to do here
            // This method is overridden here in order to prevent reflection of properties on this activity which do not exist
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// A <see cref="Guid"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override Guid Execute(CodeActivityContext context)
        {
            Contract.Assume(context != null);

            return context.WorkflowInstanceId;
        }
    }
}