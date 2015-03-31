namespace Neovolve.Toolkit.Package.Toolbox
{
    using System;
    using System.Drawing.Design;

    /// <summary>
    /// The <see cref="ActivityToolboxItem"/>
    ///   class is used to define a toolbox item for a workflow activity.
    /// </summary>
    internal abstract class ActivityToolboxItem : ToolboxItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityToolboxItem"/> class.
        /// </summary>
        /// <param name="metadataType">
        /// Type that describtes the metadata for the toolbox item.
        /// </param>
        /// <param name="activityType">
        /// Type of the activity created by the toolbox item.
        /// </param>
        protected ActivityToolboxItem(Type metadataType, Type activityType)
            : this(metadataType, activityType, activityType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityToolboxItem"/> class.
        /// </summary>
        /// <param name="metadataType">
        /// Type of the metadata.
        /// </param>
        /// <param name="activityType">
        /// Type of the activity.
        /// </param>
        /// <param name="factoryType">
        /// Type of the factory.
        /// </param>
        protected ActivityToolboxItem(Type metadataType, Type activityType, Type factoryType)
            : base(metadataType)
        {
            DisplayName = CalculateDisplayName(activityType);
            AssemblyName = activityType.Assembly.GetName();
            TypeName = factoryType.AssemblyQualifiedName;
        }

        /// <summary>
        /// Calculates the display name.
        /// </summary>
        /// <param name="activityType">
        /// Type of the activity.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String CalculateDisplayName(Type activityType)
        {
            String activityName = activityType.Name;

            // Strip the `x from the name
            Int32 genericMarkerIndex = activityName.IndexOf('`');

            if (genericMarkerIndex > -1)
            {
                activityName = activityName.Substring(0, genericMarkerIndex);
            }

            if (activityType.IsGenericType)
            {
                activityName += "<T>";
            }

            return activityName;
        }
    }
}