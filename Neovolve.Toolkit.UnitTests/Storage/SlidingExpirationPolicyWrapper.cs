namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="SlidingExpirationPolicyWrapper"/>
    ///   class is used to wrap the <see cref="SlidingExpirationPolicy"/> for testing.
    /// </summary>
    public class SlidingExpirationPolicyWrapper : SlidingExpirationPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SlidingExpirationPolicyWrapper"/> class.
        /// </summary>
        /// <param name="relativeExpiration">
        /// The relative expiration.
        /// </param>
        public SlidingExpirationPolicyWrapper(TimeSpan relativeExpiration)
            : base(relativeExpiration)
        {
        }

        /// <summary>
        ///   Gets the actual last read time.
        /// </summary>
        /// <value>
        ///   The actual last read time.
        /// </value>
        public DateTime ActualLastReadTime
        {
            get
            {
                return base.LastRead;
            }
        }

        /// <summary>
        ///   Gets or sets the test current time.
        /// </summary>
        /// <value>
        ///   The test current time.
        /// </value>
        public DateTime TestCurrentTime
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the test expires.
        /// </summary>
        /// <value>
        ///   The test expires.
        /// </value>
        public DateTime TestExpires
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the test last read.
        /// </summary>
        /// <value>
        ///   The test last read.
        /// </value>
        public DateTime TestLastRead
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the current time.
        /// </summary>
        /// <returns>
        ///   A <see cref = "DateTime" /> instance.
        /// </returns>
        /// <value>
        ///   The current time.
        /// </value>
        protected override DateTime CurrentTime
        {
            get
            {
                return TestCurrentTime;
            }
        }

        /// <summary>
        ///   Gets the expiration point.
        /// </summary>
        /// <returns>
        ///   A <see cref = "DateTime" /> instance.
        /// </returns>
        /// <value>
        ///   The expires.
        /// </value>
        protected override DateTime Expires
        {
            get
            {
                return TestExpires;
            }
        }

        /// <summary>
        ///   Gets the item last read.
        /// </summary>
        /// <returns>
        ///   A <see cref = "DateTime" /> instance.
        /// </returns>
        /// <value>
        ///   The last read.
        /// </value>
        protected override DateTime LastRead
        {
            get
            {
                return TestLastRead;
            }
        }
    }
}