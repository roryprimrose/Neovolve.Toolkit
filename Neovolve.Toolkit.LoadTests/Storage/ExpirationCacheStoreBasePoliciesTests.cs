namespace Neovolve.Toolkit.LoadTests.Storage
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="ExpirationCacheStoreBasePoliciesTests"/>
    ///   class is used to test the <see cref="ExpirationCacheStoreBase"/> class.
    /// </summary>
    [TestClass]
    public class ExpirationCacheStoreBasePoliciesTests
    {
        /// <summary>
        ///   Stores the cache.
        /// </summary>
        private DictionaryCacheStore _store;

        #region Setup/Teardown

        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _store = new DictionaryCacheStore();

            for (Int32 index = 0; index < 1000; index++)
            {
                if (index % 2 == 0)
                {
                    _store.Add("Expiration" + index, index);
                }
                else
                {
                    TimeSpan slidingExpiration = new TimeSpan(0, 0, 2);
                    SlidingExpirationPolicy expirationPolicy = new SlidingExpirationPolicy(slidingExpiration);

                    _store.Add("Expiration" + index, index, expirationPolicy);
                }
            }
        }

        #endregion

        /// <summary>
        /// Runs test for expiration cache store add new item.
        /// </summary>
        [TestMethod]
        public void ExpirationCacheStoreAddNewItemTest()
        {
            _store.Add(Guid.NewGuid().ToString(), Environment.TickCount);
        }

        /// <summary>
        /// Runs test for expiration cache store add new item with policy.
        /// </summary>
        [TestMethod]
        public void ExpirationCacheStoreAddNewItemWithPolicyTest()
        {
            TimeSpan slidingExpiration = new TimeSpan(0, 0, 2);
            SlidingExpirationPolicy expirationPolicy = new SlidingExpirationPolicy(slidingExpiration);

            _store.Add(Guid.NewGuid().ToString(), Environment.TickCount, expirationPolicy);
        }

        /// <summary>
        /// Runs test for expiration cache store evaluates policies.
        /// </summary>
        [TestMethod]
        public void ExpirationCacheStoreEvaluatesPoliciesTest()
        {
            _store.GetItem("Expiration999");
        }

        /// <summary>
        /// Runs test for expiration cache store remove item.
        /// </summary>
        [TestMethod]
        public void ExpirationCacheStoreRemoveItemTest()
        {
            String key = _store.Keys.First();

            _store.Remove(key);
        }

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}