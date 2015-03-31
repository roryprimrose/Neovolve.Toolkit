namespace Neovolve.Toolkit.Workflow.Design.Factories
{
    using System.Activities;
    using System.Activities.Presentation;
    using System.Activities.Statements;
    using System.Windows;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="IfThenFactory"/>
    ///   class is used to create an instance of the <see cref="IfThen"/> activity with a default configuration.
    /// </summary>
    public sealed class IfThenFactory : IActivityTemplateFactory
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
            return new IfThen
                   {
                       Body =
                           {
                               Handler = new Sequence()
                           }
                   };
        }
    }
}