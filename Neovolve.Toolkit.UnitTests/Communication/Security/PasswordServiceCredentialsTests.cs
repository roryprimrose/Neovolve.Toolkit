namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System.IdentityModel.Selectors;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="PasswordServiceCredentialsTests"/>
    ///   class is used to test the <see cref="PasswordServiceCredentials"/> class.
    /// </summary>
    [TestClass]
    public class PasswordServiceCredentialsTests
    {
        /// <summary>
        /// Runs test for can create password service credentials.
        /// </summary>
        [TestMethod]
        public void CanCreatePasswordServiceCredentialsTest()
        {
            new PasswordServiceCredentials();
        }

        /// <summary>
        /// Runs test for clone returns correct type.
        /// </summary>
        [TestMethod]
        public void CloneReturnsCorrectTypeTest()
        {
            PasswordServiceCredentials target = new PasswordServiceCredentials();

            ServiceCredentials actual = target.Clone();

            Assert.IsInstanceOfType(actual, typeof(PasswordServiceCredentials), "Clone returned an instance with an incorrect type");
        }

        /// <summary>
        /// Runs test for create security token manager returns default manager when not using custom validator.
        /// </summary>
        [TestMethod]
        public void CreateSecurityTokenManagerReturnsDefaultManagerWhenNotUsingCustomValidatorTest()
        {
            PasswordServiceCredentials target = new PasswordServiceCredentials();

            target.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Windows;

            SecurityTokenManager actual = target.CreateSecurityTokenManager();

            Assert.IsNotInstanceOfType(actual, typeof(PasswordSecurityTokenManager), "Incorrect SecurityTokenManager type returned");
        }

        /// <summary>
        /// Runs test for create security token manager returns password security token manager when using custom validator.
        /// </summary>
        [TestMethod]
        public void CreateSecurityTokenManagerReturnsPasswordSecurityTokenManagerWhenUsingCustomValidatorTest()
        {
            PasswordServiceCredentials target = new PasswordServiceCredentials();

            target.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;

            SecurityTokenManager actual = target.CreateSecurityTokenManager();

            Assert.IsInstanceOfType(actual, typeof(PasswordSecurityTokenManager), "Incorrect SecurityTokenManager type returned");
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