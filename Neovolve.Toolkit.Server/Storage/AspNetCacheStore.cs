namespace Neovolve.Toolkit.Server.Storage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="AspNetCacheStore"/>
    ///   class is used to provide a <see cref="ICacheStore"/>
    ///   implementation that leverages a <see cref="Cache"/> instance.
    /// </summary>
    /// <threadsafety instance="true"/>
    public class AspNetCacheStore : ExpirationCacheStoreBase
    {
        /// <summary>
        ///   Stores the cache instance.
        /// </summary>
        private static readonly Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// Adds the item.
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
        protected override void AddItem(String key, Object item, IExpirationPolicy[] policySet)
        {
            _cache[key] = item;
        }

        /// <summary>
        /// Clears the items from the cache.
        /// </summary>
        protected override void ClearCache()
        {
            List<String> keys = Keys.ToList();

            keys.ForEach(x => _cache.Remove(x));
        }

        /// <summary>
        /// Checks if the cache contains the item identified by the key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        protected override Boolean ContainsItem(String key)
        {
            return _cache[key] != null;
        }

        /// <summary>
        /// Reads the item from the cache using the specified key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        protected override Object ReadItem(String key)
        {
            return _cache[key];
        }

        /// <summary>
        /// Reads the cache keys.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> array instance.
        /// </returns>
        protected override Collection<String> ReadKeys()
        {
            Collection<String> keys = new Collection<String>();

            foreach (DictionaryEntry entry in _cache)
            {
                keys.Add(entry.Key as String);
            }

            return keys;
        }

        /// <summary>
        /// Removes the item from the cache.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        protected override Object RemoveItem(String key)
        {
            return _cache.Remove(key);
        }

        /// <summary>
        ///   Gets the item count.
        /// </summary>
        /// <value>
        ///   The item count.
        /// </value>
        protected override Int32 ItemCount
        {
            get
            {
                return _cache.Count;
            }
        }
    }
}