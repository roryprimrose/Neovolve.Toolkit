namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
    using System.ServiceModel.Security.Tokens;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="PasswordSecurityTokenManagerTests"/>
    ///   class is used to test the <see cref="PasswordSecurityTokenManager"/> class.
    /// </summary>
    [TestClass]
    public class PasswordSecurityTokenManagerTests
    {
        /// <summary>
        /// Runs test for can create with service credentials.
        /// </summary>
        [TestMethod]
        public void CanCreateWithServiceCredentialsTest()
        {
            PasswordServiceCredentials credentials = new PasswordServiceCredentials();
            PasswordSecurityTokenManager target = new PasswordSecurityTokenManager(credentials);

            Assert.AreSame(credentials, target.ServiceCredentials, "ServiceCredentials returned an incorrect value");
        }

        /// <summary>
        /// Runs test for create security token authenticator creates new validator if none is found.
        /// </summary>
        [TestMethod]
        public void CreateSecurityTokenAuthenticatorCreatesNewValidatorIfNoneIsFoundTest()
        {
            PasswordServiceCredentials credentials = new PasswordServiceCredentials();
            PasswordSecurityTokenManager target = new PasswordSecurityTokenManager(credentials);
            SecurityTokenResolver outOfBandTokenResolver;
            SecurityTokenRequirement requirement = new RecipientServiceModelSecurityTokenRequirement
                                                   {
                                                       TokenType = SecurityTokenTypes.UserName
                                                   };

            Assert.IsNull(credentials.UserNameAuthentication.CustomUserNamePasswordValidator, "No validator instance should be assigned");

            PasswordSecurityTokenAuthenticator actual =
                target.CreateSecurityTokenAuthenticator(requirement, out outOfBandTokenResolver) as PasswordSecurityTokenAuthenticator;

            Assert.IsNotNull(actual, "No authenticator or an authenticator of an incorrect type was returned");

            UserNameSecurityToken token = new UserNameSecurityToken(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            // This is the best way to test that the correct validator type has been assigned given that there are no ways to directly validate the correct type
            try
            {
                actual.ValidateToken(token);
            }
            catch (SecurityTokenException)
            {
                Assert.Fail("If a DefaultPasswordValidator was used, this call should not have failed");
            }

            token = new UserNameSecurityToken(Guid.NewGuid().ToString(), null);

            try
            {
                actual.ValidateToken(token);
            }
            catch (SecurityTokenException)
            {
                // This was the expected behaviour if the D
            }
        }

        /// <summary>
        /// Runs test for create security token authenticator returns correct authenticator for user name token requirement.
        /// </summary>
        [TestMethod]
        public void CreateSecurityTokenAuthenticatorReturnsCorrectAuthenticatorForUserNameTokenRequirementTest()
        {
            PasswordServiceCredentials credentials = new PasswordServiceCredentials();
            PasswordSecurityTokenManager target = new PasswordSecurityTokenManager(credentials);
            SecurityTokenResolver outOfBandTokenResolver;
            SecurityTokenRequirement requirement = new RecipientServiceModelSecurityTokenRequirement
                                                   {
                                                       TokenType = SecurityTokenTypes.UserName
                                                   };

            SecurityTokenAuthenticator actual = target.CreateSecurityTokenAuthenticator(requirement, out outOfBandTokenResolver);

            Assert.IsInstanceOfType(actual, typeof(PasswordSecurityTokenAuthenticator), "Incorrect authenticator type was returned");
        }

        /// <summary>
        /// Runs test for create security token authenticator returns default authenticator for non user name token requirement.
        /// </summary>
        [TestMethod]
        public void CreateSecurityTokenAuthenticatorReturnsDefaultAuthenticatorForNonUserNameTokenRequirementTest()
        {
            PasswordServiceCredentials credentials = new PasswordServiceCredentials();
            PasswordSecurityTokenManager target = new PasswordSecurityTokenManager(credentials);
            SecurityTokenResolver outOfBandTokenResolver;
            SecurityTokenRequirement requirement = new InitiatorServiceModelSecurityTokenRequirement
                                                   {
                                                       TokenType = SecurityTokenTypes.X509Certificate, 
                                                       KeyUsage = SecurityKeyUsage.Exchange
                                                   };

            SecurityTokenAuthenticator actual = target.CreateSecurityTokenAuthenticator(requirement, out outOfBandTokenResolver);

            Assert.IsNotInstanceOfType(actual, typeof(PasswordSecurityTokenAuthenticator), "Incorrect authenticator type was returned");
        }

        /// <summary>
        /// Runs test for create security token authenticator throws exception with null security token requirement.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSecurityTokenAuthenticatorThrowsExceptionWithNullSecurityTokenRequirementTest()
        {
            PasswordServiceCredentials credentials = new PasswordServiceCredentials();
            PasswordSecurityTokenManager target = new PasswordSecurityTokenManager(credentials);
            SecurityTokenResolver outOfBandTokenResolver;

            target.CreateSecurityTokenAuthenticator(null, out outOfBandTokenResolver);
        }

        /// <summary>
        /// Runs test for create security token authenticator uses existing validator.
        /// </summary>
        [TestMethod]
        public void CreateSecurityTokenAuthenticatorUsesExistingValidatorTest()
        {
            MockRepository mock = new MockRepository();
            UserNamePasswordValidator validator = mock.StrictMock<UserNamePasswordValidator>();
            UserNameSecurityToken token = new UserNameSecurityToken(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            using (mock.Record())
            {
                validator.Validate(token.UserName, token.Password);
            }

            PasswordServiceCredentials credentials = new PasswordServiceCredentials();
            PasswordSecurityTokenManager target = new PasswordSecurityTokenManager(credentials);
            SecurityTokenResolver outOfBandTokenResolver;
            SecurityTokenRequirement requirement = new RecipientServiceModelSecurityTokenRequirement();

            credentials.UserNameAuthentication.CustomUserNamePasswordValidator = validator;
            requirement.TokenType = SecurityTokenTypes.UserName;

            PasswordSecurityTokenAuthenticator actual =
                target.CreateSecurityTokenAuthenticator(requirement, out outOfBandTokenResolver) as PasswordSecurityTokenAuthenticator;

            Assert.IsNotNull(actual, "No authenticator or an authenticator of an incorrect type was returned");

            using (mock.Playback())
            {
                actual.ValidateToken(token);
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