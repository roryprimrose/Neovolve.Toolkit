namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="GetWorkflowInstanceIdToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="GetWorkflowInstanceId"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(GetWorkflowInstanceIdToolboxItem), "wrench.bmp")]
    [Description("Returns the instance id of the current workflow")]
    [Serializable]
    internal class GetWorkflowInstanceIdToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "GetWorkflowInstanceIdToolboxItem" /> class.
        /// </summary>
        public GetWorkflowInstanceIdToolboxItem()
            : base(typeof(GetWorkflowInstanceIdToolboxItem), typeof(GetWorkflowInstanceId))
        {
        }
    }
}