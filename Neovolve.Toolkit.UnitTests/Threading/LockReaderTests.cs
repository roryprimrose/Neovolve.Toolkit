namespace Neovolve.Toolkit.UnitTests.Threading
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// This is a test class for LockReaderTests and is intended
    ///   to contain all LockReaderTests Unit Tests.
    /// </summary>
    [TestClass]
    public class LockReaderTests
    {
        /// <summary>
        /// Runs test for lock reader reader writer lock external released.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockExternalReleasedTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            Assert.IsFalse(lockReference.IsWriterLockHeld, "IsWriterLockHeld returned an incorrect value");
            Assert.IsFalse(lockReference.IsReaderLockHeld, "IsReaderLockHeld returned an incorrect value");

            using (new LockReader(lockReference))
            {
                lockReference.ReleaseReaderLock();

                Assert.IsFalse(lockReference.IsWriterLockHeld, "IsWriterLockHeld returned an incorrect value");
                Assert.IsFalse(lockReference.IsReaderLockHeld, "IsReaderLockHeld returned an incorrect value");
            }

            Assert.IsFalse(lockReference.IsWriterLockHeld, "IsWriterLockHeld returned an incorrect value");
            Assert.IsFalse(lockReference.IsReaderLockHeld, "IsReaderLockHeld returned an incorrect value");
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock manual dispose.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockManualDisposeTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (LockReader reader = new LockReader(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, true, false);

                using (LockReader nestedReader = new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false);

                    nestedReader.Dispose();

                    TestHelper.ValidateLockStatus(lockReference, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, true, false);

                reader.Dispose();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock manual release.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockManualReleaseTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockReader(lockReference))
            {
                lockReference.ReleaseReaderLock();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock multiple dispose.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockMultipleDisposeTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (LockReader writer = new LockReader(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, true, false);

                writer.Dispose();
                writer.Dispose();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock nested.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockNestedTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockReader(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, true, false);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, true, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock null reader writer lock.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LockReaderReaderWriterLockNullReaderWriterLockTest()
        {
            const ReaderWriterLock LockReference = null;

            using (new LockReader(LockReference))
            {
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock parent writer.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockParentWriterTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockWriter(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, false, true);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, false, true);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock simple.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSimpleTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockReader(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, true, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim external released.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimExternalReleasedTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference))
                {
                    lockReference.ExitReadLock();

                    TestHelper.ValidateLockStatus(lockReference, false, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim int32.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimInt32Test()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                const Int32 Timeout = 123;

                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference, Timeout))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim manual dispose.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimManualDisposeTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (LockReader reader = new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    using (LockReader nestedReader = new LockReader(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, true, false, false);

                        nestedReader.Dispose();

                        TestHelper.ValidateLockStatus(lockReference, true, false, false);
                    }

                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    reader.Dispose();

                    TestHelper.ValidateLockStatus(lockReference, false, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim multiple dispose.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimMultipleDisposeTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (LockReader writer = new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    writer.Dispose();
                    writer.Dispose();

                    TestHelper.ValidateLockStatus(lockReference, false, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim nested reader no recursion.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimNestedReaderNoRecursionTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    try
                    {
                        using (new LockReader(lockReference))
                        {
                        }
                    }
                    catch (LockRecursionException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    TestHelper.ValidateLockStatus(lockReference, true, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim nested reader.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimNestedReaderTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    using (new LockReader(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, true, false, false);
                    }

                    TestHelper.ValidateLockStatus(lockReference, true, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim null reader writer lock slim.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LockReaderReaderWriterLockSlimNullReaderWriterLockSlimTest()
        {
            const ReaderWriterLockSlim LockReference = null;

            using (new LockReader(LockReference))
            {
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim parent writer.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimParentWriterTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);

                    using (new LockReader(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, true, true, false);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim simple.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimSimpleTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim timeout.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimTimeoutTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                Thread writeLockThread = new Thread(LockReaderReaderWriterLockSlimThreadHandler)
                                         {
                                             Name = "LockReaderReaderWriterLockSlimTimeoutTest",
                                             IsBackground = true
                                         };

                writeLockThread.Start(lockReference);

                Thread.Sleep(50);

                try
                {
                    using (new LockReader(lockReference, 10))
                    {
                    }

                    Assert.Fail("TimeoutException was expected");
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock slim upgradeable timeout.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockSlimUpgradeableTimeoutTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                Thread writeLockThread = new Thread(LockReaderReaderWriterLockSlimThreadHandler)
                                         {
                                             Name = "LockReaderReaderWriterLockSlimTimeoutTest",
                                             IsBackground = true
                                         };

                writeLockThread.Start(lockReference);

                Thread.Sleep(50);

                try
                {
                    using (new LockReader(lockReference, 10, true))
                    {
                    }

                    Assert.Fail("TimeoutException was expected");
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Runs test for lock reader reader writer lock timeout.
        /// </summary>
        [TestMethod]
        public void LockReaderReaderWriterLockTimeoutTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            Thread writeLockThread = new Thread(LockReaderReaderWriterLockThreadHandler)
                                     {
                                         Name = "LockReaderReaderWriterLockTimeoutTest",
                                         IsBackground = true
                                     };

            writeLockThread.Start(lockReference);

            Thread.Sleep(50);

            try
            {
                using (new LockReader(lockReference, 10))
                {
                }

                Assert.Fail("TimeoutException was expected");
            }
            catch (TimeoutException ex)
            {
                Assert.IsNotNull(ex.InnerException, "InnerException failed to return an instance");
                Assert.IsTrue(ex.InnerException is ApplicationException, "InnerException returned an incorrect type");
                Assert.AreEqual(ex.Message, ex.InnerException.Message, "Message returned an incorrect value");

                Debug.WriteLine(ex);
            }
        }

        #region Static Helper Methods

        /// <summary>
        /// Writes the lock reader writer lock slim thread handler.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private static void LockReaderReaderWriterLockSlimThreadHandler(Object state)
        {
            ReaderWriterLockSlim lockReference = state as ReaderWriterLockSlim;

            if (lockReference == null)
            {
                return;
            }

            using (new LockWriter(lockReference))
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Writes the lock reader writer lock slim thread handler.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private static void LockReaderReaderWriterLockThreadHandler(Object state)
        {
            ReaderWriterLock lockReference = state as ReaderWriterLock;

            if (lockReference == null)
            {
                return;
            }

            using (new LockWriter(lockReference))
            {
                Thread.Sleep(1000);
            }
        }

        #endregion

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