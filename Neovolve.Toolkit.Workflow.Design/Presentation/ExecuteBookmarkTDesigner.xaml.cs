namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ExecuteBookmarkTDesigner"/>
    ///   class is used to provide design time support for the <see cref="ExecuteBookmark{T}"/> class.
    /// </summary>
    public partial class ExecuteBookmarkTDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ExecuteBookmarkTDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public ExecuteBookmarkTDesigner()
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
            builder.AddCustomAttributes(typeof(ExecuteBookmark<>), new DesignerAttribute(typeof(ExecuteBookmarkTDesigner)));
            builder.AddCustomAttributes(typeof(ExecuteBookmark<>), new DefaultTypeArgumentAttribute(typeof(String)));
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