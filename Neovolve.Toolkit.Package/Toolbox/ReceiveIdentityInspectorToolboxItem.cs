namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ReceiveIdentityInspectorToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="ReceiveIdentityInspector{T}"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(ReceiveIdentityInspectorToolboxItem), "user.bmp")]
    [Description("Returns the identity invoking the Receive activity")]
    [Serializable]
    internal class ReceiveIdentityInspectorToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ReceiveIdentityInspectorToolboxItem" /> class.
        /// </summary>
        public ReceiveIdentityInspectorToolboxItem()
            : base(typeof(ReceiveIdentityInspectorToolboxItem), typeof(ReceiveIdentityInspector<>))
        {
        }
    }
}