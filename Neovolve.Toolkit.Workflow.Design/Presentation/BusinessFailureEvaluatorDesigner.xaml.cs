namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="BusinessFailureEvaluatorDesigner"/>
    ///   class is used to provide design time support for the <see cref="BusinessFailureEvaluator{T}"/> class.
    /// </summary>
    public partial class BusinessFailureEvaluatorDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BusinessFailureEvaluatorDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public BusinessFailureEvaluatorDesigner()
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
            builder.AddCustomAttributes(typeof(BusinessFailureEvaluator<>), new DesignerAttribute(typeof(BusinessFailureEvaluatorDesigner)));
            builder.AddCustomAttributes(typeof(BusinessFailureEvaluator<>), new DefaultTypeArgumentAttribute(typeof(Int32)));
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