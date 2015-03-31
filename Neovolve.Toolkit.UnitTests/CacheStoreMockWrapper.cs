namespace Neovolve.Toolkit.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="CacheStoreMockWrapper"/>
    ///   class is used to provide a concreate type that wraps a mock of
    ///   <see cref="ICacheStore"/>.
    /// </summary>
    public class CacheStoreMockWrapper : ICacheStore
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
            MockInstance.Add(key, item, policySet);
        }

        /// <summary>
        /// Clears the items in the cache.
        /// </summary>
        public void Clear()
        {
            MockInstance.Clear();
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
            return MockInstance.Contains(key);
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
            return MockInstance.GetItem(key);
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
            return MockInstance.Remove(key);
        }

        /// <summary>
        ///   Gets or sets the mock instance.
        /// </summary>
        /// <value>
        ///   The mock instance.
        /// </value>
        public static ICacheStore MockInstance
        {
            get;
            set;
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
                return MockInstance.Count;
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
                return MockInstance.Keys;
            }
        }
    }
}