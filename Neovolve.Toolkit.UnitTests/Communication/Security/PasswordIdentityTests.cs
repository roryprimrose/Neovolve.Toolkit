namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// This is a test class for PasswordIdentityTests and is intended
    ///   to contain all PasswordIdentityTests Unit Tests.
    /// </summary>
    [TestClass]
    public class PasswordIdentityTests
    {
        /// <summary>
        /// Runs test for can create with password and is authenticated.
        /// </summary>
        [TestMethod]
        public void CanCreateWithPasswordAndIsAuthenticatedTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            String authenticationType = Guid.NewGuid().ToString();
            const Boolean IsAuthenticated = true;
            PasswordIdentity target = new PasswordIdentity(userName, password, IsAuthenticated, authenticationType);

            Assert.AreEqual(userName, target.Name, "Name returned an incorrect value");
            Assert.AreEqual(password.ConvertToString(), target.Password.ConvertToString(), "Password returned an incorrect value");
            Assert.AreEqual(IsAuthenticated, target.IsAuthenticated, "IsAuthenticated returned an incorrect value");
            Assert.AreEqual(authenticationType, target.AuthenticationType, "AuthenticationType returned an incorrect value");
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