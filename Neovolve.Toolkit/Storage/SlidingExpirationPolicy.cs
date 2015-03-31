namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="SlidingExpirationPolicy"/>
    ///   class is used to define a sliding time when a cache item is to expire based on when it was last read.
    /// </summary>
    public class SlidingExpirationPolicy : IExpirationPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SlidingExpirationPolicy"/> class.
        /// </summary>
        /// <param name="relativeExpiration">
        /// The relative expiration.
        /// </param>
        public SlidingExpirationPolicy(TimeSpan relativeExpiration)
        {
            ItemLastRead = DateTime.Now;
            Expiration = relativeExpiration;
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
            ItemLastRead = DateTime.Now;

            Debug.WriteLine("New expiration point is " + Expires.Ticks);
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
        public TimeSpan Expiration
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
                DateTime expirationPoint = Expires;
                Debug.WriteLine("Expiration point is " + expirationPoint.Ticks);

                DateTime currentTime = CurrentTime;

                Debug.WriteLine("Current time is " + currentTime.Ticks);

                return expirationPoint.Ticks <= currentTime.Ticks;
            }
        }

        /// <summary>
        ///   Gets the current time.
        /// </summary>
        /// <value>
        ///   The current time.
        /// </value>
        protected virtual DateTime CurrentTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        ///   Gets the time the policy expires.
        /// </summary>
        /// <value>
        ///   The time the policy expires.
        /// </value>
        protected virtual DateTime Expires
        {
            get
            {
                DateTime lastRead = LastRead;

                return lastRead.AddTicks(Expiration.Ticks);
            }
        }

        /// <summary>
        ///   Gets the last read.
        /// </summary>
        /// <value>
        ///   The last read.
        /// </value>
        protected virtual DateTime LastRead
        {
            get
            {
                return ItemLastRead;
            }
        }

        /// <summary>
        ///   Gets or sets when the item was last read.
        /// </summary>
        /// <value>
        ///   When the item was last read.
        /// </value>
        private DateTime ItemLastRead
        {
            get;
            set;
        }
    }
}