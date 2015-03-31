namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Controls;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverDesigner"/>
    ///   class is used to provide design time support for the <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> class.
    /// </summary>
    public partial class InstanceResolverDesigner
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
        ///   Initializes a new instance of the <see cref = "InstanceResolverDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public InstanceResolverDesigner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Registers the metadata.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        internal static void RegisterMetadata(AttributeTableBuilder builder)
        {
            builder.AddCustomAttributes(_instanceResolverT16GenericType, new DesignerAttribute(typeof(InstanceResolverDesigner)));
        }

        /// <summary>
        /// Called when the model item has been changed.
        /// </summary>
        /// <param name="newItem">
        /// The new item.
        /// </param>
        protected override void OnModelItemChanged(Object newItem)
        {
            InstanceResolverPropertiesService.Attach(ModelItem);

            base.OnModelItemChanged(newItem);
        }

        /// <summary>
        /// Arguments the count changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void ArgumentCountChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count < 1)
            {
                return;
            }

            GenericArgumentCount fromValue = e.RemovedItems.OfType<GenericArgumentCount>().First();

            InstanceResolverTypeUpdater.UpdateDisplayName(ModelItem, fromValue);
        }
    }
}