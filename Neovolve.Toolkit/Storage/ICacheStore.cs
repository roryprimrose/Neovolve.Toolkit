namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ICacheStore"/>
    ///   interface defines the methods used to read and write to a cache store.
    /// </summary>
    [ContractClass(typeof(CacheStoreContracts))]
    public interface ICacheStore
    {
        /// <summary>
        /// Adds an item to the cache using the specified key and policy set.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <param name="item">
        /// The cache item.
        /// </param>
        /// <param name="policySet">
        /// The policy set.
        /// </param>
        void Add(String key, Object item, params IExpirationPolicy[] policySet);

        /// <summary>
        /// Clears the items in the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the cache contains an item for the specified cache key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// <c>true</c>if the item exists in the cache, otherwise <c>false</c>.
        /// </returns>
        Boolean Contains(String key);

        /// <summary>
        /// Gets the item from the cache.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance or <c>null</c> if the item is not in the cache.
        /// </returns>
        Object GetItem(String key);

        /// <summary>
        /// Removes an item from the cache using the specified cache key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// The item removed from the cache, or <c>null</c> if the item did not exist in the cache.
        /// </returns>
        Object Remove(String key);

        /// <summary>
        ///   Gets the number of items stored in the cache.
        /// </summary>
        /// <value>
        ///   The count.
        /// </value>
        /// <returns>
        ///   An <see cref = "Int32" /> value.
        /// </returns>
        Int32 Count
        {
            get;
        }

        /// <summary>
        ///   Gets the cache keys.
        /// </summary>
        /// <value>
        ///   The cache keys.
        /// </value>
        IEnumerable<String> Keys
        {
            get;
        }
    }
}