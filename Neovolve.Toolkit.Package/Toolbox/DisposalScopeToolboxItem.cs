namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.Design.Factories;

    /// <summary>
    /// The <see cref="DisposalScopeToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="DisposalScope{T}"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(DisposalScopeToolboxItem), "bin_closed.bmp")]
    [Description("Disposes an IDisposable instance on completion")]
    [Serializable]
    internal class DisposalScopeToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DisposalScopeToolboxItem" /> class.
        /// </summary>
        public DisposalScopeToolboxItem()
            : base(typeof(DisposalScopeToolboxItem), typeof(DisposalScope<>), typeof(DisposalScopeFactory))
        {
        }
    }
}