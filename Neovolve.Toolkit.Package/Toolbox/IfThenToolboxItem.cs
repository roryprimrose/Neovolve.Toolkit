namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.Design.Factories;

    /// <summary>
    /// The <see cref="IfThenToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="IfThen"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(IfThenToolboxItem), "arrow_branch.bmp")]
    [Description("Executes the body if a condition is true")]
    [Serializable]
    internal class IfThenToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IfThenToolboxItem" /> class.
        /// </summary>
        public IfThenToolboxItem()
            : base(typeof(IfThenToolboxItem), typeof(IfThen), typeof(IfThenFactory))
        {
        }
    }
}