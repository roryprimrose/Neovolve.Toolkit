namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Globalization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="CacheStoreFactory"/>
    ///   class is used to create <see cref="ICacheStore"/> instances.
    /// </summary>
    /// <remarks>
    /// The application configuration is used to determine the type of 
    ///   <see cref="ICacheStore"/> to create. The cache key used is <c>CacheStoreType</c> which is defined
    ///   in <see cref="CacheStoreTypeCacheKey"/>.
    ///   If the application configuration does not contain a value, an <see cref="DictionaryCacheStore"/> instance will be returned.
    /// </remarks>
    public static class CacheStoreFactory
    {
        /// <summary>
        ///   Defines the cache key used to obtain the cache store type
        ///   from application configuration.
        /// </summary>
        public const String CacheStoreTypeCacheKey = "CacheStoreType";

        /// <summary>
        ///   Stores the object used for locking the class when resolving the store type.
        /// </summary>
        private static readonly Object _syncLock = new Object();

        /// <summary>
        ///   Stores the cache store type.
        /// </summary>
        private static Type _storeType;

        /// <summary>
        /// Creates a <see cref="ICacheStore"/> instance.
        /// </summary>
        /// <returns>
        /// A <see cref="ICacheStore"/> instance.
        /// </returns>
        /// <remarks>
        /// The application configuration is used to determine the type of 
        ///   <see cref="ICacheStore"/> to create. The cache key used is <c>CacheStoreType</c> which is defined
        ///   in <see cref="CacheStoreTypeCacheKey"/>.
        ///   If the application cache does not contain a value, an <see cref="DictionaryCacheStore"/> instance will be returned.
        /// </remarks>
        public static ICacheStore Create()
        {
            return (ICacheStore)Activator.CreateInstance(StoreType);
        }

        /// <summary>
        /// Determines the type of the cache store.
        /// </summary>
        private static void DetermineStoreType()
        {
            lock (_syncLock)
            {
                if (_storeType == null)
                {
                    _storeType = ConfigurationTypeLoader.DetermineStoreType(
                        ConfigurationStoreFactory.Create(), CacheStoreTypeCacheKey, typeof(ICacheStore), typeof(DictionaryCacheStore));
                }
            }
        }

        /// <summary>
        ///   Gets or sets the type of the cache store.
        /// </summary>
        /// <value>
        ///   The type of the cache store.
        /// </value>
        /// <remarks>
        ///   The application configuration is used to determine the type of 
        ///   <see cref = "ICacheStore" /> to create. The cache key used is <c>StoreType</c> which is defined
        ///   in <see cref = "CacheStoreTypeCacheKey" />.
        ///   If the application cache does not contain a value, an <see cref = "DictionaryCacheStore" /> instance will be returned.
        /// </remarks>
        public static Type StoreType
        {
            get
            {
                if (_storeType == null)
                {
                    DetermineStoreType();
                }

                return _storeType;
            }

            set
            {
                if (value != null)
                {
                    if (typeof(ICacheStore).IsAssignableFrom(value) == false)
                    {
                        throw new InvalidCastException(
                            String.Format(CultureInfo.InvariantCulture, Resources.InvalidCastExceptionMessage, value.GetType(), typeof(ICacheStore)));
                    }
                }

                _storeType = value;
            }
        }
    }
}