namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.IdentityModel.Tokens;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="DefaultPasswordValidatorTests"/>
    ///   class is used to test the <see cref="DefaultPasswordValidator"/> class.
    /// </summary>
    [TestClass]
    public class DefaultPasswordValidatorTests
    {
        /// <summary>
        /// Runs test for validate passes with empty password.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SecurityTokenException))]
        public void ValidatePassesWithEmptyPasswordTest()
        {
            DefaultPasswordValidator target = new DefaultPasswordValidator();
            String userName = Guid.NewGuid().ToString();

            target.Validate(userName, String.Empty);
        }

        /// <summary>
        /// Runs test for validate passes with user name and password.
        /// </summary>
        [TestMethod]
        public void ValidatePassesWithUserNameAndPasswordTest()
        {
            DefaultPasswordValidator target = new DefaultPasswordValidator();
            String userName = Guid.NewGuid().ToString();
            String password = Guid.NewGuid().ToString();

            target.Validate(userName, password);
        }

        /// <summary>
        /// Runs test for validate with empty user name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SecurityTokenException))]
        public void ValidateWithEmptyUserNameThrowsExceptionTest()
        {
            DefaultPasswordValidator target = new DefaultPasswordValidator();
            String password = Guid.NewGuid().ToString();

            target.Validate(String.Empty, password);
        }

        /// <summary>
        /// Runs test for validate with null password throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SecurityTokenException))]
        public void ValidateWithNullPasswordThrowsExceptionTest()
        {
            DefaultPasswordValidator target = new DefaultPasswordValidator();
            String userName = Guid.NewGuid().ToString();

            target.Validate(userName, null);
        }

        /// <summary>
        /// Runs test for validate with null user name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SecurityTokenException))]
        public void ValidateWithNullUserNameThrowsExceptionTest()
        {
            DefaultPasswordValidator target = new DefaultPasswordValidator();
            String password = Guid.NewGuid().ToString();

            target.Validate(null, password);
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