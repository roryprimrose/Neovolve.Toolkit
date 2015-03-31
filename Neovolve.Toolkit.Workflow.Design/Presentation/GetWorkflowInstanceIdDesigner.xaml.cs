namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="GetWorkflowInstanceIdDesigner"/>
    ///   class is used to provide design time support for the <see cref="GetWorkflowInstanceId"/> class.
    /// </summary>
    [DebuggerNonUserCode]
    public partial class GetWorkflowInstanceIdDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "GetWorkflowInstanceIdDesigner" /> class.
        /// </summary>
        public GetWorkflowInstanceIdDesigner()
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
            builder.AddCustomAttributes(typeof(GetWorkflowInstanceId), new DesignerAttribute(typeof(GetWorkflowInstanceIdDesigner)));
        }
    }
}