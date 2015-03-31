namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="PasswordPrincipalTests"/>
    ///   class is used to test the <see cref="PasswordPrincipal"/> class.
    /// </summary>
    [TestClass]
    public class PasswordPrincipalTests
    {
        /// <summary>
        /// Runs test for can create.
        /// </summary>
        [TestMethod]
        public void CanCreateTest()
        {
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordIdentity identity = new PasswordIdentity(Guid.NewGuid().ToString(), password, true, Guid.NewGuid().ToString());
            PasswordPrincipal target = new PasswordPrincipal(identity, null);

            Assert.AreSame(identity, target.Identity, "Identity returned an incorrect value");
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