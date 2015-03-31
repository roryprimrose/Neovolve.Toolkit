namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ExpirationCacheStoreBaseContracts"/>
    ///   class is used to define the contracts for <see cref="ExpirationCacheStoreBase"/>.
    /// </summary>
    [ContractClassFor(typeof(ExpirationCacheStoreBase))]
    internal abstract class ExpirationCacheStoreBaseContracts : ExpirationCacheStoreBase
    {
        /// <summary>
        /// Reads the cache keys.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> array instance.
        /// </returns>
        protected override Collection<String> ReadKeys()
        {
            Contract.Ensures(Contract.Result<Collection<String>>() != null);

            return null;
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
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return null;
        }
    }
}