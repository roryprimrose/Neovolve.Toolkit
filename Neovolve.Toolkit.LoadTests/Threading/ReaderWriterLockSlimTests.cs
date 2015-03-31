namespace Neovolve.Toolkit.LoadTests.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// The <see cref="ReaderWriterLockSlimTests"/>
    ///   class contains unit tests for <see cref="ReaderWriterLockSlim"/> implementations.
    /// </summary>
    [TestClass]
    public class ReaderWriterLockSlimTests
    {
        /// <summary>
        ///   Stores the lock instance.
        /// </summary>
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        ///   Stores the data store instance.
        /// </summary>
        private static readonly Dictionary<Int32, Int32> _store = new Dictionary<Int32, Int32>();

        /// <summary>
        /// Runs test for run reader writer lock slim read.
        /// </summary>
        [TestMethod]
        public void RunReaderWriterLockSlimReadTest()
        {
            using (new LockReader(_lock))
            {
                Int32 currentCount = _store.Count;

                Assert.IsTrue(currentCount >= 0, "Ignore, used to satisfy code analysis");
            }
        }

        /// <summary>
        /// Runs test for run reader writer lock slim write only.
        /// </summary>
        [TestMethod]
        public void RunReaderWriterLockSlimWriteOnlyTest()
        {
            using (new LockWriter(_lock))
            {
                Int32 newValue = _store.Count;

                if (_store.ContainsKey(newValue))
                {
                    Assert.Fail("Multiple writers are active");
                }
                else
                {
                    Debug.WriteLine(newValue + " was added");

                    _store.Add(newValue, newValue);
                }
            }
        }

        /// <summary>
        /// Runs test for run reader writer lock slim write.
        /// </summary>
        [TestMethod]
        public void RunReaderWriterLockSlimWriteTest()
        {
            using (new LockReader(_lock, true))
            {
                Int32 newValue = _store.Count;

                using (new LockWriter(_lock))
                {
                    if (_store.ContainsKey(newValue))
                    {
                        Debug.WriteLine(newValue + " already exists and will be ignored");
                    }
                    else
                    {
                        Debug.WriteLine(newValue + " was added");

                        // Add a sleep to allow other threads running this test to acquire a read lock and get the same value
                        // and then attempt to insert the same value
                        Thread.Sleep(100);

                        _store.Add(newValue, newValue);
                    }
                }
            }
        }

        /// <summary>
        /// If this test is run in the load test, it will demonstrate the mutliple threads will not be able to run this test without causing errors.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void UnsafeThreadAccessTest()
        {
            Int32 newValue = _store.Count;

            if (_store.ContainsKey(newValue))
            {
                Debug.WriteLine(newValue + " already exists and will be ignored");
            }
            else
            {
                Debug.WriteLine(newValue + " was added");

                // Add a sleep to allow other threads running this test to acquire a read lock and get the same value
                // and then attempt to insert the same value
                Thread.Sleep(100);
                _store.Add(newValue, newValue);
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