namespace Neovolve.Toolkit.Workflow.Design.Factories
{
    using System;
    using System.Activities;
    using System.Activities.Presentation;
    using System.Activities.Statements;
    using System.Windows;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverFactory"/>
    ///   class is used to create an instance of the 
    ///   <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> activity with a default configuration.
    /// </summary>
    public sealed class InstanceResolverFactory : IActivityTemplateFactory
    {
        /// <summary>
        /// Creates the specified target.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// A <see cref="Activity"/> instance.
        /// </returns>
        public Activity Create(DependencyObject target)
        {
            return new InstanceResolver
                <Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object>
                   {
                       Body =
                           {
                               Handler = new Sequence()
                           }
                   };
        }
    }
}