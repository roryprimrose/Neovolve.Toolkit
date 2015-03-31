namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Configuration;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="DomainContainer"/>
    ///   class is used to provide an <see cref="IUnityContainer"/> instance for the current <see cref="AppDomain"/>.
    /// </summary>
    public static class DomainContainer
    {
        /// <summary>
        ///   Defines the object used to protect the container.
        /// </summary>
        private static readonly Object _syncLock = new Object();

        /// <summary>
        ///   Stores the container managed for the current app domain.
        /// </summary>
        private static IUnityContainer _domainContainer;

        /// <summary>
        ///   Stores whether a resolution attempt has been made.
        /// </summary>
        private static Boolean _resolutionAttemptMade;

        /// <summary>
        /// Destroys the container held for the current <see cref="AppDomain"/>.
        /// </summary>
        public static void Destroy()
        {
            if (_domainContainer == null && _resolutionAttemptMade == false)
            {
                return;
            }

            lock (_syncLock)
            {
                _resolutionAttemptMade = false;

                if (_domainContainer == null)
                {
                    return;
                }

                _domainContainer.Dispose();
                _domainContainer = null;
            }
        }

        /// <summary>
        /// Tries to get the current container.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        public static Boolean TryGetCurrent(out IUnityContainer container)
        {
            if (_domainContainer == null)
            {
                if (_resolutionAttemptMade == false)
                {
                    CreateOptionalContainer();
                }
            }

            container = _domainContainer;

            return _domainContainer != null;
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        private static void CreateContainer()
        {
            lock (_syncLock)
            {
                // Protect the container from multiple threads that get passed the first check
                if (_domainContainer != null)
                {
                    return;
                }

                _domainContainer = UnityContainerResolver.Resolve();
            }
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        private static void CreateOptionalContainer()
        {
            lock (_syncLock)
            {
                // Protect the container from multiple threads that get passed the first check
                if (_domainContainer != null)
                {
                    return;
                }

                UnityContainerResolver.TryResolve(out _domainContainer);

                _resolutionAttemptMade = true;
            }
        }

        /// <summary>
        ///   Gets the container for the current <see cref = "AppDomain" />.
        /// </summary>
        /// <value>
        ///   The current container.
        /// </value>
        /// <exception cref = "ConfigurationErrorsException">No container has been configured.</exception>
        public static IUnityContainer Current
        {
            get
            {
                if (_domainContainer == null)
                {
                    CreateContainer();
                }

                return _domainContainer;
            }
        }
    }
}