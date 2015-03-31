namespace Neovolve.Toolkit.Storage
{
    using System;

    /// <summary>
    /// The <see cref="AbsoluteExpirationPolicy"/>
    ///   class is used to define an absolute time when a cache item is to expire.
    /// </summary>
    public class AbsoluteExpirationPolicy : IExpirationPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbsoluteExpirationPolicy"/> class.
        /// </summary>
        /// <param name="absoluteExpiration">
        /// The absolute expiration.
        /// </param>
        public AbsoluteExpirationPolicy(TimeSpan absoluteExpiration)
        {
            Expiration = DateTime.Now.Add(absoluteExpiration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbsoluteExpirationPolicy"/> class.
        /// </summary>
        /// <param name="absoluteExpiration">
        /// The absolute expiration.
        /// </param>
        public AbsoluteExpirationPolicy(DateTime absoluteExpiration)
        {
            Expiration = absoluteExpiration;
        }

        /// <summary>
        /// Notifies that the cache item has been added to the cache.
        /// </summary>
        /// <param name="cacheItem">
        /// The cache item.
        /// </param>
        public void NotifyItemAdded(Object cacheItem)
        {
        }

        /// <summary>
        /// Notifies that the cache item has been read from the cache.
        /// </summary>
        /// <param name="cachedItem">
        /// The cached item.
        /// </param>
        public void NotifyItemRead(Object cachedItem)
        {
        }

        /// <summary>
        /// Notifies the cache item has been removed from the cache.
        /// </summary>
        /// <param name="cachedItem">
        /// The cached item.
        /// </param>
        public void NotifyItemRemoved(Object cachedItem)
        {
        }

        /// <summary>
        ///   Gets the expiration.
        /// </summary>
        /// <value>
        ///   The expiration.
        /// </value>
        public DateTime Expiration
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets a value indicating whether the cache item has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c>if the cache item has expired; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasExpired
        {
            get
            {
                return Expiration < DateTime.Now;
            }
        }
    }
}