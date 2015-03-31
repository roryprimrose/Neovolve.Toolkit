namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="UnityInstanceProvider"/>
    ///   class is used by WCF to create a new instance of a service contract.
    /// </summary>
    internal class UnityInstanceProvider : IInstanceProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceProvider"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="serviceType">
        /// Type of the service.
        /// </param>
        /// <param name="resolveName">
        /// Name of the resolve.
        /// </param>
        public UnityInstanceProvider(IUnityContainer container, Type serviceType, String resolveName)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(serviceType != null);

            Container = container;
            ServiceType = serviceType;
            ResolveName = resolveName;
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <returns>
        /// A user-defined service object.
        /// </returns>
        /// <param name="instanceContext">
        /// The current <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </param>
        public Object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <returns>
        /// The service object.
        /// </returns>
        /// <param name="instanceContext">
        /// The current <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </param>
        /// <param name="message">
        /// The message that triggered the creation of a service object.
        /// </param>
        public Object GetInstance(InstanceContext instanceContext, Message message)
        {
            return Container.Resolve(ServiceType, ResolveName);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">
        /// The service's instance context.
        /// </param>
        /// <param name="instance">
        /// The service object to be recycled.
        /// </param>
        public void ReleaseInstance(InstanceContext instanceContext, Object instance)
        {
            Container.Teardown(instance);
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

        /// <summary>
        ///   Gets or sets the type of the service.
        /// </summary>
        /// <value>
        ///   The type of the service.
        /// </value>
        protected Type ServiceType
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the container.
        /// </summary>
        /// <value>
        ///   The container.
        /// </value>
        private IUnityContainer Container
        {
            get;
            set;
        }
    }
}