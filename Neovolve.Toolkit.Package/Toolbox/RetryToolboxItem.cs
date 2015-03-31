namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.Design.Factories;

    /// <summary>
    /// The <see cref="RetryToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="Retry"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(RetryToolboxItem), "arrow_retry.bmp")]
    [Description("Retries the child activity when it throws an exception")]
    [Serializable]
    internal class RetryToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "RetryToolboxItem" /> class.
        /// </summary>
        public RetryToolboxItem()
            : base(typeof(RetryToolboxItem), typeof(Retry), typeof(RetryFactory))
        {
        }
    }
}