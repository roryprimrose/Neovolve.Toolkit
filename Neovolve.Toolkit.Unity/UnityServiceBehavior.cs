namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Unity.Properties;

    /// <summary>
    /// The <see cref="UnityServiceBehavior"/>
    ///   class is used to provide a service behavior for configuring unity injection in WCF.
    /// </summary>
    public class UnityServiceBehavior : IServiceBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceBehavior"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="container"/> value is <c>null</c>.
        /// </exception>
        public UnityServiceBehavior(IUnityContainer container)
            : this(container, null)
        {
            Contract.Requires<ArgumentNullException>(container != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceBehavior"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="unityResolveName">
        /// Name of the unity resolve.
        /// </param>
        public UnityServiceBehavior(IUnityContainer container, String unityResolveName)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            Container = container;
            ResolveName = unityResolveName;
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description of the service.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host of the service.
        /// </param>
        /// <param name="endpoints">
        /// The service endpoints.
        /// </param>
        /// <param name="bindingParameters">
        /// Custom objects to which binding elements have access.
        /// </param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors,
        ///   security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host that is currently being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="serviceDescription"/> value is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="serviceHostBase"/> value is <c>null</c>.
        /// </exception>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceDescription == null)
            {
                throw new ArgumentNullException("serviceDescription");
            }

            if (serviceDescription.ServiceType == null)
            {
                throw new ArgumentNullException("serviceDescription", Resources.UnityServiceBehavior_ApplyDispatchBehavior_NullServiceType);
            }

            if (serviceHostBase == null)
            {
                throw new ArgumentNullException("serviceHostBase");
            }

            for (Int32 dispatcherIndex = 0; dispatcherIndex < serviceHostBase.ChannelDispatchers.Count; dispatcherIndex++)
            {
                ChannelDispatcherBase dispatcher = serviceHostBase.ChannelDispatchers[dispatcherIndex];
                ChannelDispatcher channelDispatcher = (ChannelDispatcher)dispatcher;

                for (Int32 endpointIndex = 0; endpointIndex < channelDispatcher.Endpoints.Count; endpointIndex++)
                {
                    EndpointDispatcher endpointDispatcher = channelDispatcher.Endpoints[endpointIndex];

                    Contract.Assume(Container != null);

                    endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(
                        Container, serviceDescription.ServiceType, ResolveName);
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The service host that is currently being constructed.
        /// </param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        ///   Gets the container.
        /// </summary>
        /// <value>
        ///   The container.
        /// </value>
        protected IUnityContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the name of the resolve.
        /// </summary>
        /// <value>
        ///   The name of the resolve.
        /// </value>
        protected String ResolveName
        {
            get;
            set;
        }
    }
}