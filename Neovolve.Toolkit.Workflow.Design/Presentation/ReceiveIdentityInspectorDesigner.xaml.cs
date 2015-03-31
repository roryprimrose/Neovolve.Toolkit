namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Security.Principal;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ReceiveIdentityInspectorDesigner"/>
    ///   class is used to provide desig time support for the <see cref="ReceiveIdentityInspector{T}"/> class.
    /// </summary>
    public partial class ReceiveIdentityInspectorDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ReceiveIdentityInspectorDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public ReceiveIdentityInspectorDesigner()
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
            builder.AddCustomAttributes(typeof(ReceiveIdentityInspector<>), new DesignerAttribute(typeof(ReceiveIdentityInspectorDesigner)));
            builder.AddCustomAttributes(typeof(ReceiveIdentityInspector<>), new DefaultTypeArgumentAttribute(typeof(IIdentity)));
        }

        /// <summary>
        /// Called when the model item has been changed.
        /// </summary>
        /// <param name="newItem">
        /// The new item.
        /// </param>
        protected override void OnModelItemChanged(Object newItem)
        {
            base.OnModelItemChanged(newItem);

            GenericArgumentTypeUpdater.Attach(ModelItem);
        }
    }
}