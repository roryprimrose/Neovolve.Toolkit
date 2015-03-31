namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Model;
    using System.Diagnostics.Contracts;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverProperties"/>
    ///   class is used to attach properties to an owning 
    ///   <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> 
    ///   activity type.
    /// </summary>
    internal class InstanceResolverProperties
    {
        /// <summary>
        ///   Stores the owner activity type.
        /// </summary>
        private readonly Type _ownerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceResolverProperties"/> class.
        /// </summary>
        /// <param name="instanceResolverType">
        /// Type of the instance resolver.
        /// </param>
        public InstanceResolverProperties(Type instanceResolverType)
        {
            Contract.Requires<ArgumentNullException>(instanceResolverType != null);

            _ownerType = instanceResolverType;
        }

        /// <summary>
        /// Attaches to context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void AttachToContext(EditingContext context)
        {
            AttachedPropertiesService attachedPropertiesService = context.Services.GetRequiredService<AttachedPropertiesService>();

            for (Int32 index = 0; index < 16; index++)
            {
                AttachUpdatableArgumentType(_ownerType, attachedPropertiesService, index);
            }
        }

        /// <summary>
        /// Attaches the type of the updatable argument.
        /// </summary>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        /// <param name="attachedPropertiesService">
        /// The attached properties service.
        /// </param>
        /// <param name="argumentIndex">
        /// Index of the argument.
        /// </param>
        private static void AttachUpdatableArgumentType(Type ownerType, AttachedPropertiesService attachedPropertiesService, Int32 argumentIndex)
        {
            String propertyName = "ArgumentType" + (argumentIndex + 1);

            AttachedProperty<Type> attachedProperty = new AttachedProperty<Type>
                                                      {
                                                          Name = propertyName, 
                                                          OwnerType = ownerType, 
                                                          IsBrowsable = true, 
                                                          Getter = arg => GetTypeArgument(arg, argumentIndex), 
                                                          Setter = (arg, newType) => UpdateTypeArgument(arg, argumentIndex, newType)
                                                      };

            attachedPropertiesService.AddProperty(attachedProperty);
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

            InstanceResolverTypeUpdater.UpdateModelType(modelItem, newType);
        }
    }
}