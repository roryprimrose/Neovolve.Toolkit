namespace Neovolve.Toolkit.Server.Unity
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Unity;

    /// <summary>
    /// The <see cref="UnityHttpModuleBase"/>
    ///   class is used to provide management of a global unity container for <see cref="IHttpModule"/> instances.
    /// </summary>
    public abstract class UnityHttpModuleBase : IHttpModule
    {
        /// <summary>
        ///   Stores the lock object used to manage the container.
        /// </summary>
        private static readonly Object _syncLock = new Object();

        /// <summary>
        ///   Stores a local reference to the container.
        /// </summary>
        private static IUnityContainer _container;

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public virtual void Dispose()
        {
            DestroyContainer();
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events 
        ///   common to all application objects within an ASP.NET application.
        /// </param>
        public virtual void Init(HttpApplication context)
        {
            // The container should only be assigned once by the Init method of the module
            // There are a pool of modules against the app pool and we only want the container created once
            AssignContainer(() => DomainContainer.Current, false);
        }

        /// <summary>
        /// Assigns the container.
        /// </summary>
        /// <param name="getContainer">
        /// The get container method.
        /// </param>
        /// <param name="allowContainerReassignment">
        /// If set to <c>true</c> [allow container reassignment].
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected static void AssignContainer(Func<IUnityContainer> getContainer, Boolean allowContainerReassignment)
        {
            Contract.Requires<ArgumentNullException>(getContainer != null);
            Contract.Ensures(_container != null);

            if (allowContainerReassignment == false && _container != null)
            {
                return;
            }

            lock (_syncLock)
            {
                // Protect the container against multiple threads and instances that get past the initial check
                if (allowContainerReassignment == false && _container != null)
                {
                    return;
                }

                _container = getContainer();
            }
        }

        /// <summary>
        /// Destroys the container.
        /// </summary>
        private static void DestroyContainer()
        {
            Contract.Ensures(_container == null);

            if (_container == null)
            {
                return;
            }

            lock (_syncLock)
            {
                // Protect the container against multiple threads and instances that get past the initial check
                if (_container == null)
                {
                    return;
                }

                _container.Dispose();
                _container = null;
            }

            Contract.Assume(_container == null);
        }

        /// <summary>
        ///   Gets or sets the container used to build up and tear down ASP.Net pages.
        /// </summary>
        /// <value>
        ///   The container used to build up and tear down ASP.Net pages.
        /// </value>
        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }

            set
            {
                // This is a manual container assignment so it can overwrite the existing container
                AssignContainer(() => value, true);
            }
        }
    }
}