namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ExpirationCacheStoreBaseTests"/>
    ///   class is used to test the <see cref="ExpirationCacheStoreBase"/> class.
    /// </summary>
    [TestClass]
    public class ExpirationCacheStoreBaseTests
    {
        /// <summary>
        /// Runs test for adding item with policies adds item to cache.
        /// </summary>
        [TestMethod]
        public void AddingItemWithPoliciesAddsItemToCacheTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, null);

            Assert.AreSame(item, target.StoredItem, "StoredItem returned any incorrect value");
        }

        /// <summary>
        /// Runs test for adding item with policy notifies policy of cache addition.
        /// </summary>
        [TestMethod]
        public void AddingItemWithPolicyNotifiesPolicyOfCacheAdditionTest()
        {
            MockRepository mock = new MockRepository();
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            IExpirationPolicy policy = mock.StrictMock<IExpirationPolicy>();
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            using (mock.Record())
            {
                policy.NotifyItemAdded(item);
            }

            using (mock.Playback())
            {
                target.Add(key, item, policySet);
            }
        }

        /// <summary>
        /// Runs test for adding new item through indexer overwrites existing policy.
        /// </summary>
        [TestMethod]
        public void AddingNewItemThroughIndexerOverwritesExistingPolicyTest()
        {
            MockRepository mock = new MockRepository();
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            IExpirationPolicy policy = mock.Stub<IExpirationPolicy>();
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            SetupResult.For(policy.HasExpired).Return(true);
            mock.Replay(policy);

            target.Add(key, item, policySet);

            Object newItem = new Object();

            // This should clear the existing policy that marks the item as being expired when the test runs
            target.Add(key, newItem);

            Assert.AreSame(newItem, target.GetItem(key), "Indexer returned an incorrect value");
        }

        /// <summary>
        /// Runs test for cache returns null with expired policy.
        /// </summary>
        [TestMethod]
        public void CacheReturnsNullWithExpiredPolicyTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy policy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(-1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Assert.IsNull(target.GetItem(key), "Indexer returned an incorrect result");
        }

        /// <summary>
        /// Runs test for cache returns null with mixed policy expiry.
        /// </summary>
        [TestMethod]
        public void CacheReturnsNullWithMixedPolicyExpiryTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy unexpiredPolicy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));
            AbsoluteExpirationPolicy expiredPolicy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(-1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                unexpiredPolicy, expiredPolicy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Assert.IsNull(target.GetItem(key), "Indexer returned an incorrect result");
        }

        /// <summary>
        /// Runs test for clear notifies policies of cache item removal.
        /// </summary>
        [TestMethod]
        public void ClearNotifiesPoliciesOfCacheItemRemovalTest()
        {
            MockRepository mock = new MockRepository();
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            IExpirationPolicy policy = mock.StrictMock<IExpirationPolicy>();
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            using (mock.Record())
            {
                policy.NotifyItemAdded(item);

                policy.NotifyItemRemoved(item);
            }

            using (mock.Playback())
            {
                target.Add(key, item, policySet);

                target.Clear();
            }
        }

        /// <summary>
        /// Runs test for contains returns item with unexpired policy.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsItemWithUnexpiredPolicyTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy policy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Boolean actual = target.Contains(key);

            Assert.IsTrue(actual, "Contains returned an incorrect result");
        }

        /// <summary>
        /// Runs test for contains returns null for item with expired policy.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsNullForItemWithExpiredPolicyTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy policy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(-1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Boolean actual = target.Contains(key);

            Assert.IsFalse(actual, "Contains returned an incorrect result");
        }

        /// <summary>
        /// Runs test for contains returns true with null policies.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsTrueWithNullPoliciesTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, null);

            Boolean actual = target.Contains(key);

            Assert.IsTrue(actual, "Contains returned an incorrect result");
        }

        /// <summary>
        /// Runs test for count returns one with unexpired policy.
        /// </summary>
        [TestMethod]
        public void CountReturnsOneWithUnexpiredPolicyTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy policy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Assert.AreEqual(1, target.Count, "Count returned an incorrect result");
        }

        /// <summary>
        /// Runs test for count returns zero with expired policy.
        /// </summary>
        [TestMethod]
        public void CountReturnsZeroWithExpiredPolicyTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy policy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(-1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Assert.AreEqual(0, target.Count, "Count returned an incorrect result");
        }

        /// <summary>
        /// Runs test for item can be read from cache with empty policies.
        /// </summary>
        [TestMethod]
        public void ItemCanBeReadFromCacheWithEmptyPoliciesTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            String key = Guid.NewGuid().ToString();
            Object item = new Object();
            IExpirationPolicy[] policySet = new IExpirationPolicy[0];

            target.Add(key, item, policySet);

            Object actual = target.GetItem(key);

            Assert.AreSame(item, actual, "Indexer returned an incorrect value");
        }

        /// <summary>
        /// Runs test for item can be read from cache with null policies.
        /// </summary>
        [TestMethod]
        public void ItemCanBeReadFromCacheWithNullPoliciesTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, null);

            Assert.AreSame(item, target.GetItem(key), "Indexer returned an incorrect value");
        }

        /// <summary>
        /// Runs test for item can be read from cache with unexpired policy.
        /// </summary>
        [TestMethod]
        public void ItemCanBeReadFromCacheWithUnexpiredPolicyTest()
        {
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            AbsoluteExpirationPolicy policy = new AbsoluteExpirationPolicy(DateTime.Now.AddDays(1));
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            target.Add(key, item, policySet);

            Assert.AreSame(item, target.GetItem(key), "Indexer returned an incorrect result");
        }

        /// <summary>
        /// Runs test for remove notifies policy of cache item removal.
        /// </summary>
        [TestMethod]
        public void RemoveNotifiesPolicyOfCacheItemRemovalTest()
        {
            MockRepository mock = new MockRepository();
            ExpirationCacheStoreBaseStub target = new ExpirationCacheStoreBaseStub();
            IExpirationPolicy policy = mock.StrictMock<IExpirationPolicy>();
            IExpirationPolicy[] policySet = new[]
                                            {
                                                policy
                                            };
            String key = Guid.NewGuid().ToString();
            Object item = new Object();

            using (mock.Record())
            {
                policy.NotifyItemAdded(item);
                policy.NotifyItemRemoved(item);
            }

            using (mock.Playback())
            {
                target.Add(key, item, policySet);

                target.Remove(key);
            }
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