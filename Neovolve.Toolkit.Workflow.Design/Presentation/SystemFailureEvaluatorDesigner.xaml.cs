namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="SystemFailureEvaluatorDesigner"/>
    ///   class is used to provide design time support for the <see cref="SystemFailureEvaluator"/> class.
    /// </summary>
    public partial class SystemFailureEvaluatorDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "SystemFailureEvaluatorDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public SystemFailureEvaluatorDesigner()
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
            builder.AddCustomAttributes(typeof(SystemFailureEvaluator), new DesignerAttribute(typeof(SystemFailureEvaluatorDesigner)));
        }
    }
}