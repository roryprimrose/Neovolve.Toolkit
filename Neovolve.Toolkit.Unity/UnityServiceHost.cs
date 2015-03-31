namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="UnityServiceHost"/>
    ///   class provides a service host that creates WCF service instances using Unity.
    /// </summary>
    public class UnityServiceHost : ServiceHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHost"/> class.
        /// </summary>
        /// <param name="serviceType">
        /// Type of the service.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="baseAddresses">
        /// The base addresses.
        /// </param>
        public UnityServiceHost(Type serviceType, IUnityContainer container, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Contract.Requires<ArgumentNullException>(serviceType != null);
            Contract.Requires<ArgumentNullException>(container != null);

            Container = container;
        }

        /// <summary>
        /// Invoked during the transition of a communication object into the opening state.
        /// </summary>
        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                Contract.Assume(Container != null);
                Description.Behaviors.Add(new UnityServiceBehavior(Container));
            }

            base.OnOpening();
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
    }
}