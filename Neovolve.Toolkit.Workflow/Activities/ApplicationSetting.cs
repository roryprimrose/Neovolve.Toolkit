namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity;

    /// <summary>
    /// The <see cref="ApplicationSetting&lt;T&gt;"/>
    ///   class is a workflow activity that returns an application configuration value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of value to return.
    /// </typeparam>
    /// <remarks>
    /// The configuration value is obtained from a <see cref="IConfigurationStore"/> instance. 
    ///   The instance is resolved from <see cref="DomainContainer.TryGetCurrent">DomainContainer.TryGetCurrent</see>.
    ///   If no <see cref="IUnityContainer"/> exists with a registration for <see cref="IConfigurationStore"/>, the instance is obtained from
    ///   <see cref="ConfigurationStoreFactory.Create">ConfigurationStoreFactory.Create</see>.
    /// </remarks>
    /// <seealso cref="DomainContainer"/>
    /// <seealso cref="ConfigurationStoreFactory"/>
    public sealed class ApplicationSetting<T> : NativeActivity<T>
    {
        /// <summary>
        ///   Stores the configuration store for the activity against the current app domain.
        /// </summary>
        private static readonly IConfigurationStore _configurationStore = BuildConfigurationStore();

        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            RuntimeArgument defaultValueArgument = new RuntimeArgument("DefaultValue", typeof(T), ArgumentDirection.In);
            RuntimeArgument isRequiredArgument = new RuntimeArgument("IsRequired", typeof(Boolean), ArgumentDirection.In);
            RuntimeArgument keyArgument = new RuntimeArgument("Key", typeof(String), ArgumentDirection.In, true);

            metadata.Bind(DefaultValue, defaultValueArgument);
            metadata.Bind(IsRequired, isRequiredArgument);
            metadata.Bind(Key, keyArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        defaultValueArgument, 
                                                        isRequiredArgument, 
                                                        keyArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);
        }

        /// <summary>
        /// Runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", 
            Justification = "The workflow property is acting as the argument in this scenario.")]
        protected override void Execute(NativeActivityContext context)
        {
            String key = Key.Get(context);

            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("Key");
            }

            T defaultValue = DefaultValue.Get(context);
            Boolean isRequired = IsRequired.Get(context);

            T storedValue = _configurationStore.GetApplicationSetting(key, defaultValue, isRequired);

            Result.Set(context, storedValue);
        }

        /// <summary>
        /// Builds the configuration store.
        /// </summary>
        /// <returns>
        /// A <see cref="IConfigurationStore"/> instance.
        /// </returns>
        private static IConfigurationStore BuildConfigurationStore()
        {
            Contract.Ensures(Contract.Result<IConfigurationStore>() != null);

            IUnityContainer container;
            Boolean containerExists = DomainContainer.TryGetCurrent(out container);

            if (containerExists)
            {
                // Check to see if it contains a registration for the configuration store
                IEnumerable<ContainerRegistration> registrations = container.Registrations;

                if (registrations != null)
                {
                    if (registrations.Any(x => x.RegisteredType.Equals(typeof(IConfigurationStore))))
                    {
                        IConfigurationStore resolvedStore = container.Resolve<IConfigurationStore>();

                        Contract.Assume(resolvedStore != null);

                        return resolvedStore;
                    }
                }
            }

            return ConfigurationStoreFactory.Create();
        }

        /// <summary>
        ///   Gets or sets the default value.
        /// </summary>
        /// <value>
        ///   The default value.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<T> DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the is required.
        /// </summary>
        /// <value>
        ///   The is required.
        /// </value>
        [DefaultValue("False")]
        public InArgument<Boolean> IsRequired
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the key.
        /// </summary>
        /// <value>
        ///   The key.
        /// </value>
        public InArgument<String> Key
        {
            get;
            set;
        }
    }
}