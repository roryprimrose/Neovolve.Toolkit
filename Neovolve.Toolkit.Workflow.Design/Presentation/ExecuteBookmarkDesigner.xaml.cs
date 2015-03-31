namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ExecuteBookmarkDesigner"/>
    ///   class is used to provide design time support for the <see cref="ExecuteBookmark"/> class.
    /// </summary>
    public partial class ExecuteBookmarkDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ExecuteBookmarkDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public ExecuteBookmarkDesigner()
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
            builder.AddCustomAttributes(typeof(ExecuteBookmark), new DesignerAttribute(typeof(ExecuteBookmarkDesigner)));
        }
    }
}