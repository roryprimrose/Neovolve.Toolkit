namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="SlidingExpirationPolicyTests"/>
    ///   class is used to test the <see cref="SlidingExpirationPolicy"/> class.
    /// </summary>
    [TestClass]
    public class SlidingExpirationPolicyTests
    {
        /// <summary>
        /// Runs test for calling notify item added has no affect.
        /// </summary>
        [TestMethod]
        public void CallingNotifyItemAddedHasNoAffectTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 50);
            SlidingExpirationPolicy target = new SlidingExpirationPolicy(expiration);

            target.NotifyItemAdded(this);
        }

        /// <summary>
        /// Runs test for calling notify item read updates last read time.
        /// </summary>
        [TestMethod]
        public void CallingNotifyItemReadUpdatesLastReadTimeTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 50);
            SlidingExpirationPolicyWrapper target = new SlidingExpirationPolicyWrapper(expiration);
            DateTime current = DateTime.Now;

            Assert.IsTrue(current >= target.ActualLastReadTime, "Initial read time set incorrectly");

            Thread.Sleep(15);

            target.NotifyItemRead(this);

            Assert.IsTrue(current < target.ActualLastReadTime, "LastReadTime was not updated");
        }

        /// <summary>
        /// Runs test for calling notify item removed has no affect.
        /// </summary>
        [TestMethod]
        public void CallingNotifyItemRemovedHasNoAffectTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 50);
            SlidingExpirationPolicy target = new SlidingExpirationPolicy(expiration);

            target.NotifyItemRemoved(this);
        }

        /// <summary>
        /// Runs test for creating Relative expiration policy correctly stores expiration.
        /// </summary>
        [TestMethod]
        public void CreatingRelativeExpirationPolicyCorrectlyStoresExpirationTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 5);
            SlidingExpirationPolicy target = new SlidingExpirationPolicy(expiration);

            Assert.AreEqual(expiration, target.Expiration, "Expiration returned an incorrect value");
        }

        /// <summary>
        /// Runs test for policy is expired after item read and new time span expired.
        /// </summary>
        [TestMethod]
        public void PolicyIsExpiredAfterItemReadAndNewTimeSpanExpiredTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 50);
            SlidingExpirationPolicyWrapper target = new SlidingExpirationPolicyWrapper(expiration);
            DateTime startPoint = DateTime.Now;

            target.TestExpires = startPoint.Add(expiration);
            target.TestLastRead = startPoint.AddMilliseconds(40);
            target.TestCurrentTime = startPoint.AddMilliseconds(80);

            Boolean actual = target.HasExpired;

            Assert.IsTrue(actual, "HasExpired returned an incorrect value");
        }

        /// <summary>
        /// Runs test for policy is expired when time span has passed without A read.
        /// </summary>
        [TestMethod]
        public void PolicyIsExpiredWhenTimeSpanHasPassedWithoutAReadTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 50);
            SlidingExpirationPolicyWrapper target = new SlidingExpirationPolicyWrapper(expiration);
            DateTime startPoint = DateTime.Now;

            target.TestExpires = startPoint.Add(expiration);
            target.TestLastRead = startPoint;
            target.TestCurrentTime = startPoint.AddMilliseconds(80);

            Boolean actual = target.HasExpired;

            Assert.IsTrue(actual, "HasExpired returned an incorrect value");
        }

        /// <summary>
        /// Runs test for policy is not expired when item read before original time span has passed.
        /// </summary>
        [TestMethod]
        public void PolicyIsNotExpiredWhenItemReadBeforeOriginalTimeSpanHasPassedTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 50);
            SlidingExpirationPolicyWrapper target = new SlidingExpirationPolicyWrapper(expiration);
            DateTime startPoint = DateTime.Now;

            target.TestExpires = startPoint.Add(expiration);
            target.TestLastRead = startPoint.AddMilliseconds(30);
            target.TestCurrentTime = startPoint.AddMilliseconds(30);

            Boolean actual = target.HasExpired;

            Assert.IsFalse(actual, "HasExpired returned an incorrect value");
        }

        /// <summary>
        /// Runs test for policy is not expired for expiration in the future.
        /// </summary>
        [TestMethod]
        public void PolicyIsNotExpiredWhenTimeSpanHasNotPassedTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 50);
            SlidingExpirationPolicyWrapper target = new SlidingExpirationPolicyWrapper(expiration);
            DateTime startPoint = DateTime.Now;

            target.TestExpires = startPoint.Add(expiration);
            target.TestLastRead = startPoint;
            target.TestCurrentTime = startPoint.AddMilliseconds(30);

            Boolean actual = target.HasExpired;

            Assert.IsFalse(actual, "HasExpired returned an incorrect value");
        }

        /// <summary>
        /// Runs test for zero expiration length expires immediately.
        /// </summary>
        [TestMethod]
        public void ZeroExpirationLengthExpiresImmediatelyTest()
        {
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, 0);
            SlidingExpirationPolicy target = new SlidingExpirationPolicy(expiration);

            Boolean actual = target.HasExpired;

            Assert.IsTrue(actual, "HasExpired returned an incorrect value");
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