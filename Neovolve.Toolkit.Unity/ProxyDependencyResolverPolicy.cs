namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;
    using Neovolve.Toolkit.Communication;
    using Neovolve.Toolkit.Unity.Properties;

    /// <summary>
    /// The <see cref="ProxyDependencyResolverPolicy"/>
    ///   class is used to resolve a proxy dependency created by <see cref="ProxyManager{T}"/>.
    /// </summary>
    internal class ProxyDependencyResolverPolicy : IDependencyResolverPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyDependencyResolverPolicy"/> class.
        /// </summary>
        /// <param name="parameterType">
        /// Type of the parameter.
        /// </param>
        /// <param name="name">
        /// The name of the dependency.
        /// </param>
        public ProxyDependencyResolverPolicy(Type parameterType, String name)
        {
            // Validate that the proxy type can be have a proxy created for it
            ValidateProxyType(parameterType);

            ParameterType = parameterType;
            Name = name;
        }

        /// <summary>
        /// Resolves the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        public Object Resolve(IBuilderContext context)
        {
            NamedTypeBuildKey managerBuildKey = GetProxyManagerBuildKey(context);
            Object proxyManager;

            if (managerBuildKey != null)
            {
                // Resolve the manager using the build key
                Contract.Assume(managerBuildKey.Type.IsGenericType);
                Type genericTypeDefinition = managerBuildKey.Type.GetGenericTypeDefinition();

                Type[] parameterTypes = new Type[] { ParameterType };

                Contract.Assume(genericTypeDefinition.IsGenericTypeDefinition);
                Contract.Assume(genericTypeDefinition.GetGenericArguments().Length == parameterTypes.Length);
                Type managerType = genericTypeDefinition.MakeGenericType(parameterTypes);
                NamedTypeBuildKey managerResolveBuildKey = new NamedTypeBuildKey(managerType, managerBuildKey.Name);

                proxyManager = CreateProxyManager(context, managerResolveBuildKey);
            }
            else
            {
                // Resolve the manager using its default constructor
                proxyManager = CreateProxyManager(context, null);
            }

            // Return the proxy property of the manager
            PropertyInfo proxyProperty = proxyManager.GetType().GetProperty("Proxy");

            return proxyProperty.GetValue(proxyManager, new Object[0]);
        }

        /// <summary>
        /// Validates the type of the proxy.
        /// </summary>
        /// <param name="parameterType">
        /// Type of the parameter.
        /// </param>
        private static void ValidateProxyType(Type parameterType)
        {
            if (parameterType.IsMarshalByRef == false && parameterType.IsInterface == false)
            {
                String message = String.Format(
                    CultureInfo.InvariantCulture, 
                    Resources.ProxyDependencyResolverPolicy_TypeNotInterfaceOrMarshalByRefObject_ExceptionMessageFormat, 
                    parameterType.FullName);

                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Creates the proxy manager.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="managerResolveBuildKey">
        /// The manager resolve build key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        private Object CreateProxyManager(IBuilderContext context, NamedTypeBuildKey managerResolveBuildKey)
        {
            if (managerResolveBuildKey != null)
            {
                return context.NewBuildUp(managerResolveBuildKey);
            }

            // We will use the build key to create the proxy manager
            // We will use the default constructor of the manager to create the manager
            Type[] constructorParameters = Type.EmptyTypes;
            Type proxyManagerDefinition = typeof(ProxyManager<>);
            ConstructorInfo constructor = proxyManagerDefinition.GetConstructor(constructorParameters);
            InjectionParameterValue[] emptyInjectionParameters = new InjectionParameterValue[0];
            IBuilderPolicy constructorPolicy = new SpecifiedConstructorSelectorPolicy(constructor, emptyInjectionParameters);

            Contract.Assume(proxyManagerDefinition.IsGenericType);

            Type genericTypeDefinition = proxyManagerDefinition.GetGenericTypeDefinition();

            Type[] typeArguments = new[]
                                   {
                                       ParameterType
                                   };

            Contract.Assume(genericTypeDefinition.IsGenericTypeDefinition);
            Contract.Assume(genericTypeDefinition.GetGenericArguments().Length == typeArguments.Length);

            Type managerType = genericTypeDefinition.MakeGenericType(typeArguments);
            NamedTypeBuildKey managerBuildKey = new NamedTypeBuildKey(managerType);

            // This uses a constructor policy to ensure that Unity resolves the default constructor
            return context.NewBuildUp(managerBuildKey, x => x.Policies.Set(typeof(IConstructorSelectorPolicy), constructorPolicy, managerBuildKey));
        }

        /// <summary>
        /// Gets the proxy manager build key.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// A <see cref="NamedTypeBuildKey"/> instance.
        /// </returns>
        private NamedTypeBuildKey GetProxyManagerBuildKey(IBuilderContext context)
        {
            if (String.IsNullOrEmpty(Name) == false)
            {
                // Create the ProxyManager<> using the defined name resolution
                return new NamedTypeBuildKey(typeof(ProxyManager<>), Name);
            }

            // See if there is a build mapping policy for the manager
            NamedTypeBuildKey managerBuildKey = new NamedTypeBuildKey(typeof(ProxyManager<>));
            IBuildKeyMappingPolicy managerMappingPolicy =
                context.PersistentPolicies.Get(typeof(IBuildKeyMappingPolicy), managerBuildKey) as IBuildKeyMappingPolicy;

            if (managerMappingPolicy != null)
            {
                return managerBuildKey;
            }

            // See if there is a mapping policy for ProxyHandler<>
            NamedTypeBuildKey findHandlerBuildKey = new NamedTypeBuildKey(typeof(ProxyHandler<>));

            IBuildKeyMappingPolicy handlerMappingPolicy =
                context.PersistentPolicies.Get(typeof(IBuildKeyMappingPolicy), findHandlerBuildKey) as IBuildKeyMappingPolicy;

            if (handlerMappingPolicy != null)
            {
                return managerBuildKey;
            }

            return null;
        }

        /// <summary>
        ///   Gets the name of the dependency.
        /// </summary>
        /// <value>
        ///   The name of the dependency.
        /// </value>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the type of the parameter.
        /// </summary>
        /// <value>
        ///   The type of the parameter.
        /// </value>
        public Type ParameterType
        {
            get;
            private set;
        }
    }
}