namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Model;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// The <see cref="GenericArgumentTypeUpdater"/>
    ///   class is used to configure attached properties for a generic type to allow the types to be changed.
    /// </summary>
    public static class GenericArgumentTypeUpdater
    {
        /// <summary>
        ///   Defines the DisplayName property name.
        /// </summary>
        private const String DisplayName = "DisplayName";

        /// <summary>
        /// Attaches the specified model item.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts have validated the parameter.")]
        public static void Attach(ModelItem modelItem)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);

            Attach(modelItem, Int32.MaxValue);
        }

        /// <summary>
        /// Attaches the specified model item.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="maximumUpdatableTypes">
        /// The maximum updatable types.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public static void Attach(ModelItem modelItem, Int32 maximumUpdatableTypes)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);

            Type[] genericArguments = modelItem.ItemType.GetGenericArguments();

            if (genericArguments.Any() == false)
            {
                return;
            }

            Int32 argumentCount = genericArguments.Length;
            Int32 updatableArgumentCount = Math.Min(argumentCount, maximumUpdatableTypes);
            EditingContext context = modelItem.GetEditingContext();
            AttachedPropertiesService attachedPropertiesService = context.Services.GetService<AttachedPropertiesService>();

            for (Int32 index = 0; index < updatableArgumentCount; index++)
            {
                AttachUpdatableArgumentType(modelItem, attachedPropertiesService, index, updatableArgumentCount);
            }
        }

        /// <summary>
        /// Attaches the type of the updatable argument.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="attachedPropertiesService">
        /// The attached properties service.
        /// </param>
        /// <param name="argumentIndex">
        /// Index of the argument.
        /// </param>
        /// <param name="argumentCount">
        /// The argument count.
        /// </param>
        private static void AttachUpdatableArgumentType(
            ModelItem modelItem, AttachedPropertiesService attachedPropertiesService, Int32 argumentIndex, Int32 argumentCount)
        {
            String propertyName = "ArgumentType";

            if (argumentCount > 1)
            {
                propertyName += argumentIndex + 1;
            }

            AttachedProperty<Type> attachedProperty = new AttachedProperty<Type>
                                                      {
                                                          Name = propertyName, 
                                                          OwnerType = modelItem.ItemType, 
                                                          IsBrowsable = true
                                                      };

            attachedProperty.Getter = (ModelItem arg) => GetTypeArgument(arg, argumentIndex);
            attachedProperty.Setter = (ModelItem arg, Type newType) => UpdateTypeArgument(arg, argumentIndex, newType);

            attachedPropertiesService.AddProperty(attachedProperty);
        }

        /// <summary>
        /// Displays the name requires update.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private static Boolean DisplayNameRequiresUpdate(ModelItem modelItem)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);

            String currentDisplayName = (String)modelItem.Properties[DisplayName].ComputedValue;

            // Sometimes the display name is empty
            if (String.IsNullOrWhiteSpace(currentDisplayName))
            {
                return true;
            }

            // The default calculation of a generic type does not include spaces in the generic type arguments
            // However an activity might include these as the default display name
            // Strip spaces to provide a more accurate match
            String defaultDisplayName = GetActivityDefaultName(modelItem.ItemType);

            currentDisplayName = currentDisplayName.Replace(" ", String.Empty);
            defaultDisplayName = defaultDisplayName.Replace(" ", String.Empty);

            if (String.Equals(currentDisplayName, defaultDisplayName, StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the default name of the activity.
        /// </summary>
        /// <param name="activityType">
        /// Type of the activity.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String GetActivityDefaultName(Type activityType)
        {
            Contract.Requires<ArgumentNullException>(activityType != null);

            Activity activity = (Activity)Activator.CreateInstance(activityType);

            return activity.DisplayName;
        }

        /// <summary>
        /// Gets the type argument.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="argumentIndex">
        /// Index of the argument.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        private static Type GetTypeArgument(ModelItem modelItem, Int32 argumentIndex)
        {
            return modelItem.ItemType.GetGenericArguments()[argumentIndex];
        }

        /// <summary>
        /// Updates the type argument.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        /// <param name="argumentIndex">
        /// Index of the argument.
        /// </param>
        /// <param name="newGenericType">
        /// The new type.
        /// </param>
        private static void UpdateTypeArgument(ModelItem modelItem, Int32 argumentIndex, Type newGenericType)
        {
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType != null);
            Contract.Requires<ArgumentNullException>(modelItem.ItemType.IsGenericType);

            Type itemType = modelItem.ItemType;
            Type[] genericTypes = itemType.GetGenericArguments();

            // Replace the type being changed
            genericTypes[argumentIndex] = newGenericType;

            Type genericDefinition = itemType.GetGenericTypeDefinition();

            Contract.Assume(genericDefinition.IsGenericTypeDefinition);
            Contract.Assume(genericDefinition.GetGenericArguments().Length == genericTypes.Length);

            Type newType = genericDefinition.MakeGenericType(genericTypes);
            EditingContext editingContext = modelItem.GetEditingContext();
            Object instanceOfNewType = Activator.CreateInstance(newType);
            ModelItem newModelItem = ModelFactory.CreateItem(editingContext, instanceOfNewType);

            using (ModelEditingScope editingScope = newModelItem.BeginEdit("Change type argument"))
            {
                MorphHelper.MorphObject(modelItem, newModelItem);
                MorphHelper.MorphProperties(modelItem, newModelItem);

                if (itemType.IsSubclassOf(typeof(Activity)) && newType.IsSubclassOf(typeof(Activity)))
                {
                    Contract.Assume(modelItem.ItemType != null);

                    if (DisplayNameRequiresUpdate(modelItem))
                    {
                        // Update to the new display name
                        String newDisplayName = GetActivityDefaultName(newType);

                        newModelItem.Properties[DisplayName].SetValue(newDisplayName);
                    }
                }

                DesignerUpdater.UpdateModelItem(modelItem, newModelItem);

                editingScope.Complete();
            }
        }
    }
}