namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using System.Collections.ObjectModel;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="ExpirationCacheStoreBaseStub"/>
    ///   class is used to provide a stub of the <see cref="ExpirationCacheStoreBase"/> class.
    /// </summary>
    public class ExpirationCacheStoreBaseStub : ExpirationCacheStoreBase
    {
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
        /// The policySet.
        /// </param>
        protected override void AddItem(String key, Object item, IExpirationPolicy[] policySet)
        {
            Key = key;
            StoredItem = item;
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        protected override void ClearCache()
        {
            Key = String.Empty;
            StoredItem = null;
        }

        /// <summary>
        /// Contains item.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// The contains item.
        /// </returns>
        protected override Boolean ContainsItem(String key)
        {
            return Key == key;
        }

        /// <summary>
        /// Read item.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// The read item.
        /// </returns>
        protected override Object ReadItem(String key)
        {
            if (Key == key)
            {
                return StoredItem;
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
            if (String.IsNullOrEmpty(Key))
            {
                return new Collection<String>
                       {
                           Key
                       };
            }

            return new Collection<String>();
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// The remove item.
        /// </returns>
        protected override Object RemoveItem(String key)
        {
            if (Key == key)
            {
                Object item = StoredItem;

                Clear();

                return item;
            }

            return null;
        }

        /// <summary>
        ///   Gets or sets cache key.
        /// </summary>
        /// <value>
        ///   The cache key.
        /// </value>
        public String Key
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the stored item.
        /// </summary>
        /// <value>
        ///   The stored item.
        /// </value>
        public Object StoredItem
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets ItemCount.
        /// </summary>
        /// <value>
        ///   The item count.
        /// </value>
        protected override Int32 ItemCount
        {
            get
            {
                if (StoredItem != null)
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}