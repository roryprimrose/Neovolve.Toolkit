namespace Neovolve.Toolkit.UnitTests.Threading
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// This is a test class for LockWriterTests and is intended
    ///   to contain all LockWriterTests Unit Tests.
    /// </summary>
    [TestClass]
    public class LockWriterTests
    {
        /// <summary>
        /// Runs test for lock writer reader writer lock external released.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockExternalReleasedTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockWriter(lockReference))
            {
                lockReference.ReleaseWriterLock();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock manual dispose.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockManualDisposeTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (LockWriter writer = new LockWriter(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, false, true);

                using (LockWriter nestedWriter = new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true);

                    nestedWriter.Dispose();

                    TestHelper.ValidateLockStatus(lockReference, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, false, true);

                writer.Dispose();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock manual release.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockManualReleaseTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockWriter(lockReference))
            {
                lockReference.ReleaseReaderLock();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock multiple dispose.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockMultipleDisposeTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (LockWriter writer = new LockWriter(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, false, true);

                writer.Dispose();
                writer.Dispose();

                TestHelper.ValidateLockStatus(lockReference, false, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock nested.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockNestedTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockWriter(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, false, true);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, false, true);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock null reader writer lock.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LockWriterReaderWriterLockNullReaderWriterLockTest()
        {
            const ReaderWriterLock LockReference = null;

            using (new LockWriter(LockReference))
            {
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock simple.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSimpleTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockWriter(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, false, true);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim external released.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimExternalReleasedTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);

                    lockReference.ExitWriteLock();

                    TestHelper.ValidateLockStatus(lockReference, false, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim int32.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimInt32Test()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                const Int32 Timeout = 123;

                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockWriter(lockReference, Timeout))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim manual dispose.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimManualDisposeTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (LockWriter writer = new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);

                    using (LockWriter nestedWriter = new LockWriter(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, false, true, false);

                        nestedWriter.Dispose();

                        TestHelper.ValidateLockStatus(lockReference, false, true, false);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, true, false);
                    writer.Dispose();

                    TestHelper.ValidateLockStatus(lockReference, false, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim multiple dispose.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimMultipleDisposeTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (LockWriter writer = new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);

                    writer.Dispose();
                    writer.Dispose();

                    TestHelper.ValidateLockStatus(lockReference, false, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim nested no recursion.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimNestedNoRecursionTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);

                    try
                    {
                        using (new LockWriter(lockReference))
                        {
                        }

                        Assert.Fail("LockRecursionException was expected");
                    }
                    catch (LockRecursionException ex)
                    {
                        Debug.WriteLine(ex);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim nested.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimNestedTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);

                    using (new LockWriter(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, false, true, false);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim null reader writer lock slim.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LockWriterReaderWriterLockSlimNullReaderWriterLockSlimTest()
        {
            const ReaderWriterLockSlim LockReference = null;

            using (new LockWriter(LockReference))
            {
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim simple.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimSimpleTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim timeout.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimTimeoutTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                Thread writeLockThread = new Thread(LockWriteReaderWriterLockSlimThreadHandler)
                                         {
                                             Name = "LockWriterReaderWriterLockSlimTimeoutTest",
                                             IsBackground = true
                                         };

                writeLockThread.Start(lockReference);

                Thread.Sleep(50);

                try
                {
                    using (new LockWriter(lockReference, 10))
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
        /// Runs test for lock writer reader writer lock slim upgrade from reader nested writer.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimUpgradeFromReaderNestedWriterTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference, true))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, false, true);

                    using (new LockWriter(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, false, true, true);

                        using (new LockWriter(lockReference))
                        {
                            TestHelper.ValidateLockStatus(lockReference, false, true, true);
                        }

                        TestHelper.ValidateLockStatus(lockReference, false, true, true);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim upgrade from reader no recursion.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimUpgradeFromReaderNoRecursionTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference, true))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, false, true);

                    using (new LockWriter(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, false, true, true);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim upgrade from reader not upgradeable no recursion.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimUpgradeFromReaderNotUpgradeableNoRecursionTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim())
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    try
                    {
                        using (new LockWriter(lockReference))
                        {
                        }

                        Assert.Fail("LockRecursionException was expected");
                    }
                    catch (LockRecursionException ex)
                    {
                        Debug.WriteLine(ex);
                    }

                    TestHelper.ValidateLockStatus(lockReference, true, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim upgrade from reader not upgradeable.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimUpgradeFromReaderNotUpgradeableTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, true, false, false);

                    try
                    {
                        using (new LockWriter(lockReference))
                        {
                        }

                        Assert.Fail("LockRecursionException was expected");
                    }
                    catch (LockRecursionException ex)
                    {
                        Debug.WriteLine(ex);
                    }

                    TestHelper.ValidateLockStatus(lockReference, true, false, false);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock slim upgrade from reader.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockSlimUpgradeFromReaderTest()
        {
            using (ReaderWriterLockSlim lockReference = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                TestHelper.ValidateLockStatus(lockReference, false, false, false);

                using (new LockReader(lockReference, true))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, false, true);

                    using (new LockWriter(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, false, true, true);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, false, false, false);
            }
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock timeout.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockTimeoutTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            Thread writeLockThread = new Thread(LockWriteReaderWriterLockThreadHandler)
                                     {
                                         Name = "LockWriterReaderWriterLockTimeoutTest",
                                         IsBackground = true
                                     };

            writeLockThread.Start(lockReference);

            Thread.Sleep(50);

            try
            {
                using (new LockWriter(lockReference, 10))
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

        /// <summary>
        /// Runs test for lock writer reader writer lock upgrade from reader nested writer.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockUpgradeFromReaderNestedWriterTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockReader(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, true, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true);

                    using (new LockWriter(lockReference))
                    {
                        TestHelper.ValidateLockStatus(lockReference, false, true);
                    }

                    TestHelper.ValidateLockStatus(lockReference, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, true, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock upgrade from reader.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockUpgradeFromReaderTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            TestHelper.ValidateLockStatus(lockReference, false, false);

            using (new LockReader(lockReference))
            {
                TestHelper.ValidateLockStatus(lockReference, true, false);

                using (new LockWriter(lockReference))
                {
                    TestHelper.ValidateLockStatus(lockReference, false, true);
                }

                TestHelper.ValidateLockStatus(lockReference, true, false);
            }

            TestHelper.ValidateLockStatus(lockReference, false, false);
        }

        /// <summary>
        /// Runs test for lock writer reader writer lock upgrade timeout.
        /// </summary>
        [TestMethod]
        public void LockWriterReaderWriterLockUpgradeTimeoutTest()
        {
            ReaderWriterLock lockReference = new ReaderWriterLock();

            using (new LockReader(lockReference))
            {
                Thread writeLockThread = new Thread(LockWriteReaderWriterLockThreadHandler)
                                         {
                                             Name = "LockWriterReaderWriterLockTimeoutTest",
                                             IsBackground = true
                                         };

                writeLockThread.Start(lockReference);

                Thread.Sleep(50);

                try
                {
                    using (new LockWriter(lockReference, 10))
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
        }

        #region Static Helper Methods

        /// <summary>
        /// Writes the lock reader writer lock slim thread handler.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private static void LockWriteReaderWriterLockSlimThreadHandler(Object state)
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
        private static void LockWriteReaderWriterLockThreadHandler(Object state)
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