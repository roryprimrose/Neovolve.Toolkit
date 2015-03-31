namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="IfThenDesigner"/>
    ///   class is used to provide desig time support for the <see cref="IfThen"/> class.
    /// </summary>
    public partial class IfThenDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IfThenDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public IfThenDesigner()
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
            builder.AddCustomAttributes(typeof(IfThen), new DesignerAttribute(typeof(IfThenDesigner)));
        }
    }
}