namespace Neovolve.Toolkit.Workflow.ExecutionProperties
{
    using System;
    using System.Activities;
    using System.Runtime.Serialization;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Activities;

    /// <summary>
    /// The <see cref="OperationIdentity"/>
    ///   class is used to get the identity executing the service operation.
    /// </summary>
    [DataContract]
    public class OperationIdentity : IReceiveMessageCallback
    {
        /// <summary>
        ///   Defines the name of the execution property.
        /// </summary>
        private static readonly String _executionPropertyName = typeof(OperationIdentity).FullName;

        /// <summary>
        /// Executed when a service message is received.
        /// </summary>
        /// <param name="operationContext">
        /// The operation context under which the message received.
        /// </param>
        /// <param name="activityExecutionProperties">
        /// The set of execution properties available within the workflow.
        /// </param>
        public void OnReceiveMessage(OperationContext operationContext, ExecutionProperties activityExecutionProperties)
        {
            if (operationContext == null)
            {
                return;
            }

            if (operationContext.ServiceSecurityContext == null)
            {
                return;
            }

            Identity = operationContext.ServiceSecurityContext.PrimaryIdentity;
        }

        /// <summary>
        ///   Gets the name of the property.
        /// </summary>
        /// <value>
        ///   The name of the property.
        /// </value>
        public static String Name
        {
            get
            {
                return _executionPropertyName;
            }
        }

        /// <summary>
        ///   Gets or sets the principal.
        /// </summary>
        /// <value>
        ///   The principal.
        /// </value>
        [DataMember]
        public IIdentity Identity
        {
            get;
            set;
        }
    }
}