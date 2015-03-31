namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ApplicationSettingToolboxItem"/>
    ///   class is used to provide a toolbox item for the <see cref="ApplicationSetting{T}"/> activity.
    /// </summary>
    [ToolboxBitmap(typeof(ApplicationSettingToolboxItem), "cog.bmp")]
    [Description("Returns a strongly typed application configuration value")]
    [Serializable]
    internal class ApplicationSettingToolboxItem : ActivityToolboxItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ApplicationSettingToolboxItem" /> class.
        /// </summary>
        public ApplicationSettingToolboxItem()
            : base(typeof(ApplicationSettingToolboxItem), typeof(ApplicationSetting<>))
        {
        }
    }
}