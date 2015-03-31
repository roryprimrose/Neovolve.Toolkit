namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Model;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverTypeUpdater"/>
    ///   class is used to configure attached properties for an <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> to allow the types to be changed.
    /// </summary>
    public static class InstanceResolverTypeUpdater
    {
        /// <summary>
        ///   Defines the ArgumentCount property name.
        /// </summary>
        public const String ArgumentCountPropertyName = "ArgumentCount";

        /// <summary>
        ///   Defines the DisplayName property name.
        /// </summary>
        private const String DisplayNamePropertyName = "DisplayName";

        /// <summary>
        /// Updates the display name.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="fromValue">
        /// From value.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts have validated the parameter.")]
        public static void UpdateDisplayName(ModelItem modelItem, GenericArgumentCount fromValue)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);

            String fromDisplayName = BuildDisplayName(modelItem, fromValue);
            String currentDisplayName = GetDisplayName(modelItem);

            if (fromDisplayName.Equals(currentDisplayName, StringComparison.OrdinalIgnoreCase))
            {
                ForceDisplayNameUpdate(modelItem);
            }
        }

        /// <summary>
        /// Updates the type of the model.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="newType">
        /// The new type.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public static void UpdateModelType(ModelItem modelItem, Type newType)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);
            Contract.Requires<ArgumentNullException>(newType != null);

            Boolean displayNameRequiresUpdated = HasDefaultDisplayName(modelItem);
            GenericArgumentCount originalArgumentCount = GetArgumentCount(modelItem);
            EditingContext editingContext = modelItem.GetEditingContext();
            Object instanceOfNewType = Activator.CreateInstance(newType, originalArgumentCount);
            ModelItem newModelItem = ModelFactory.CreateItem(editingContext, instanceOfNewType);

            using (ModelEditingScope editingScope = newModelItem.BeginEdit("Change type argument"))
            {
                MorphHelper.MorphObject(modelItem, newModelItem);
                MorphHelper.MorphProperties(modelItem, newModelItem);

                if (displayNameRequiresUpdated)
                {
                    ForceDisplayNameUpdate(newModelItem);
                }

                DesignerUpdater.UpdateModelItem(modelItem, newModelItem);

                editingScope.Complete();
            }
        }

        /// <summary>
        /// Gets the argument count.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <returns>
        /// A <see cref="GenericArgumentCount"/> instance.
        /// </returns>
        internal static GenericArgumentCount GetArgumentCount(ModelItem modelItem)
        {
            if (modelItem == null)
            {
                return GenericArgumentCount.One;
            }

            ModelProperty argumentCountProperty = modelItem.Properties[ArgumentCountPropertyName];

            if (argumentCountProperty == null)
            {
                return GenericArgumentCount.One;
            }

            return (GenericArgumentCount)argumentCountProperty.ComputedValue;
        }

        /// <summary>
        /// Builds the display name.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="previousArgumentCount">
        /// The previous argument count.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String BuildDisplayName(ModelItem modelItem, GenericArgumentCount previousArgumentCount)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);

            String displayName = ActivityHelper.GenerateDisplayName(modelItem.ItemType, previousArgumentCount);

            displayName = displayName.Replace(" ", String.Empty);

            return displayName;
        }

        /// <summary>
        /// Forces the display name update.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        private static void ForceDisplayNameUpdate(ModelItem modelItem)
        {
            if (modelItem == null)
            {
                return;
            }

            if (modelItem.ItemType == null)
            {
                return;
            }

            GenericArgumentCount argumentCount = GetArgumentCount(modelItem);

            Contract.Assume(modelItem.ItemType != null);

            // The activity has a default display name for a different argument count
            String newDisplayName = ActivityHelper.GenerateDisplayName(modelItem.ItemType, argumentCount);

            // Assign the new display name
            ModelProperty displayNameProperty = modelItem.Properties[DisplayNamePropertyName];

            if (displayNameProperty != null)
            {
                displayNameProperty.SetValue(newDisplayName);
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String GetDisplayName(ModelItem modelItem)
        {
            if (modelItem == null)
            {
                return String.Empty;
            }

            ModelProperty displayNameProperty = modelItem.Properties[DisplayNamePropertyName];

            if (displayNameProperty == null)
            {
                return String.Empty;
            }

            String displayName = (String)displayNameProperty.ComputedValue;

            displayName = displayName.Replace(" ", String.Empty);

            return displayName;
        }

        /// <summary>
        /// Determines whether the specified model item has the default display name.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified model item has the default display name; otherwise, <c>false</c>.
        /// </returns>
        private static Boolean HasDefaultDisplayName(ModelItem modelItem)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);

            String currentDisplayName = GetDisplayName(modelItem);
            GenericArgumentCount argumentCount = GetArgumentCount(modelItem);

            Contract.Assume(modelItem.ItemType != null);

            String defaultDisplayName = BuildDisplayName(modelItem, argumentCount);

            if (currentDisplayName.Equals(defaultDisplayName, StringComparison.OrdinalIgnoreCase))
            {
                // The activity has its default display name
                return true;
            }

            return false;
        }
    }
}