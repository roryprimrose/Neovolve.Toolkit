namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Validation;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Neovolve.Toolkit.Workflow.ExecutionProperties;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="BusinessFailureEvaluator&lt;T&gt;"/>
    ///   class is used to evaluate business failure conditions.
    /// </summary>
    /// <typeparam name="T">
    /// The type of failure code.
    /// </typeparam>
    /// <remarks>
    /// This activity will throw a <see cref="BusinessFailureException{T}"/> directly when it does not exist within a 
    ///   <see cref="BusinessFailureScope{T}"/> of the same generic type. The activity will add the failure to the resolved parent
    ///   scope activity where it is contained within a <see cref="BusinessFailureScope{T}"/> of the same generic type.
    /// </remarks>
    public sealed class BusinessFailureEvaluator<T> : NativeActivity where T : struct
    {
        /// <summary>
        /// Creates and validates a description of the activity’s arguments, variables, child activities, and activity delegates.
        /// </summary>
        /// <param name="metadata">
        /// The activity’s metadata that encapsulates the activity’s arguments, variables, child activities, and activity delegates.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            CacheMetadataValidation(metadata);
            CacheMetadataBindProperties(metadata);
        }

        /// <summary>
        /// Performs the execution of the activity.
        /// </summary>
        /// <param name="context">
        /// The execution context under which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", 
            Justification = "The workflow property is acting as the argument in this scenario.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            Nullable<Boolean> condition = Condition.Get(context);

            if (condition != null && condition == false)
            {
                // This is not a failure
                return;
            }

            BusinessFailure<T> failure = Failure.Get(context);

            if (failure == null)
            {
                T code = Code.Get(context);
                String description = Description.Get(context);

                if (String.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentNullException("Description");
                }

                failure = new BusinessFailure<T>(code, description);
            }

            BusinessFailureTracker<T> tracker = context.Properties.Find(BusinessFailureTracker<T>.Name) as BusinessFailureTracker<T>;

            if (tracker == null)
            {
                throw new BusinessFailureException<T>(failure);
            }

            tracker.Failures.Add(failure);
        }

        /// <summary>
        /// Caches the metadata bind properties.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        private void CacheMetadataBindProperties(NativeActivityMetadata metadata)
        {
            RuntimeArgument codeArgument = new RuntimeArgument("Code", typeof(T), ArgumentDirection.In);
            RuntimeArgument conditionArgument = new RuntimeArgument("Condition", typeof(Nullable<Boolean>), ArgumentDirection.In);
            RuntimeArgument descriptionArgument = new RuntimeArgument("Description", typeof(String), ArgumentDirection.In);
            RuntimeArgument failureArgument = new RuntimeArgument("Failure", typeof(BusinessFailure<T>), ArgumentDirection.In);

            metadata.Bind(Code, codeArgument);
            metadata.Bind(Condition, conditionArgument);
            metadata.Bind(Description, descriptionArgument);
            metadata.Bind(Failure, failureArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        codeArgument, 
                                                        conditionArgument, 
                                                        descriptionArgument, 
                                                        failureArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);
        }

        /// <summary>
        /// Caches the metadata validation.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        private void CacheMetadataValidation(NativeActivityMetadata metadata)
        {
            Boolean conditionBound = Condition != null && Condition.Expression != null;

            if (conditionBound == false)
            {
                ValidationError validationError = new ValidationError(Resources.BusinessFailureEvaluator_NoConditionBoundWarning, true, "Condition");

                metadata.AddValidationError(validationError);
            }

            Boolean failureBound = Failure != null && Failure.Expression != null;
            Boolean codeBound = Code != null && Code.Expression != null;
            Boolean descriptionBound = Description != null && Description.Expression != null;
            const String CodePropertyName = "Code";

            if (failureBound == false && codeBound == false && descriptionBound == false)
            {
                ValidationError validationError = new ValidationError(
                    Resources.BusinessFailureEvaluator_FailureInformationNotBound, false, CodePropertyName);

                metadata.AddValidationError(validationError);
            }
            else if (failureBound && (codeBound || descriptionBound))
            {
                ValidationError validationError = new ValidationError(
                    Resources.BusinessFailureEvaluator_ConflictingFailureInformationBound, false, CodePropertyName);

                metadata.AddValidationError(validationError);
            }
            else if (codeBound && descriptionBound == false)
            {
                ValidationError validationError = new ValidationError(Resources.BusinessFailureEvaluator_DescriptionNotBound, false, "Description");

                metadata.AddValidationError(validationError);
            }
            else if (codeBound == false && descriptionBound)
            {
                ValidationError validationError = new ValidationError(Resources.BusinessFailureEvaluator_CodeNotBound, false, CodePropertyName);

                metadata.AddValidationError(validationError);
            }
        }

        /// <summary>
        ///   Gets or sets the code.
        /// </summary>
        /// <value>
        ///   The code for the failure.
        /// </value>
        [Category("Inputs")]
        [Description("The code of the failure")]
        [DefaultValue((String)null)]
        public InArgument<T> Code
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the condition.
        /// </summary>
        /// <value>
        ///   The condition.
        /// </value>
        [Category("Inputs")]
        [Description("The condition used to determine if there is a failure")]
        [DefaultValue((String)null)]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Workflow forces a generic type to be wrapped in a InArgument.")]
        public InArgument<Nullable<Boolean>> Condition
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the description.
        /// </summary>
        /// <value>
        ///   The description.
        /// </value>
        [Category("Inputs")]
        [Description("The description of the failure")]
        [DefaultValue((String)null)]
        public InArgument<String> Description
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the failure.
        /// </summary>
        /// <value>
        ///   The failure.
        /// </value>
        [Category("Inputs")]
        [Description("The failure")]
        [DefaultValue((String)null)]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Workflow forces a generic type to be wrapped in a InArgument.")]
        public InArgument<BusinessFailure<T>> Failure
        {
            get;
            set;
        }
    }
}