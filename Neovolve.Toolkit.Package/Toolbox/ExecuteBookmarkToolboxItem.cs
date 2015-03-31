namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ExecuteBookmarkToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="ExecuteBookmark"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(ExecuteBookmarkToolboxItem), "book_open.bmp")]
    [Description("Creates a bookmark that does not accept a resumption value")]
    [Serializable]
    internal class ExecuteBookmarkToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ExecuteBookmarkToolboxItem" /> class.
        /// </summary>
        public ExecuteBookmarkToolboxItem()
            : base(typeof(ExecuteBookmarkToolboxItem), typeof(ExecuteBookmark))
        {
        }
    }
}