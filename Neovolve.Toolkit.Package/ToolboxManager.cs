namespace Neovolve.Toolkit.Package
{
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The <see cref="ToolboxManager"/>
    ///   class is used to configure toolbox items for the toolkit.
    /// </summary>
    [ProvideAssemblyFilter("Neovolve.Toolkit.Package, Version=*, Culture=*, PublicKeyToken=*")]
    [Guid("1F9B5E78-A198-4CE2-8A6B-257B0CC58120")]
    public class ToolboxManager : IConfigureToolboxItem
    {
        /// <summary>
        /// Called by the toolbox service to configure <see cref="T:System.Drawing.Design.ToolboxItem"/> objects.
        /// </summary>
        /// <param name="item">
        /// [in] The <see cref="T:System.Drawing.Design.ToolboxItem"/> object whose configuration is to be modified.
        /// </param>
        public void ConfigureToolboxItem(ToolboxItem item)
        {
            if (item == null)
            {
                return;
            }

            ToolboxItemFilterAttribute[] filters = new[]
                                                   {
                                                       new ToolboxItemFilterAttribute("System.Activities.Activity", ToolboxItemFilterType.Require)
                                                   };

            item.Filter = filters;
        }
    }
}