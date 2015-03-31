namespace Neovolve.Toolkit.Workflow.Design.Factories
{
    using System;
    using System.Activities;
    using System.Activities.Presentation;
    using System.Activities.Statements;
    using System.Windows;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="DisposalScopeFactory"/>
    ///   class is used to create an instance of the <see cref="DisposalScope{T}"/> activity with a default configuration.
    /// </summary>
    public sealed class DisposalScopeFactory : IActivityTemplateFactory
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// A <see cref="Activity"/> instance.
        /// </returns>
        public Activity Create(DependencyObject target)
        {
            return new DisposalScope<IDisposable>
                   {
                       Body =
                           {
                               Handler = new Sequence()
                           }
                   };
        }
    }
}