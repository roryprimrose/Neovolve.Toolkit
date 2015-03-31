namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities;
    using System.Activities.Presentation.Metadata;
    using System.Activities.Presentation.Model;
    using System.Diagnostics.Contracts;
    using Neovolve.Toolkit.Workflow.Design.Presentation;

    /// <summary>
    /// The <see cref="RegisterMetadata"/>
    ///   class is used to register metadata for activity designers.
    /// </summary>
    public class RegisterMetadata : IRegisterMetadata
    {
        /// <summary>
        ///   Stores the generic definition of ActivityAction`16.
        /// </summary>
        private static readonly Type _activityActionGenericType = DetermineActivityActionType();

        /// <summary>
        /// Registers this instance.
        /// </summary>
        public void Register()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();

            ApplicationSettingDesigner.RegisterMetadata(builder);
            BusinessFailureEvaluatorDesigner.RegisterMetadata(builder);
            BusinessFailureScopeDesigner.RegisterMetadata(builder);
            DisposalScopeDesigner.RegisterMetadata(builder);
            ExecuteBookmarkDesigner.RegisterMetadata(builder);
            ExecuteBookmarkTDesigner.RegisterMetadata(builder);
            GetWorkflowInstanceIdDesigner.RegisterMetadata(builder);
            IfThenDesigner.RegisterMetadata(builder);
            InstanceResolverDesigner.RegisterMetadata(builder);
            SystemFailureEvaluatorDesigner.RegisterMetadata(builder);
            ReceiveIdentityInspectorDesigner.RegisterMetadata(builder);
            RetryDesigner.RegisterMetadata(builder);

            MetadataStore.AddAttributeTable(builder.CreateTable());

            MorphHelper.AddPropertyValueMorphHelper(_activityActionGenericType, MorphExtension.MorphActivityAction);
        }

        /// <summary>
        /// Determines the type of the activity action.
        /// </summary>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        private static Type DetermineActivityActionType()
        {
            Type closedType =
                typeof(
                    ActivityAction
                        <Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, 
                            Object>);

            Contract.Assume(closedType.IsGenericType);

            return closedType.GetGenericTypeDefinition();
        }
    }
}