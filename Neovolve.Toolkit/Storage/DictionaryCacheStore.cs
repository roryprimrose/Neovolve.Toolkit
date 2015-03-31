namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// The <see cref="DictionaryCacheStore"/>
    ///   class is used to provide a <see cref="ICacheStore"/>
    ///   implementation that leverages a <see cref="Dictionary{TKey,TValue}"/> instance.
    /// </summary>
    /// <threadsafety instance="true"/>
    public class DictionaryCacheStore : ExpirationCacheStoreBase
    {
        /// <summary>
        ///   Stores the cache instance.
        /// </summary>
        private static readonly Dictionary<String, Object> _cache = new Dictionary<String, Object>();

        /// <summary>
        ///   Stores the lock used for thread safety.
        /// </summary>
        private static readonly ReaderWriterLock _lock = new ReaderWriterLock();

        /// <summary>
        /// Adds the item to the cache.
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
            using (new LockWriter(_lock))
            {
                _cache[key] = item;
            }
        }

        /// <summary>
        /// Clears the items from the cache.
        /// </summary>
        protected override void ClearCache()
        {
            using (new LockWriter(_lock))
            {
                _cache.Clear();
            }
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
            using (new LockReader(_lock))
            {
                return _cache.ContainsKey(key);
            }
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
            using (new LockReader(_lock))
            {
                if (_cache.ContainsKey(key))
                {
                    return _cache[key];
                }
            }

            return null;
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

            foreach (String key in _cache.Keys)
            {
                keys.Add(key);
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
            Object cacheValue = GetItem(key);

            using (new LockWriter(_lock))
            {
                _cache.Remove(key);
            }

            return cacheValue;
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
                using (new LockReader(_lock))
                {
                    return _cache.Count;
                }
            }
        }
    }
}