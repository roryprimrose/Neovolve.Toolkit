namespace Neovolve.Toolkit.Communication
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Neovolve.Toolkit.Properties;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="ChannelProxyHandler{T}"/>
    ///   class is used to provide a proxy implementation for a WCF service channel.
    /// </summary>
    /// <typeparam name="T">
    /// The type of WCF service to invoke.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// The <see cref="ChannelProxyHandler{T}"/> ensures that the the WCF call will always operate on a valid WCF channel.
    ///     The proxy handler will recreate the WCF channel if a prior call has faulted the channel.
    ///   </para>
    /// <note>
    /// The proxy handler will throw all WCF exceptions to the caller even though it manages faulted channels internally.
    ///   </note>
    /// <para>
    /// The default constructor will search the application configuration for an endpoint for the specified service contract. 
    ///     A <see cref="ConfigurationErrorsException"/> will be thrown if no endpoint configuration is found or if multiple 
    ///     endpoint configurations are found. An unnamed endpoint configuration is taken as the priority over a named endpoint 
    ///     configuration where both a named and unnamed endpoint configuraiton is found.
    ///   </para>
    /// <para>
    /// The WCF channel is closed when the proxy is disposed.
    ///   </para>
    /// <para>
    /// Any initialization required for the WCF channel can be achieved by providing a <see cref="Action{T}"/> of type
    ///     <see cref="ChannelFactory{TChannel}"/> to the <see cref="Initialize{TInitialize}"/> method. This action will be
    ///     invoked before each time the WCF channel is created. Any existing channel will be destroyed when an action is defined
    ///     to ensure that the next channel invocation uses the provided initialization functionality.
    ///   </para>
    /// </remarks>
    public class ChannelProxyHandler<T> : ProxyHandler<T>, IDisposable where T : class
    {
        /// <summary>
        ///   Stores the function used to create the factory.
        /// </summary>
        private readonly Func<ChannelFactory<T>> _createFactory;

        /// <summary>
        ///   Stores the type mapping cache.
        /// </summary>
        private static ICacheStore _typeMapping;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ChannelProxyHandler{T}" /> class.
        /// </summary>
        /// <remarks>
        ///   The default constructor will search the application configuration for an endpoint for the specified service contract. 
        ///   A <see cref = "ConfigurationErrorsException" /> will be thrown if no endpoint configuration is found or if multiple 
        ///   endpoint configurations are found.
        /// </remarks>
        public ChannelProxyHandler()
            : this(ConfigurationStoreFactory.Create())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelProxyHandler{T}"/> class.
        /// </summary>
        /// <param name="configurationName">
        /// Name of the configuration.
        /// </param>
        public ChannelProxyHandler(String configurationName)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(configurationName) == false);

            _createFactory = new Func<ChannelFactory<T>>(() => new ChannelFactory<T>(configurationName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelProxyHandler{T}"/> class.
        /// </summary>
        /// <param name="store">
        /// The store.
        /// </param>
        public ChannelProxyHandler(IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(store != null);

            String configurationName = ResolveConfigurationName(store);

            _createFactory = new Func<ChannelFactory<T>>(() => new ChannelFactory<T>(configurationName));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes the proxy using the specified action.
        /// </summary>
        /// <typeparam name="TInitialize">
        /// The type of object to be initialized for the proxy.
        /// </typeparam>
        /// <param name="action">
        /// The action to invoke.
        /// </param>
        public override void Initialize<TInitialize>(Action<TInitialize> action)
        {
            if (typeof(ChannelFactory).IsAssignableFrom(typeof(TInitialize)) == false)
            {
                return;
            }

            // This initialization function is supported by this handler
            // Allow the initialization to continue
            base.Initialize(action);

            // On the assumption that the initialize action changes the credentials, the factory must be recreated
            DestroyChannel(false);
            DestroyFactory();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (Disposed == false)
                {
                    Disposed = true;

                    DestroyChannel(false);
                    DestroyFactory();
                }
            }

            // Free native resources if there are any.
        }

        /// <summary>
        /// Executes the method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        protected override Object ExecuteMethod(MethodBase method, Object[] parameters)
        {
            if (Channel == null)
            {
                CreateChannel();
            }
            else if (ChannelIsFaulted())
            {
                CreateChannel();
            }

            try
            {
                return InvokeServiceMethod(method, parameters);
            }
            catch (TimeoutException)
            {
                DestroyChannel(true);

                throw;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (CommunicationException)
            {
                DestroyChannel(true);

                throw;
            }
        }

        /// <summary>
        /// Generates the config mapping cache key.
        /// </summary>
        /// <param name="proxyType">
        /// Type of the proxy.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String GenerateConfigMappingCacheKey(Type proxyType)
        {
            Contract.Requires<ArgumentNullException>(proxyType != null);
            Contract.Ensures(String.IsNullOrWhiteSpace(Contract.Result<String>()) == false);

            String configMappingCacheKey = proxyType.AssemblyQualifiedName;

            Contract.Assume(String.IsNullOrWhiteSpace(configMappingCacheKey) == false);

            return configMappingCacheKey;
        }

        /// <summary>
        /// Resolves the name of the configuration.
        /// </summary>
        /// <param name="store">
        /// The store.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String ResolveConfigurationName(IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(store != null);

            String configMappingCacheKey = GenerateConfigMappingCacheKey(typeof(ChannelProxyHandler<T>));

            return TypeMapping.GetItem(configMappingCacheKey, () => SearchConfigurationName(store));
        }

        /// <summary>
        /// Searches the name of the configuration.
        /// </summary>
        /// <param name="store">
        /// The store.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        /// <remarks>
        /// A value of <see cref="String.Empty">String.Empty</see> is a valid value for the service configuration name.
        /// </remarks>
        private static String SearchConfigurationName(IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Ensures(Contract.Result<String>() != null);

            ClientSection clientConfig = store.GetSection<ClientSection>("system.serviceModel/client");

            if (clientConfig == null)
            {
                return String.Empty;
            }

            String fullName = typeof(T).FullName;
            String partialName = fullName + ", " + Path.GetFileNameWithoutExtension(typeof(T).Assembly.Location);
            Boolean emptyConfigFound = false;
            String configurationName = String.Empty;

            foreach (ChannelEndpointElement endpoint in clientConfig.Endpoints)
            {
                if (endpoint.Contract != fullName && endpoint.Contract != partialName)
                {
                    continue;
                }

                if (String.IsNullOrWhiteSpace(endpoint.Name))
                {
                    if (emptyConfigFound)
                    {
                        // There are duplicate configurations with the empty configuration name
                        String message = String.Format(
                            CultureInfo.InvariantCulture, 
                            Resources.ChannelProxyHandler_MultipleUnnamedEndpointConfigurationsFoundForContract, 
                            fullName);

                        throw new ConfigurationErrorsException(message);
                    }

                    emptyConfigFound = true;
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(configurationName) == false)
                    {
                        String message = String.Format(
                            CultureInfo.InvariantCulture, Resources.ChannelProxyHandler_MultipleNamedEndpointConfigurationsFoundForContract, fullName);

                        throw new ConfigurationErrorsException(message);
                    }

                    configurationName = endpoint.Name;
                }
            }

            if (emptyConfigFound)
            {
                // At this point there is only one empty configuration so this will take priority over named configurations
                return String.Empty;
            }

            if (String.IsNullOrWhiteSpace(configurationName))
            {
                String message = String.Format(
                    CultureInfo.InvariantCulture, Resources.ChannelProxyHandler_NoEndpointConfigurationForServiceType, typeof(T).FullName);

                throw new ConfigurationErrorsException(message);
            }

            // At this point there is no empty configuration name found and only one named configuration. This will be the value used to load the service config.
            return configurationName;
        }

        /// <summary>
        /// Channels the is fauled.
        /// </summary>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private Boolean ChannelIsFaulted()
        {
            ICommunicationObject channel = (ICommunicationObject)Channel;

            return channel.State == CommunicationState.Faulted;
        }

        /// <summary>
        /// The create channel.
        /// </summary>
        private void CreateChannel()
        {
            DestroyChannel(false);
            CreateFactory();

            if (Factory.State != CommunicationState.Opened)
            {
                if (InitializeAction != null)
                {
                    Action<ChannelFactory<T>> factoryInitalize = InitializeAction as Action<ChannelFactory<T>>;

                    if (factoryInitalize != null)
                    {
                        factoryInitalize.Invoke(Factory);
                    }
                }

                Factory.Open();
            }

            Channel = Factory.CreateChannel();
        }

        /// <summary>
        /// Creates the factory.
        /// </summary>
        private void CreateFactory()
        {
            if (Factory != null)
            {
                return;
            }

            // Create the factory
            Factory = _createFactory.Invoke();
        }

        /// <summary>
        /// Destroys the channel.
        /// </summary>
        /// <param name="forceAbort">
        /// If set to <c>true</c> the channel will be forced to abort.
        /// </param>
        private void DestroyChannel(Boolean forceAbort)
        {
            if (Channel == null)
            {
                return;
            }

            ICommunicationObject channel = (ICommunicationObject)Channel;

            if (forceAbort)
            {
                channel.Abort();
            }
            else if (channel.State == CommunicationState.Faulted)
            {
                channel.Abort();
            }
            else
            {
                try
                {
                    // Close the channel
                    channel.Close();
                }
                catch (CommunicationException)
                {
                    // Abort the channel
                    channel.Abort();
                }
            }

            Channel = null;
        }

        /// <summary>
        /// Destroys the factory.
        /// </summary>
        private void DestroyFactory()
        {
            if (Factory == null)
            {
                return;
            }

            Factory.Close();
            Factory = null;
        }

        /// <summary>
        /// Invokes the service method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        private Object InvokeServiceMethod(MethodBase method, Object[] parameters)
        {
            try
            {
                // Invoke the service method
                return method.Invoke(Channel, parameters);
            }
            catch (TargetInvocationException ex)
            {
                Debug.Assert(ex.InnerException != null, "No inner exception was found");

                // Throw the inner exception
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///   Gets the type mapping.
        /// </summary>
        /// <value>
        ///   The type mapping.
        /// </value>
        private static ICacheStore TypeMapping
        {
            get
            {
                if (_typeMapping == null)
                {
                    _typeMapping = CacheStoreFactory.Create();
                }

                return _typeMapping;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "ProxyManager{T}" /> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        protected Boolean Disposed
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the channel.
        /// </summary>
        /// <value>
        ///   The channel.
        /// </value>
        private T Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the factory.
        /// </summary>
        /// <value>
        ///   The factory.
        /// </value>
        private ChannelFactory<T> Factory
        {
            get;
            set;
        }
    }
}