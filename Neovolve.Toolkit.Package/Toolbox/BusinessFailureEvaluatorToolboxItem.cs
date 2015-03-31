namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="BusinessFailureEvaluatorToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="BusinessFailureEvaluator{T}"/> activity.
    /// </summary>
    [Description("Evaluates a condition to build a BusinessFailureException<T> instance")]
    [ToolboxBitmap(typeof(BusinessFailureEvaluatorToolboxItem), "shield.bmp")]
    [Serializable]
    internal class BusinessFailureEvaluatorToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BusinessFailureEvaluatorToolboxItem" /> class.
        /// </summary>
        public BusinessFailureEvaluatorToolboxItem()
            : base(typeof(BusinessFailureEvaluatorToolboxItem), typeof(BusinessFailureEvaluator<>))
        {
        }
    }
}