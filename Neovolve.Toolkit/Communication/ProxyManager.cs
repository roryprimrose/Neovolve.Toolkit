namespace Neovolve.Toolkit.Communication
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.ServiceModel;

    /// <summary>
    /// The <see cref="ProxyManager{T}"/>
    ///   class is used to manage invocations of proxy objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of class to proxy calls to.
    /// </typeparam>
    public class ProxyManager<T> : IDisposable where T : class
    {
        /// <summary>
        ///   Stores the proxy instance.
        /// </summary>
        private T _proxy;

        /// <summary>
        ///   Stores the proxy handler.
        /// </summary>
        private ProxyHandler<T> _proxyHandler;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProxyManager{T}" /> class.
        /// </summary>
        /// <remarks>
        ///   The <see cref = "ProxyManager{T}" /> will check if the <typeparamref name = "T" /> type is decorated with the
        ///   <see cref = "ServiceContractAttribute" />. A <see cref = "ChannelProxyHandler{T}" /> is used if the type is a service contract,
        ///   otherwise a <see cref = "DefaultProxyHandler{T}" /> is used.
        /// </remarks>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", 
            Justification = "The instance cannot be disposed as it is stored in a field.")]
        public ProxyManager()
        {
            // Check if T is a service contract
            Object serviceContract = typeof(T).GetCustomAttributes(true).Where(x => x is ServiceContractAttribute).FirstOrDefault();

            if (serviceContract != null)
            {
                // This is a service contract, default to using the WCF proxy handler
                _proxyHandler = new ChannelProxyHandler<T>();
            }
            else
            {
                // No proxy handler is known for this type
                _proxyHandler = new DefaultProxyHandler<T>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyManager{T}"/> class.
        /// </summary>
        /// <param name="proxyHandler">
        /// The proxy handler.
        /// </param>
        public ProxyManager(ProxyHandler<T> proxyHandler)
        {
            _proxyHandler = proxyHandler;
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

                    IDisposable disposableHandler = ProxyHandler as IDisposable;

                    if (disposableHandler != null)
                    {
                        disposableHandler.Dispose();
                    }

                    _proxyHandler = null;
                    Proxy = null;
                }
            }

            // Free native resources if there are any.
        }

        /// <summary>
        ///   Gets the proxy.
        /// </summary>
        /// <value>
        ///   The proxy.
        /// </value>
        public T Proxy
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_proxy == null)
                {
                    // Create a channel from the handler
                    _proxy = (T)ProxyHandler.GetTransparentProxy();

                    Debug.Assert(_proxy != null, "The proxy handler failed to create the proxy.");
                }

                return _proxy;
            }

            private set
            {
                _proxy = value;
            }
        }

        /// <summary>
        ///   Gets the proxy handler.
        /// </summary>
        /// <value>
        ///   The proxy handler.
        /// </value>
        public ProxyHandler<T> ProxyHandler
        {
            get
            {
                return _proxyHandler;
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
    }
}