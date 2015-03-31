namespace Neovolve.Toolkit.Storage
{
    using System;

    /// <summary>
    /// The <see cref="IExpirationPolicy"/>
    ///   interface is used to define how a cache item expiration policy is evaluated in 
    ///   order to determine whether the item should be removed from the cache.
    /// </summary>
    public interface IExpirationPolicy
    {
        /// <summary>
        /// Notifies that the cache item has been added to the cache.
        /// </summary>
        /// <param name="cacheItem">
        /// The cache item.
        /// </param>
        void NotifyItemAdded(Object cacheItem);

        /// <summary>
        /// Notifies that the cache item has been read from the cache.
        /// </summary>
        /// <param name="cachedItem">
        /// The cached item.
        /// </param>
        void NotifyItemRead(Object cachedItem);

        /// <summary>
        /// Notifies the cache item has been removed from the cache.
        /// </summary>
        /// <param name="cachedItem">
        /// The cached item.
        /// </param>
        void NotifyItemRemoved(Object cachedItem);

        /// <summary>
        ///   Gets a value indicating whether the cache item has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the cache item has expired; otherwise, <c>false</c>.
        /// </value>
        Boolean HasExpired
        {
            get;
        }
    }
}