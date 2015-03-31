namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities;
    using System.Activities.Presentation.Model;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="MorphExtension"/>
    ///   class is used to define morph extensions for activity designer ModelItem transformations.
    /// </summary>
    internal static class MorphExtension
    {
        /// <summary>
        /// Morphs the activity action.
        /// </summary>
        /// <param name="originalValue">
        /// The original value.
        /// </param>
        /// <param name="newModelProperty">
        /// The new model property.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        public static Object MorphActivityAction(ModelItem originalValue, ModelProperty newModelProperty)
        {
            Contract.Requires<ArgumentNullException>(newModelProperty.PropertyType != null);

            Type newActivityActionType = newModelProperty.PropertyType;
            ActivityDelegate newActivityDelegate = (ActivityDelegate)Activator.CreateInstance(newActivityActionType);
            ModelItem newModelItem = ModelFactory.CreateItem(originalValue.GetEditingContext(), newActivityDelegate);
            Type[] genericArguments = newActivityActionType.GetGenericArguments();

            for (Int32 index = 1; index <= genericArguments.Length; index++)
            {
                String argumentName = "Argument" + index;
                ModelItem argumentItem = originalValue.Properties[argumentName].Value;

                if (argumentItem != null)
                {
                    Type[] delegateTypeList = new[]
                                              {
                                                  genericArguments[index - 1]
                                              };
                    Type inArgumentType = typeof(DelegateInArgument<>);

                    Contract.Assume(inArgumentType.IsGenericTypeDefinition);
                    Contract.Assume(inArgumentType.GetGenericArguments().Length == delegateTypeList.Length);

                    DelegateInArgument argument =
                        (DelegateInArgument)Activator.CreateInstance(inArgumentType.MakeGenericType(delegateTypeList));

                    argument.Name = (String)argumentItem.Properties["Name"].Value.GetCurrentValue();
                    newModelItem.Properties[argumentName].SetValue(argument);
                }
            }

            const String HandlerName = "Handler";
            ModelItem handerItem = originalValue.Properties[HandlerName].Value;

            if (handerItem != null)
            {
                // Copy the activity of the activity action
                newModelItem.Properties[HandlerName].SetValue(handerItem);
                originalValue.Properties[HandlerName].SetValue(null);
            }

            return newModelItem;
        }
    }
}