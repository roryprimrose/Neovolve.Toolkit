namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The execute bookmark t toolbox item.
    /// </summary>
    [ToolboxBitmap(typeof(ExecuteBookmarkTToolboxItem), "book_open.bmp")]
    [Description("Creates a bookmark that accepts a strongly typed resumption value")]
    [Serializable]
    internal class ExecuteBookmarkTToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ExecuteBookmarkTToolboxItem" /> class.
        /// </summary>
        public ExecuteBookmarkTToolboxItem()
            : base(typeof(ExecuteBookmarkTToolboxItem), typeof(ExecuteBookmark<>))
        {
        }
    }
}