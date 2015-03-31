namespace Neovolve.Toolkit.Server.UnitTests.Unity
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Server.Unity;

    /// <summary>
    /// The <see cref="DerivedUnityServiceHostFactory"/>
    ///   class is used to test the <see cref="UnityServiceHostFactory"/> class.
    /// </summary>
    internal class DerivedUnityServiceHostFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedUnityServiceHostFactory"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public DerivedUnityServiceHostFactory(IUnityContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Creates the host.
        /// </summary>
        /// <param name="serviceType">
        /// Type of the service.
        /// </param>
        /// <param name="addresses">
        /// The addresses.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceHost"/> instance.
        /// </returns>
        public ServiceHost CreateHost(Type serviceType, Uri[] addresses)
        {
            return CreateServiceHost(serviceType, addresses);
        }
    }
}