namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// The <see cref="ExpirationCacheStoreBase"/>
    ///   class is used to provide the base cache store implementation that handles expiration policies.
    /// </summary>
    [ContractClass(typeof(ExpirationCacheStoreBaseContracts))]
    public abstract class ExpirationCacheStoreBase : ICacheStore
    {
        /// <summary>
        ///   Stores the lock used for reading and writing to the expiration policy sets.
        /// </summary>
        private static readonly ReaderWriterLockSlim _policyLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        ///   Stores the expiration policies for items in the cache.
        /// </summary>
        private readonly Dictionary<String, IExpirationPolicy[]> _expirationPolicies = new Dictionary<String, IExpirationPolicy[]>();

        /// <summary>
        /// Adds the specified item with a set of expiration policies.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <param name="item">
        /// The item to be added to the cache.
        /// </param>
        /// <param name="policySet">
        /// The expiration policies for the item.
        /// </param>
        public virtual void Add(String key, Object item, params IExpirationPolicy[] policySet)
        {
            AddItemInternal(key, item, policySet);
        }

        /// <summary>
        /// Clears the items from the cache.
        /// </summary>
        public void Clear()
        {
            String[] keys;

            using (new LockReader(_policyLock, true))
            {
                keys = new String[_expirationPolicies.Keys.Count];

                _expirationPolicies.Keys.CopyTo(keys, 0);
            }

            for (Int32 keyIndex = 0; keyIndex < keys.Length; keyIndex++)
            {
                String key = keys[keyIndex];
                Object cacheItem = ReadItem(key);

                IExpirationPolicy[] policySet = GetPolicySet(key);

                Debug.Assert(policySet != null, "No policy set has been stored");
                Debug.Assert(policySet.Length > 0, "The policy set is empty");

                for (Int32 policyIndex = 0; policyIndex < policySet.Length; policyIndex++)
                {
                    IExpirationPolicy policy = policySet[policyIndex];

                    policy.NotifyItemRemoved(cacheItem);
                }
            }

            using (new LockWriter(_policyLock))
            {
                _expirationPolicies.Clear();
            }

            ClearCache();
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
            EvaluatePolicySetForKey(key);

            return ContainsItem(key);
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
            IExpirationPolicy[] policySet = GetPolicySet(key);
            Object cacheItem = ReadItem(key);

            if (policySet != null)
            {
                Debug.Assert(policySet.Length > 0, "The policy set is empty");

                for (Int32 index = 0; index < policySet.Length; index++)
                {
                    IExpirationPolicy policy = policySet[index];

                    policy.NotifyItemRead(cacheItem);
                }

                if (ContainsExpiredPolicy(policySet))
                {
                    RemoveItemInternal(key);

                    return null;
                }
            }

            return cacheItem;
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
            return RemoveItemInternal(key);
        }

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
        protected abstract void AddItem(String key, Object item, IExpirationPolicy[] policySet);

        /// <summary>
        /// Clears the items from the cache.
        /// </summary>
        protected abstract void ClearCache();

        /// <summary>
        /// Checks if the cache contains the item identified by the key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        protected abstract Boolean ContainsItem(String key);

        /// <summary>
        /// Reads the item from the cache using the specified key.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        protected abstract Object ReadItem(String key);

        /// <summary>
        /// Reads the cache keys.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> array instance.
        /// </returns>
        protected abstract Collection<String> ReadKeys();

        /// <summary>
        /// Removes the item from the cache.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        protected abstract Object RemoveItem(String key);

        /// <summary>
        /// Determines whether the specified policy set contains policies that have expired.
        /// </summary>
        /// <param name="policySet">
        /// The policy set.
        /// </param>
        /// <returns>
        /// <c>true</c>if the policy set contains expired policies; otherwise, <c>false</c>.
        /// </returns>
        private static Boolean ContainsExpiredPolicy(ICollection<IExpirationPolicy> policySet)
        {
            if (policySet == null)
            {
                return false;
            }

            Debug.Assert(policySet.Count > 0, "The policy set is empty");

            return policySet.Any(policy => policy.HasExpired);
        }

        /// <summary>
        /// The internal implementation for adding an item to the cache.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <param name="item">
        /// The item to add to the cache.
        /// </param>
        /// <param name="policySet">
        /// The policy set.
        /// </param>
        private void AddItemInternal(String key, Object item, IExpirationPolicy[] policySet)
        {
            AddItem(key, item, policySet);

            using (new LockWriter(_policyLock))
            {
                // Remove any previous policy set
                if (_expirationPolicies.ContainsKey(key))
                {
                    _expirationPolicies.Remove(key);
                }

                if (policySet != null && policySet.Length > 0)
                {
                    // Assign the new policy set for the item
                    // The new policy set may be null or empty and may replace a previous set for this key
                    _expirationPolicies[key] = policySet;
                }
                else
                {
                    // There are no policies to notify of the addition
                    return;
                }
            }

            for (Int32 index = 0; index < policySet.Length; index++)
            {
                IExpirationPolicy policy = policySet[index];

                policy.NotifyItemAdded(item);
            }
        }

        /// <summary>
        /// Evaluates all policy sets.
        /// </summary>
        private void EvaluateAllPolicySets()
        {
            // Get a copy of the keys so that we can check each of the keys and remove
            String[] keys = _expirationPolicies.Keys.ToArray();

            for (Int32 index = 0; index < keys.Length; index++)
            {
                String key = keys[index];

                if (String.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                EvaluatePolicySetForKey(key);
            }
        }

        /// <summary>
        /// Evaluates the policy set for key.
        /// </summary>
        /// <param name="cacheKey">
        /// The cache key.
        /// </param>
        private void EvaluatePolicySetForKey(String cacheKey)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(cacheKey) == false);

            IExpirationPolicy[] policySet = GetPolicySet(cacheKey);

            if (ContainsExpiredPolicy(policySet))
            {
                RemoveItemInternal(cacheKey);
            }
        }

        /// <summary>
        /// Gets the policy set.
        /// </summary>
        /// <param name="cacheKey">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="IExpirationPolicy"/> array instance.
        /// </returns>
        private IExpirationPolicy[] GetPolicySet(String cacheKey)
        {
            using (new LockReader(_policyLock))
            {
                if (_expirationPolicies.ContainsKey(cacheKey))
                {
                    return _expirationPolicies[cacheKey];
                }
            }

            return null;
        }

        /// <summary>
        /// Remove item internal.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        private Object RemoveItemInternal(String key)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            IExpirationPolicy[] policySet = GetPolicySet(key);

            using (new LockWriter(_policyLock))
            {
                // Remove the policies for this cache item as they are not needed any more
                // The local copy will be used to notify the local variable of the policies that the cache item is removed
                _expirationPolicies.Remove(key);
            }

            Object cacheItem = RemoveItem(key);

            if (policySet != null)
            {
                for (Int32 index = 0; index < policySet.Length; index++)
                {
                    IExpirationPolicy policy = policySet[index];

                    policy.NotifyItemRemoved(cacheItem);
                }
            }

            return cacheItem;
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
                EvaluateAllPolicySets();

                return ItemCount;
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
                return ReadKeys();
            }
        }

        /// <summary>
        ///   Gets the item count.
        /// </summary>
        /// <value>
        ///   The item count.
        /// </value>
        protected abstract Int32 ItemCount
        {
            get;
        }
    }
}