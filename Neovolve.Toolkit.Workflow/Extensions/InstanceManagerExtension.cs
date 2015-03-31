namespace Neovolve.Toolkit.Workflow.Extensions
{
    using System;
    using System.Activities.Persistence;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Xml.Linq;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Threading;
    using Neovolve.Toolkit.Unity;

    /// <summary>
    /// The <see cref="InstanceManagerExtension"/>
    ///   class is used to manage instances resolved from a container.
    /// </summary>
    /// <threadsafety instance="true"/>
    public class InstanceManagerExtension : PersistenceParticipant, IDisposable
    {
        /// <summary>
        ///   Stores the set of instances for the extension.
        /// </summary>
        private static readonly Dictionary<Guid, Object> _instanceCache = new Dictionary<Guid, Object>();

        /// <summary>
        ///   Stores the instance synchronization lock.
        /// </summary>
        private static readonly ReaderWriterLockSlim _instanceSyncLock = new ReaderWriterLockSlim();

        /// <summary>
        ///   Stores the set of instance handler id values against the extension instance.
        /// </summary>
        private readonly List<Guid> _handlerIdCache = new List<Guid>();

        /// <summary>
        ///   Stores the handler synchronization lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _handlerSyncLock = new ReaderWriterLockSlim();

        /// <summary>
        ///   Stores the container.
        /// </summary>
        private static IUnityContainer _container;

        /// <summary>
        /// Resolves an instance using the specified handler.
        /// </summary>
        /// <typeparam name="T">
        /// The type of instance to resolve.
        /// </typeparam>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        public static T Resolve<T>(InstanceHandler<T> handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            T resolvedInstance = Container.Resolve<T>(handler.ResolutionName);

            using (new LockWriter(_instanceSyncLock))
            {
                if (_instanceCache.ContainsKey(handler.InstanceHandlerId))
                {
                    throw new InvalidOperationException("Instance handler cache is corrupted with a stale instance");
                }

                _instanceCache.Add(handler.InstanceHandlerId, resolvedInstance);

                return resolvedInstance;
            }
        }

        /// <summary>
        /// Destroys the handler.
        /// </summary>
        /// <param name="instanceHandlerId">
        /// The instance handler id.
        /// </param>
        public void DestroyHandler(Guid instanceHandlerId)
        {
            Contract.Requires<ArgumentException>(instanceHandlerId.Equals(Guid.Empty) == false);

            DestroyHandlerByHandlerInstanceId(instanceHandlerId);
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
        /// Creates the instance handler.
        /// </summary>
        /// <typeparam name="T">
        /// The type of instance being handled.
        /// </typeparam>
        /// <param name="resolutionName">
        /// Name of the resolution.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceHandler{T}"/> instance.
        /// </returns>
        internal InstanceHandler<T> CreateInstanceHandler<T>(String resolutionName)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            InstanceHandler<T> handler = new InstanceHandler<T>(resolutionName);

            Contract.Assume(_handlerSyncLock != null);

            // Store this created handler so that it can be destroyed when the workflow is persisted
            using (new LockWriter(_handlerSyncLock))
            {
                _handlerIdCache.Add(handler.InstanceHandlerId);
            }

            return handler;
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

                    DestroyLocalHandles();

                    _handlerSyncLock.Dispose();
                }
            }

            // Free native resources if there are any.
        }

        /// <summary>
        /// A host invokes this method after it is done with collecting the values in the first stage. 
        ///   The host forwards two read-only dictionaries of values it collected from all persistence participants during the first stage (CollectValues stage) 
        ///   to this method for mapping. 
        ///   The host adds values in the dictionary returned by this method to the collection of write-only values.
        /// </summary>
        /// <param name="readWriteValues">
        /// The read-write values to be persisted.
        /// </param>
        /// <param name="writeOnlyValues">
        /// The write-only values to be persisted.
        /// </param>
        /// <returns>
        /// A dictionary containing additional write-only values to be persisted.
        /// </returns>
        protected override IDictionary<XName, Object> MapValues(
            IDictionary<XName, Object> readWriteValues, IDictionary<XName, Object> writeOnlyValues)
        {
            // This method is used to detect when the activity is being persisted
            DestroyLocalHandles();

            return base.MapValues(readWriteValues, writeOnlyValues);
        }

        /// <summary>
        /// Destroys the handler by handler instance id.
        /// </summary>
        /// <param name="instanceHandlerId">
        /// The instance handler id.
        /// </param>
        private void DestroyHandlerByHandlerInstanceId(Guid instanceHandlerId)
        {
            Contract.Assume(_handlerSyncLock != null);

            using (new LockWriter(_handlerSyncLock))
            {
                _handlerIdCache.Remove(instanceHandlerId);
            }

            Contract.Assume(_instanceSyncLock != null);

            // Get this handler
            using (new LockReader(_instanceSyncLock))
            {
                if (_instanceCache.ContainsKey(instanceHandlerId) == false)
                {
                    return;
                }
            }

            Object instance = null;

            using (new LockWriter(_instanceSyncLock))
            {
                if (_instanceCache.ContainsKey(instanceHandlerId))
                {
                    instance = _instanceCache[instanceHandlerId];

                    _instanceCache.Remove(instanceHandlerId);
                }
            }

            if (instance != null)
            {
                Container.Teardown(instance);
            }
        }

        /// <summary>
        /// Destroys the local handles.
        /// </summary>
        private void DestroyLocalHandles()
        {
            List<Guid> handleIdList;

            Contract.Assume(_handlerSyncLock != null);

            using (new LockReader(_handlerSyncLock))
            {
                handleIdList = new List<Guid>(_handlerIdCache);
            }

            handleIdList.ForEach(DestroyHandlerByHandlerInstanceId);
        }

        /// <summary>
        ///   Gets or sets the container.
        /// </summary>
        /// <value>
        ///   The container.
        /// </value>
        public static IUnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = DomainContainer.Current;
                }

                return _container;
            }

            set
            {
                _container = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "InstanceManagerExtension" /> is disposed.
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