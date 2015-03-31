namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.Design.Factories;

    /// <summary>
    /// The <see cref="InstanceResolverToolboxItem"/>
    ///   class is used to provide a toolbox item for the 
    ///   <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(InstanceResolverToolboxItem), "brick.bmp")]
    [Description("Resolves IoC dependencies")]
    [Serializable]
    internal class InstanceResolverToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Stores the generic type definition of InstanceResolver.
        /// </summary>
        private static readonly Type _instanceResolverT16GenericType =
            typeof(
                InstanceResolver
                    <Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object>).
                GetGenericTypeDefinition();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InstanceResolverToolboxItem" /> class.
        /// </summary>
        public InstanceResolverToolboxItem()
            : base(typeof(InstanceResolverToolboxItem), _instanceResolverT16GenericType, typeof(InstanceResolverFactory))
        {
        }
    }
}