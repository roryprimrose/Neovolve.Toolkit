namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="BusinessFailureScopeDesigner"/>
    ///   class is used to provide design time support for the <see cref="BusinessFailureScope{T}"/> class.
    /// </summary>
    public partial class BusinessFailureScopeDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BusinessFailureScopeDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public BusinessFailureScopeDesigner()
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
            builder.AddCustomAttributes(typeof(BusinessFailureScope<>), new DesignerAttribute(typeof(BusinessFailureScopeDesigner)));
            builder.AddCustomAttributes(typeof(BusinessFailureScope<>), new DefaultTypeArgumentAttribute(typeof(Int32)));
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