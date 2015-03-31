namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Validation;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Security.Principal;
    using System.ServiceModel.Activities;
    using System.Windows.Markup;
    using Neovolve.Toolkit.Workflow.ExecutionProperties;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="ReceiveIdentityInspector{T}"/>
    ///   class is used to inspect the identity that is executing a service operation in a <see cref="Receive"/> activity.
    /// </summary>
    /// <typeparam name="T">
    /// The type of <see cref="IIdentity"/> to return.
    /// </typeparam>
    [ContentProperty("Body")]
    public sealed class ReceiveIdentityInspector<T> : NativeActivity<T> where T : class, IIdentity
    {
        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (Body == null)
            {
                ValidationError nullBodyFailure = new ValidationError(Resources.OperationIdentityInspector_NullBodyFailure, false, "Body");

                metadata.AddValidationError(nullBodyFailure);
            }

            metadata.AddChild(Body);
        }

        /// <summary>
        /// Runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            if (Body == null)
            {
                return;
            }

            ExecutionProperties executionProperties = context.Properties;

            if (executionProperties.Find(OperationIdentity.Name) == null)
            {
                OperationIdentity inspector = new OperationIdentity();

                executionProperties.Add(OperationIdentity.Name, inspector);
            }

            context.ScheduleActivity(Body, OnBodyCompleted);
        }

        /// <summary>
        /// Called when the body activity has completed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        private void OnBodyCompleted(NativeActivityContext context, ActivityInstance instance)
        {
            OperationIdentity inspector = context.Properties.Find(OperationIdentity.Name) as OperationIdentity;

            Debug.Assert(inspector != null, "No inspector was found in the context properties");

            IIdentity operationIdentity = inspector.Identity;

            if (operationIdentity == null)
            {
                return;
            }

            T specificIdentity = operationIdentity as T;

            if (specificIdentity == null)
            {
                String message = String.Format(
                    CultureInfo.CurrentCulture, 
                    Resources.OperationIdentityInspector_UnexpectedIdentityType, 
                    typeof(T).FullName, 
                    operationIdentity.GetType().FullName);

                throw new InvalidCastException(message);
            }

            Result.Set(context, specificIdentity);
        }

        /// <summary>
        ///   Gets or sets the body.
        /// </summary>
        /// <value>
        ///   The body of the activity.
        /// </value>
        [DefaultValue((String)null)]
        [Browsable(false)]
        public Receive Body
        {
            get;
            set;
        }
    }
}