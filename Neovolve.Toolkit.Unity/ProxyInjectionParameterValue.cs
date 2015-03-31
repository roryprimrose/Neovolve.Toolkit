namespace Neovolve.Toolkit.Unity
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ProxyInjectionParameterValue"/>
    ///   class is used to provide the parameter value information for a proxy injection parameter.
    /// </summary>
    /// <remarks>
    /// A Unity build operation will be performed on named resolutions of <see cref="ProxyManager{T}"/> if a value has been provided for <see cref="Name"/>.
    ///   A search will be performed for unnamed type registrations on the container for <see cref="ProxyManager{T}"/> and then <see cref="ProxyHandler{T}"/>
    ///   where no name is provided. A build operation that uses the <see cref="ProxyManager{T}(ProxyHandler{T})"/>
    ///   constructor will be performed if a registration for either of these types is found. A build operation for the
    ///   <see cref="ProxyManager{T}"/> constructor will be performed if none of these registrations can be found.
    /// </remarks>
    public class ProxyInjectionParameterValue : TypedInjectionValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyInjectionParameterValue"/> class.
        /// </summary>
        /// <param name="parameterType">
        /// Type of the parameter.
        /// </param>
        public ProxyInjectionParameterValue(Type parameterType)
            : base(parameterType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyInjectionParameterValue"/> class.
        /// </summary>
        /// <param name="parameterType">
        /// Type of the parameter.
        /// </param>
        /// <param name="name">
        /// The name of the dependency.
        /// </param>
        public ProxyInjectionParameterValue(Type parameterType, String name)
            : base(parameterType)
        {
            Name = name;
        }

        /// <summary>
        /// Return a
        ///   <see cref="T:Microsoft.Practices.ObjectBuilder2.IDependencyResolverPolicy"/>
        ///   instance that will return this types value for the parameter.
        /// </summary>
        /// <param name="typeToBuild">
        /// Type that contains the member that needs this parameter. Used
        ///   to resolve open generic parameters.
        /// </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.ObjectBuilder2.IDependencyResolverPolicy"/>.
        /// </returns>
        public override IDependencyResolverPolicy GetResolverPolicy(Type typeToBuild)
        {
            return new ProxyDependencyResolverPolicy(ParameterType, Name);
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
    }
}