namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="CacheStoreFactoryTests"/>
    ///   class is used to test the <see cref="CacheStoreFactory"/> class.
    /// </summary>
    [TestClass]
    public class CacheStoreFactoryTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            CacheStoreFactory.StoreType = null;
        }

        #endregion

        /// <summary>
        /// Runs test for assigning A store type that is not I cache store type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void AssigningAStoreTypeThatIsNotICacheStoreTypeThrowsExceptionTest()
        {
            CacheStoreFactory.StoreType = typeof(NotSupportedException);
        }

        /// <summary>
        /// Runs test for cache store type returns an instance.
        /// </summary>
        [TestMethod]
        public void CacheStoreTypeReturnsAnICacheStoreTypeTest()
        {
            Type actual = CacheStoreFactory.StoreType;

            Assert.IsNotNull(actual, "StoreType failed to return an instance");
            Assert.IsTrue(typeof(ICacheStore).IsAssignableFrom(actual), "StoreType returned an unsupported type");
        }

        /// <summary>
        /// Runs test for can assign A store type that is I cache store type.
        /// </summary>
        [TestMethod]
        public void CanAssignAStoreTypeThatIsICacheStoreTypeTest()
        {
            CacheStoreFactory.StoreType = typeof(DictionaryCacheStore);
        }

        /// <summary>
        /// Runs test for can set cache store type to null.
        /// </summary>
        [TestMethod]
        public void CanSetCacheStoreTypeToNullTest()
        {
            CacheStoreFactory.StoreType = null;
        }

        /// <summary>
        /// Runs test for create returns default cache store.
        /// </summary>
        [TestMethod]
        public void CreateReturnsDefaultCacheStoreTest()
        {
            CacheStoreFactory.StoreType = null;
            ICacheStore actual = CacheStoreFactory.Create();

            Assert.IsInstanceOfType(actual, typeof(DictionaryCacheStore), "Create failed to return an instance");
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