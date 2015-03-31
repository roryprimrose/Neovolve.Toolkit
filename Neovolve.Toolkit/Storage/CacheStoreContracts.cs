namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="CacheStoreContracts"/>
    ///   class is used to define the code contracts for the <see cref="ICacheStore"/> interface.
    /// </summary>
    [ContractClassFor(typeof(ICacheStore))]
    internal abstract class CacheStoreContracts : ICacheStore
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
        public void Add(String key, Object item, params IExpirationPolicy[] policySet)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);
            Contract.Requires<ArgumentNullException>(item != null);
        }

        /// <summary>
        /// Clears the items in the cache.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Determines whether the cache contains an item for the specified cache key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// <c>true</c>if the item exists in the cache, otherwise <c>false</c>.
        /// </returns>
        public Boolean Contains(String key)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return false;
        }

        /// <summary>
        /// Gets the item from the cache.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance or <c>null</c> if the item is not in the cache.
        /// </returns>
        public Object GetItem(String key)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return null;
        }

        /// <summary>
        /// Removes an item from the cache using the specified cache key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// The item removed from the cache, or <c>null</c> if the item did not exist in the cache.
        /// </returns>
        public Object Remove(String key)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return null;
        }

        /// <summary>
        ///   Gets the number of items stored in the cache.
        /// </summary>
        /// <value>
        ///   The count.
        /// </value>
        /// <returns>
        ///   An <see cref = "Int32" /> value.
        /// </returns>
        public Int32 Count
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        ///   Gets the cache keys.
        /// </summary>
        /// <value>
        ///   The cache keys.
        /// </value>
        public IEnumerable<String> Keys
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<String>>() != null);

                return null;
            }
        }
    }
}