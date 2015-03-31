namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="BusinessFailureScopeToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="BusinessFailureScope{T}"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(BusinessFailureScopeToolboxItem), "shield_go.bmp")]
    [Description("Collects business failures to throw a BusinessFailureException")]
    [Serializable]
    internal class BusinessFailureScopeToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BusinessFailureScopeToolboxItem" /> class.
        /// </summary>
        public BusinessFailureScopeToolboxItem()
            : base(typeof(BusinessFailureScopeToolboxItem), typeof(BusinessFailureScope<>))
        {
        }
    }
}