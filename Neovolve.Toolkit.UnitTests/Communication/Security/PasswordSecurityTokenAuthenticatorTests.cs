namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.Collections.ObjectModel;
    using System.IdentityModel.Policy;
    using System.IdentityModel.Tokens;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="PasswordSecurityTokenAuthenticatorTests"/>
    ///   class is used to test the <see cref="PasswordSecurityTokenAuthenticator"/> class.
    /// </summary>
    [TestClass]
    public class PasswordSecurityTokenAuthenticatorTests
    {
        /// <summary>
        /// Runs test for can create with validator.
        /// </summary>
        [TestMethod]
        public void CanCreateWithValidatorTest()
        {
            DefaultPasswordValidator validator = new DefaultPasswordValidator();
            new PasswordSecurityTokenAuthenticator(validator);
        }

        /// <summary>
        /// Runs test for can validate user name password security token.
        /// </summary>
        [TestMethod]
        public void CanValidateUserNamePasswordSecurityTokenTest()
        {
            UserNameSecurityToken token = new UserNameSecurityToken(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            DefaultPasswordValidator validator = new DefaultPasswordValidator();
            PasswordSecurityTokenAuthenticator target = new PasswordSecurityTokenAuthenticator(validator);

            ReadOnlyCollection<IAuthorizationPolicy> policies = target.ValidateToken(token);

            Assert.IsNotNull(policies, "ValidateToken failed to return an instance");
            Assert.IsFalse(policies.Count == 0, "Policy collection is empty");

            Boolean policyFound = false;

            for (Int32 index = 0; index < policies.Count; index++)
            {
                IAuthorizationPolicy policy = policies[index];

                if (policy is PasswordAuthorizationPolicy)
                {
                    policyFound = true;
                }
            }

            Assert.IsTrue(policyFound, "No PasswordAuthorizationPolicy was returned");
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