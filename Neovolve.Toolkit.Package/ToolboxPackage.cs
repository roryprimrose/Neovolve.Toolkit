namespace Neovolve.Toolkit.Package
{
    using System;
    using System.Activities.Presentation;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing.Design;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// The <see cref="ToolboxPackage"/>
    ///   class is used to provide a package to Visual Studio to manage the toolbox configuration for the toolkit.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideToolboxItems(1, true)]
    [ProvideToolboxFormat("CF_WORKFLOW_4")]
    // [ProvideToolboxItemConfiguration(typeof(ToolboxManager))]
    [Guid("1989DAC3-5C27-47C3-8210-E3F8EEE9B82A")]
    public sealed class ToolboxPackage : Package
    {
        /// <summary>
        ///   The category tab prefix.
        /// </summary>
        private const String CategoryTabPrefix = "Neovolve.Toolkit";

        /// <summary>
        ///   The company header.
        /// </summary>
        private const String CompanyHeader = "Company:";

        /// <summary>
        ///   The component type header.
        /// </summary>
        private const String ComponentTypeHeader = "ComponentType:";

        /// <summary>
        ///   The description header.
        /// </summary>
        private const String DescriptionHeader = "Description:";

        /// <summary>
        ///   The name header.
        /// </summary>
        private const String NameHeader = "Name:";

        /// <summary>
        ///   The tooltip format.
        /// </summary>
        private const String TooltipFormat = "VSToolboxTipInfo";

        /// <summary>
        ///   The version header.
        /// </summary>
        private const String VersionHeader = "Version:";

        /// <summary>
        ///   Determines the category tab name.
        /// </summary>
        private static readonly String _categoryTab = GenerateCategoryTabName();

        /// <summary>
        ///   Stores the set of toolbox items.
        /// </summary>
        private IEnumerable<ToolboxItem> _toolboxItemList;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ToolboxPackage" /> class.
        /// </summary>
        public ToolboxPackage()
        {
            ToolboxInitialized += OnRefreshToolbox;
            ToolboxUpgraded += OnRefreshToolbox;
        }

        /// <summary>
        /// Builds the tooltip stream.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> instance.
        /// </returns>
        internal static Stream BuildTooltipStream(ToolboxItem item)
        {
            Char ch = (Char)((1 + NameHeader.Length) + item.DisplayName.Length);
            String str = ch + NameHeader + item.DisplayName;
            if (!String.IsNullOrEmpty(item.Version))
            {
                ch = (Char)((1 + VersionHeader.Length) + item.Version.Length);
                str = str + ch + VersionHeader + item.Version;
            }

            if (!String.IsNullOrEmpty(item.Company))
            {
                ch = (Char)((1 + CompanyHeader.Length) + item.Company.Length);
                str = str + ch + CompanyHeader + item.Company;
            }

            String str2 = "Managed .NET Component";
            if (!String.IsNullOrEmpty(str2))
            {
                ch = (Char)((1 + ComponentTypeHeader.Length) + str2.Length);
                str = str + ch + ComponentTypeHeader + str2;
            }

            if (!String.IsNullOrEmpty(item.Description))
            {
                str = str + ((Char)((1 + DescriptionHeader.Length) + item.Description.Length)) + DescriptionHeader + item.Description;
            }

            str = str + '\0';

            return SaveStringToStreamRaw(str);
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        ///   where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Use the toolbox service to get a list of all toolbox items in this assembly.
            _toolboxItemList = ToolboxItemResolver.GetToolboxItems();
        }

        /// <summary>
        /// Adds the toolbox items.
        /// </summary>
        /// <param name="toolbox">
        /// The toolbox.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        private static void AddToolboxItems(IVsToolbox toolbox, ToolboxItem item)
        {
            OleDataObject dataObject = new OleDataObject();
            Stream tooltipStream = BuildTooltipStream(item);

            dataObject.SetData(TooltipFormat, tooltipStream);
            dataObject.SetData("CF_WORKFLOW_4", item.DisplayName);
            dataObject.SetData(DragDropHelper.WorkflowItemTypeNameFormat, item.TypeName);
            dataObject.SetData("AssemblyName", item.AssemblyName);

            TBXITEMINFO[] toolboxItemInfo = new TBXITEMINFO[1];
            toolboxItemInfo[0].bstrText = item.DisplayName;

            if (item.Bitmap != null)
            {
                toolboxItemInfo[0].hBmp = item.Bitmap.GetHbitmap();

                // toolboxItemInfo[0].clrTransparent = (UInt32)ColorTranslator.ToWin32(Color.White);
            }

            toolbox.AddItem(dataObject, toolboxItemInfo, _categoryTab);
        }

        /// <summary>
        /// Generates the name of the category tab.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String GenerateCategoryTabName()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            String assemblyLocation = currentAssembly.Location;

            if (File.Exists(assemblyLocation))
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
                const String CategoryFormat = "{0} {1}.{2}";

                return String.Format(
                    CultureInfo.CurrentUICulture, CategoryFormat, CategoryTabPrefix, versionInfo.ProductMajorPart, versionInfo.ProductMinorPart);
            }

            return CategoryTabPrefix;
        }

        /// <summary>
        /// The save string to stream raw.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "The instance cannot be disposed as it is the return value.")]
        private static Stream SaveStringToStreamRaw(String value)
        {
            Byte[] bytes = new UnicodeEncoding().GetBytes(value);
            MemoryStream stream;

            if ((bytes != null) && (bytes.Length > 0))
            {
                stream = new MemoryStream(bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            else
            {
                stream = new MemoryStream();
            }

            stream.WriteByte(0);
            stream.WriteByte(0);
            stream.Flush();
            stream.Position = 0L;

            return stream;
        }

        /// <summary>
        /// The on refresh toolbox.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnRefreshToolbox(Object sender, EventArgs e)
        {
            IVsToolbox toolbox = GetService(typeof(IVsToolbox)) as IVsToolbox;

            if (toolbox == null)
            {
                return;
            }

            toolbox.RemoveTab(_categoryTab);

            if (_toolboxItemList.Count() <= 0)
            {
                return;
            }

            if (toolbox.AddTab(_categoryTab) != VSConstants.S_OK)
            {
                throw new ApplicationException("Failed to add the tab '" + _categoryTab + "'.");
            }

            // Recreate the target tab with the items from the current list. 
            foreach (ToolboxItem item in _toolboxItemList)
            {
                AddToolboxItems(toolbox, item);
            }

            toolbox.SelectTab(_categoryTab);
        }
    }
}