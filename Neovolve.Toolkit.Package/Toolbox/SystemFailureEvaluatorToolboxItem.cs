namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="SystemFailureEvaluatorToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="SystemFailureEvaluator"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(SystemFailureEvaluatorToolboxItem), "bug.bmp")]
    [Description("Throws an exception when a condition is true")]
    [Serializable]
    internal class SystemFailureEvaluatorToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "SystemFailureEvaluatorToolboxItem" /> class.
        /// </summary>
        public SystemFailureEvaluatorToolboxItem()
            : base(typeof(SystemFailureEvaluatorToolboxItem), typeof(SystemFailureEvaluator))
        {
        }
    }
}