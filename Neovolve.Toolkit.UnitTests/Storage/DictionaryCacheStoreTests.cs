namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// This is a test class for DictionaryCacheStoreTests and is intended
    ///   to contain all DictionaryCacheStoreTests Unit Tests.
    /// </summary>
    [TestClass]
    public class DictionaryCacheStoreTests
    {
        /// <summary>
        /// Runs test for clear removes items from cache.
        /// </summary>
        [TestMethod]
        public void ClearRemovesItemsFromCacheTest()
        {
            DictionaryCacheStore target = new DictionaryCacheStore();

            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            target.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsTrue(target.Count > 0, "Cache does not contain the expected value");

            target.Clear();

            Assert.AreEqual(0, target.Count, "Cache was not cleared as expected");
        }

        /// <summary>
        /// Runs test for contains returns false for item missing from cache.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsFalseForItemMissingFromCacheTest()
        {
            DictionaryCacheStore target = new DictionaryCacheStore();
            String key = Guid.NewGuid().ToString();

            Assert.IsFalse(target.Contains(key), "Contains returned an incorrect value");
        }

        /// <summary>
        /// Runs test for contains returns true for item in cache.
        /// </summary>
        [TestMethod]
        public void ContainsReturnsTrueForItemInCacheTest()
        {
            DictionaryCacheStore target = new DictionaryCacheStore();
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
            DictionaryCacheStore target = new DictionaryCacheStore();

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
            DictionaryCacheStore target = new DictionaryCacheStore();
            String key = Guid.NewGuid().ToString();
            Object expected = Guid.NewGuid().ToString();

            target.Add(key, expected);

            Object actual = target.GetItem(key);

            Assert.AreEqual(expected, actual, "this[Key]_get returned an incorrect value");
        }

        /// <summary>
        /// Runs test for item correctly updates existing item.
        /// </summary>
        [TestMethod]
        public void ItemCorrectlyUpdatesExistingItemTest()
        {
            DictionaryCacheStore target = new DictionaryCacheStore();
            String key = Guid.NewGuid().ToString();
            const String FirstValue = "first value";

            target.Add(key, FirstValue);

            Assert.AreEqual(FirstValue, target.GetItem(key), "Cache does not contain the expected value");

            const String SecondValue = "second value";

            target.Add(key, SecondValue);

            Assert.AreEqual(SecondValue, target.GetItem(key), "Cache did not correctly store the second value");

            const String ThirdValue = "third value";

            target.Add(key, ThirdValue);

            Assert.AreEqual(ThirdValue, target.GetItem(key), "Cache did not correctly store the third value");
        }

        /// <summary>
        /// Runs test for item returns correct value for item in cache.
        /// </summary>
        [TestMethod]
        public void ItemReturnsCorrectValueForItemInCacheTest()
        {
            DictionaryCacheStore target = new DictionaryCacheStore();
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
            DictionaryCacheStore target = new DictionaryCacheStore();
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
            DictionaryCacheStore target = new DictionaryCacheStore();

            String firstKey = Guid.NewGuid().ToString();
            String secondKey = Guid.NewGuid().ToString();

            target.Clear();

            target.Add(firstKey, Guid.NewGuid().ToString());
            target.Add(secondKey, Guid.NewGuid().ToString());

            IEnumerable<String> keys = target.Keys;

            Assert.IsNotNull(keys, "Keys faileed to return an instance");
            Assert.AreEqual(2, keys.Count(), "Incorrect number of keys stored");
            Assert.AreEqual(firstKey, keys.First(), "First key value is incorrect");
            Assert.AreEqual(secondKey, keys.Skip(1).First(), "Second key value is incorrect");
        }

        /// <summary>
        /// Runs test for remove.
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            DictionaryCacheStore target = new DictionaryCacheStore();
            String key = Guid.NewGuid().ToString();
            Object expected = Guid.NewGuid().ToString();

            target.Add(key, expected);

            Object actual = target.Remove(key);

            Assert.IsNull(target.GetItem(key), "Cache still contains the item that was removed");

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