namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Model;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverPropertiesService"/>
    ///   class is used to attach designer properties to the an 
    ///   <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/>
    ///   activity <see cref="ModelItem"/>.
    /// </summary>
    public class InstanceResolverPropertiesService
    {
        /// <summary>
        ///   Stores the attached properties that wrap the resolution name properties on the activity.
        /// </summary>
        private readonly Dictionary<Type, InstanceResolverProperties> _resolveNameProperties = new Dictionary<Type, InstanceResolverProperties>();

        /// <summary>
        /// Attaches the specified model item.
        /// </summary>
        /// <param name="modelItem">
        /// The model item.
        /// </param>
        public static void Attach(ModelItem modelItem)
        {
            if (modelItem == null)
            {
                return;
            }
            
            Type itemType = modelItem.ItemType;

            if (itemType == null)
            {
                return;
            }

            EditingContext editingContext = modelItem.GetEditingContext();
            InstanceResolverPropertiesService propertiesService = editingContext.Services.GetService<InstanceResolverPropertiesService>();

            if (propertiesService == null)
            {
                propertiesService = new InstanceResolverPropertiesService();

                editingContext.Services.Publish(propertiesService);
            }

            propertiesService.AttachActivityTypeToContext(itemType, editingContext);
        }

        /// <summary>
        /// Attaches the activity type to context.
        /// </summary>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        /// <param name="editingContext">
        /// The editing context.
        /// </param>
        private void AttachActivityTypeToContext(Type ownerType, EditingContext editingContext)
        {
            Contract.Requires<ArgumentNullException>(ownerType != null);

            if (_resolveNameProperties.ContainsKey(ownerType))
            {
                return;
            }

            InstanceResolverProperties properties = new InstanceResolverProperties(ownerType);

            properties.AttachToContext(editingContext);

            _resolveNameProperties.Add(ownerType, properties);
        }
    }
}