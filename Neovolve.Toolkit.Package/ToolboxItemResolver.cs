namespace Neovolve.Toolkit.Package
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Design;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The <see cref="ToolboxItemResolver"/>
    ///   class is used to return all the <see cref="ToolboxItem"/>
    ///   classes found in the current assembly.
    /// </summary>
    internal static class ToolboxItemResolver
    {
        /// <summary>
        /// Gets the toolbox items defined in this assembly.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        public static IEnumerable<ToolboxItem> GetToolboxItems()
        {
            Assembly callingAssembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> itemsFound = from x in callingAssembly.GetTypes()
                                           where typeof(ToolboxItem).IsAssignableFrom(x) && x.IsAbstract == false
                                           select x;

            IEnumerable<ToolboxItem> itemsCreated = from x in itemsFound
                                                    orderby x.Name
                                                    select Activator.CreateInstance(x) as ToolboxItem;

            return new List<ToolboxItem>(itemsCreated);
        }
    }
}