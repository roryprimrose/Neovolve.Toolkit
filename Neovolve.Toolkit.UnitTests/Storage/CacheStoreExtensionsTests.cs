namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using System.Drawing;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="CacheStoreExtensionsTests"/>
    ///   class is used to test the <see cref="CacheStoreExtensions"/> class.
    /// </summary>
    [TestClass]
    public class CacheStoreExtensionsTests
    {
        /// <summary>
        /// Runs test for cache store get item converts value with type converter.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemConvertsValueWithTypeConverterTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            Point expected = new Point(2, 4);

            store.Stub(x => x.Contains(key)).Return(true);
            store.Stub(x => x.GetItem(key)).Return("2, 4");

            Point actual = store.GetItem(key, () => Point.Empty);

            Assert.AreEqual(expected, actual, "GetItem returned an incorrect value");
        }

        /// <summary>
        /// Runs test for cache store get item does not put null values into the store.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemDoesNotPutNullValuesIntoTheStoreTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = null;

            store.Stub(x => x.Contains(key)).Return(false);

            String actual = store.GetItem(key, () => expected);

            Assert.AreEqual(expected, actual, "GetItem returned an incorrect value");
            store.AssertWasNotCalled(x => x.Add(key, null));
        }

        /// <summary>
        /// Runs test for cache store get item resolves item when it is not in the cache.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemResolvesItemWhenItIsNotInTheCacheTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();

            store.Stub(x => x.Contains(key)).Return(false);

            String actual = store.GetItem(key, () => expected);

            Assert.AreEqual(expected, actual, "GetItem returned an incorrect value");
        }

        /// <summary>
        /// Runs test for cache store get item returns item from cache.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemReturnsItemFromCacheTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();

            store.Stub(x => x.Contains(key)).Return(true);
            store.Stub(x => x.GetItem(key)).Return(expected);

            String actual = store.GetItem(
                key, 
                () =>
                {
                    Assert.Fail("Function should not have been called");

                    return String.Empty;
                });

            Assert.AreEqual(expected, actual, "GetItem returned an incorrect value");
        }

        /// <summary>
        /// Runs test for cache store get item safely casts value type.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemSafelyCastsValueTypeTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            IConvertible expected = MockRepository.GenerateStub<IConvertible>();
            String calculatedValue = Guid.NewGuid().ToString();

            store.Stub(x => x.Contains(key)).Return(true);
            store.Stub(x => x.GetItem(key)).Return(expected);
            expected.Stub(x => x.ToString(null)).IgnoreArguments().Return(calculatedValue);

            String actual = store.GetItem(key, () => calculatedValue);

            Assert.AreEqual(calculatedValue, actual, "GetItem returned an incorrect value");
        }

        /// <summary>
        /// Runs test for cache store get item stores item into the cache when resolved.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemStoresItemIntoTheCacheWhenResolvedTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();

            store.Stub(x => x.Contains(key)).Return(false);

            store.GetItem(key, () => expected);

            store.AssertWasCalled(x => x.Add(key, expected, null));
        }

        /// <summary>
        /// Runs test for cache store get item stores item into the cache with expiration policies array.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemStoresItemIntoTheCacheWithExpirationPoliciesArrayTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();
            IExpirationPolicy policy = MockRepository.GenerateStub<IExpirationPolicy>();

            store.Stub(x => x.Contains(key)).Return(false);

            store.GetItem(
                key, 
                () => expected, 
                new[]
                {
                    policy
                });

            store.AssertWasCalled(x => x.Add(key, expected, policy));
        }

        /// <summary>
        /// Runs test for cache store get item stores item into the cache with expiration policies function.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemStoresItemIntoTheCacheWithExpirationPoliciesFunctionTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();
            IExpirationPolicy policy = MockRepository.GenerateStub<IExpirationPolicy>();

            store.Stub(x => x.Contains(key)).Return(false);

            store.GetItem(
                key, 
                () => expected, 
                () => new[]
                      {
                          policy
                      });

            store.AssertWasCalled(x => x.Add(key, expected, policy));
        }

        /// <summary>
        /// Runs test for cache store get item throws invalid cast exception for an invalid type request.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CacheStoreGetItemThrowsInvalidCastExceptionForAnInvalidTypeRequestTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();

            store.Stub(x => x.Contains(key)).Return(true);
            store.Stub(x => x.GetItem(key)).Return(expected);

            store.GetItem(key, () => (Stream)null);
        }

        /// <summary>
        /// Runs test for cache store get item with no resolver returns default value if item not in cache.
        /// </summary>
        [TestMethod]
        public void CacheStoreGetItemWithNoResolverReturnsDefaultValueIfItemNotInCacheTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();
            String key = Guid.NewGuid().ToString();

            store.Stub(x => x.Contains(key)).Return(false);

            Point actual = store.GetItem<Point>(key);

            Assert.AreEqual(default(Point), actual, "GetItem returned an incorrect value");
        }

        /// <summary>
        /// Runs test for cache store get item with no resolver throws exception with empty key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithNoResolverThrowsExceptionWithEmptyKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem<Point>(String.Empty);
        }

        /// <summary>
        /// Runs test for cache store get item with no resolver throws exception with null key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithNoResolverThrowsExceptionWithNullKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem<Point>(null);
        }

        /// <summary>
        /// Runs test for cache store get item with no resolver throws exception with white space key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithNoResolverThrowsExceptionWithWhiteSpaceKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem<Point>(" ");
        }

        /// <summary>
        /// Runs test for cache store get item with resolver and policies throws exception with empty key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithResolverAndPoliciesThrowsExceptionWithEmptyKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem(String.Empty, () => String.Empty, () => null);
        }

        /// <summary>
        /// Runs test for cache store get item with resolver and policies throws exception with null key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithResolverAndPoliciesThrowsExceptionWithNullKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem(null, () => String.Empty, () => null);
        }

        /// <summary>
        /// Runs test for cache store get item with resolver and policies throws exception with white space key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithResolverAndPoliciesThrowsExceptionWithWhiteSpaceKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem(" ", () => String.Empty, () => null);
        }

        /// <summary>
        /// Runs test for cache store get item with resolver throws exception with empty key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithResolverThrowsExceptionWithEmptyKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem(String.Empty, () => String.Empty);
        }

        /// <summary>
        /// Runs test for cache store get item with resolver throws exception with null key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithResolverThrowsExceptionWithNullKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem(null, () => String.Empty);
        }

        /// <summary>
        /// Runs test for cache store get item with resolver throws exception with white space key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheStoreGetItemWithResolverThrowsExceptionWithWhiteSpaceKeyTest()
        {
            ICacheStore store = MockRepository.GenerateStub<ICacheStore>();

            store.GetItem(" ", () => String.Empty);
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