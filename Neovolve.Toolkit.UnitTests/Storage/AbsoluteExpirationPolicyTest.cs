namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// This is a test class for AbsoluteExpirationPolicyTest and is intended
    ///   to contain all AbsoluteExpirationPolicyTest Unit Tests.
    /// </summary>
    [TestClass]
    public class AbsoluteExpirationPolicyTest
    {
        /// <summary>
        /// Runs test for calling notify item added has no affect.
        /// </summary>
        [TestMethod]
        public void CallingNotifyItemAddedHasNoAffectTest()
        {
            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));

            target.NotifyItemAdded(this);
        }

        /// <summary>
        /// Runs test for calling notify item read has no affect.
        /// </summary>
        [TestMethod]
        public void CallingNotifyItemReadHasNoAffectTest()
        {
            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));

            target.NotifyItemRead(this);
        }

        /// <summary>
        /// Runs test for calling notify item removed has no affect.
        /// </summary>
        [TestMethod]
        public void CallingNotifyItemRemovedHasNoAffectTest()
        {
            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));

            target.NotifyItemRemoved(this);
        }

        /// <summary>
        /// Runs test for creating absolute expiration policy correctly stores date time expiration.
        /// </summary>
        [TestMethod]
        public void CreatingAbsoluteExpirationPolicyCorrectlyStoresDateTimeExpirationTest()
        {
            DateTime expiration = DateTime.Now;

            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(expiration);

            Assert.AreEqual(expiration, target.Expiration, "Expiration returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating absolute expiration policy correctly stores time span expiration.
        /// </summary>
        [TestMethod]
        public void CreatingAbsoluteExpirationPolicyCorrectlyStoresTimeSpanExpirationTest()
        {
            TimeSpan expiration = new TimeSpan(12, 34, 14);
            DateTime expectedExpiration = DateTime.Now.Add(expiration);

            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(expiration);

            // Need to test greater than or equal to in order to cater for the ticks between assignment and policy creation
            Assert.IsTrue(target.Expiration >= expectedExpiration, "Expiration returned an incorrect value");
            Assert.IsTrue(target.Expiration < expectedExpiration.AddSeconds(1), "Expiration returned an incorrect value");
        }

        /// <summary>
        /// Runs test for policy is expired for expiration in the past.
        /// </summary>
        [TestMethod]
        public void PolicyIsExpiredForExpirationInThePastTest()
        {
            DateTime expiration = DateTime.Now.AddDays(-1);

            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(expiration);
            Boolean actual = target.HasExpired;

            Assert.IsTrue(actual, "HasExpired returned an incorrect value");
        }

        /// <summary>
        /// Runs test for policy is not expired for expiration in the future.
        /// </summary>
        [TestMethod]
        public void PolicyIsNotExpiredForExpirationInTheFutureTest()
        {
            DateTime expiration = DateTime.Now.AddDays(1);

            AbsoluteExpirationPolicy target = new AbsoluteExpirationPolicy(expiration);
            Boolean actual = target.HasExpired;

            Assert.IsFalse(actual, "HasExpired returned an incorrect value");
        }

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///   The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}