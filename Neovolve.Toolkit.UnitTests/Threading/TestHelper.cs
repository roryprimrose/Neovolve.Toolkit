namespace Neovolve.Toolkit.UnitTests.Threading
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="TestHelper"/>
    ///   class is used to provide common logic for testing.
    /// </summary>
    internal static class TestHelper
    {
        /// <summary>
        /// Validations the lock status.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="holdsRead">
        /// If set to <c>true</c> [holds read].
        /// </param>
        /// <param name="holdsWrite">
        /// If set to <c>true</c> [holds write].
        /// </param>
        /// <param name="holdsUpgradable">
        /// If set to <c>true</c> [holds upgradable].
        /// </param>
        public static void ValidateLockStatus(ReaderWriterLockSlim lockReference, Boolean holdsRead, Boolean holdsWrite, Boolean holdsUpgradable)
        {
            Assert.AreEqual(holdsRead, lockReference.IsReadLockHeld, "IsReaderLockHeld returned an incorrect value");
            Assert.AreEqual(holdsWrite, lockReference.IsWriteLockHeld, "IsWriterLockHeld returned an incorrect value");
            Assert.AreEqual(holdsUpgradable, lockReference.IsUpgradeableReadLockHeld, "IsUpgradeableReadLockHeld returned an incorrect value");
        }

        /// <summary>
        /// Validations the lock status.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="holdsRead">
        /// If set to <c>true</c> [holds read].
        /// </param>
        /// <param name="holdsWrite">
        /// If set to <c>true</c> [holds write].
        /// </param>
        public static void ValidateLockStatus(ReaderWriterLock lockReference, Boolean holdsRead, Boolean holdsWrite)
        {
            Assert.AreEqual(holdsRead, lockReference.IsReaderLockHeld, "IsReaderLockHeld returned an incorrect value");
            Assert.AreEqual(holdsWrite, lockReference.IsWriterLockHeld, "IsWriterLockHeld returned an incorrect value");
        }
    }
}