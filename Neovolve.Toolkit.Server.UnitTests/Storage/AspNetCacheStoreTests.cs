namespace Neovolve.Toolkit.Server.UnitTests.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Server.Storage;

    /// <summary>
    /// The <see cref="AspNetCacheStoreTests"/>
    ///   class is used to test the <see cref="AspNetCacheStore"/> class.
    /// </summary>
    [TestClass]
    public class AspNetCacheStoreTests
    {
        /// <summary>
        /// Runs test for clear removes items from cache.
        /// </summary>
        [TestMethod]
        public void ClearRemovesItemsFromCacheTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();

            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsTrue(HttpRuntime.Cache.Count > 0, "Cache does not contain the expected value");

            target.Clear();

            Assert.AreEqual(0, HttpRuntime.Cache.Count, "Cache was not cleared as expected");
        }

        /// <summary>
        /// Runs test for contains returns false for item missing from cache.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsFalseForItemMissingFromCacheTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();

            Assert.IsFalse(target.Contains(key), "Contains returned an incorrect value");
        }

        /// <summary>
        /// Runs test for contains returns true for item in cache.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsTrueForItemInCacheTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();
            Object value = Guid.NewGuid().ToString();

            target.Add(key, value);

            Assert.IsTrue(target.Contains(key), "Contains returned an incorrect value");
        }

        /// <summary>
        /// Runs test for count.
        /// </summary>
        [TestMethod]
        public void CountTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();

            target.Clear();

            Assert.AreEqual(0, target.Count, "Cache was not cleared as expected");

            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.AreEqual(1, target.Count, "Cache was not cleared as expected");

            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.AreEqual(2, target.Count, "Cache was not cleared as expected");
        }

        /// <summary>
        /// Runs test for item correctly adds new item.
        /// </summary>
        [TestMethod]
        public void ItemCorrectlyAddsNewItemTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();
            Object expected = Guid.NewGuid().ToString();

            target.Add(key, expected);

            Object actual = HttpRuntime.Cache[key];

            Assert.AreEqual(expected, actual, "this[Key]_get returned an incorrect value");
        }

        /// <summary>
        /// Runs test for item correctly updates existing item.
        /// </summary>
        [TestMethod]
        public void ItemCorrectlyUpdatesExistingItemTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();
            const String FirstValue = "first value";

            target.Add(key, FirstValue);

            Assert.AreEqual(FirstValue, HttpRuntime.Cache[key], "Cache does not contain the expected value");

            const String SecondValue = "second value";

            target.Add(key, SecondValue);

            Assert.AreEqual(SecondValue, HttpRuntime.Cache[key], "Cache did not correctly store the second value");

            const String ThirdValue = "third value";

            target.Add(key, ThirdValue);

            Assert.AreEqual(ThirdValue, HttpRuntime.Cache[key], "Cache did not correctly store the third value");
        }

        /// <summary>
        /// Runs test for item returns correct value for item in cache.
        /// </summary>
        [TestMethod]
        public void ItemReturnsCorrectValueForItemInCacheTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();
            Object expected = Guid.NewGuid().ToString();

            target.Add(key, expected);

            Object actual = target.GetItem(key);

            Assert.AreEqual(expected, actual, "this[Key]_get returned an incorrect value");
        }

        /// <summary>
        /// Runs test for item returns null for item missing from cache.
        /// </summary>
        [TestMethod]
        public void ItemReturnsNullForItemMissingFromCacheTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();

            Object actual = target.GetItem(key);

            Assert.IsNull(actual, "this[Key]_get returned an incorrect value");
        }

        /// <summary>
        /// Runs test for keys returns keys added.
        /// </summary>
        [TestMethod]
        public void KeysReturnsKeysAddedTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String firstKey = Guid.NewGuid().ToString();
            String secondKey = Guid.NewGuid().ToString();

            target.Clear();

            target.Add(firstKey, Guid.NewGuid().ToString());
            target.Add(secondKey, Guid.NewGuid().ToString());

            IEnumerable<String> keys = target.Keys;

            Assert.IsNotNull(keys, "Keys faileed to return an instance");
            Assert.AreEqual(2, keys.Count(), "Incorrect number of keys stored");

            // The Asp.Net cache does not ensure the same ordering of keys as they were added to the cache
            if (firstKey == keys.First())
            {
                Assert.AreEqual(firstKey, keys.First(), "First key value is incorrect");
                Assert.AreEqual(secondKey, keys.Skip(1).First(), "Second key value is incorrect");
            }
            else
            {
                Assert.AreEqual(firstKey, keys.Skip(1).First(), "First key value is incorrect");
                Assert.AreEqual(secondKey, keys.First(), "Second key value is incorrect");
            }
        }

        /// <summary>
        /// Runs test for remove.
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            AspNetCacheStore target = new AspNetCacheStore();
            String key = Guid.NewGuid().ToString();
            Object expected = Guid.NewGuid().ToString();

            target.Add(key, expected);

            Object actual = target.Remove(key);

            Assert.IsNull(HttpRuntime.Cache[key], "Cache still contains the item that was removed");

            Assert.AreEqual(expected, actual, "Remove returned an incorrect value");
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