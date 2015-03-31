namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Claims;
    using System.IdentityModel.Policy;
    using System.Security;
    using System.Security.Principal;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="PasswordAuthorizationPolicyTests"/>
    ///   class is used to test the <see cref="PasswordAuthorizationPolicy"/> class.
    /// </summary>
    [TestClass]
    public class PasswordAuthorizationPolicyTests
    {
        /// <summary>
        /// Runs test for can create with user name and password.
        /// </summary>
        [TestMethod]
        public void CanCreateWithUserNameAndPasswordTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();

            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            Assert.AreEqual(ClaimSet.System, target.Issuer, "Issuer returned an incorrect value");
            Assert.IsFalse(String.IsNullOrEmpty(target.Id), "Id failed to return a value");
        }

        /// <summary>
        /// Runs test for evaluate null context returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateNullContextReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            Object state = null;

            Boolean actual = target.Evaluate(null, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with empty properties returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithEmptyPropertiesReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            Object state = null;

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with empty user name returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithEmptyUserNameReturnsFalseTest()
        {
            String userName = String.Empty;
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            List<IIdentity> identities = new List<IIdentity>();
            Object state = null;

            // Configure the context
            identities.Add(identity);
            evaluationContext.Properties.Add("Identities", identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with expected valid input returns true.
        /// </summary>
        [TestMethod]
        public void EvaluateWithExpectedValidInputReturnsTrueTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            GenericIdentity identity = new GenericIdentity(userName);
            List<IIdentity> identities = new List<IIdentity>();
            Object state = null;
            const Boolean Expected = true;

            // Configure the context
            identities.Add(identity);
            evaluationContext.Properties.Add("Identities", identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.AreEqual(Expected, actual, "Evaluate returned an incorrect value");
            Assert.AreEqual(1, identities.Count, "Incorrect number of identities found");

            PasswordIdentity newIdentity = identities[0] as PasswordIdentity;

            Assert.IsNotNull(newIdentity, "Failed to populate list with the new PasswordIdentity");
            Assert.AreEqual(userName, newIdentity.Name, "Name returned an incorrect value");
            Assert.AreEqual(password, newIdentity.Password, "Password returned an incorrect value");
            Assert.AreEqual(identity.IsAuthenticated, newIdentity.IsAuthenticated, "IsAuthenticated returned an incorrect value");
            Assert.AreEqual(identity.AuthenticationType, newIdentity.AuthenticationType, "AuthenticationType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with incorrect identity type returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithIncorrectIdentityTypeReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            List<IIdentity> identities = new List<IIdentity>();
            Object state = null;

            // Configure the context
            identities.Add(identity);
            evaluationContext.Properties.Add("Identities", identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with invalid identities type returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithInvalidIdentitiesTypeReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            Object state = null;

            // Configure the context
            evaluationContext.Properties.Add("Identities", Guid.NewGuid().ToString());

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with no identities returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithNoIdentitiesReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            Object state = null;

            // Configure the context
            evaluationContext.Properties.Add("SomeKey", Guid.NewGuid().ToString());

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with no identity match returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithNoIdentityMatchReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            GenericIdentity identity = new GenericIdentity(Guid.NewGuid().ToString());
            List<IIdentity> identities = new List<IIdentity>();
            Object state = null;

            // Configure the context
            identities.Add(identity);
            evaluationContext.Properties.Add("Identities", identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with null identities returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithNullIdentitiesReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            const List<IIdentity> Identities = null;
            Object state = null;

            // Configure the context
            evaluationContext.Properties.Add("Identities", Identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with null properties returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithNullPropertiesReturnsFalseTest()
        {
            String userName = Guid.NewGuid().ToString();
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub(false);
            Object state = null;

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with null user name returns false.
        /// </summary>
        [TestMethod]
        public void EvaluateWithNullUserNameReturnsFalseTest()
        {
            String userName = null;
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(userName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            List<IIdentity> identities = new List<IIdentity>();
            Object state = null;

            // Configure the context
            identities.Add(identity);
            evaluationContext.Properties.Add("Identities", identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsFalse(actual, "Evaluate returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate with white space user name returns true.
        /// </summary>
        [TestMethod]
        public void EvaluateWithWhiteSpaceUserNameReturnsTrueTest()
        {
            const String UserName = "  ";
            SecureString password = Guid.NewGuid().ToString().ConvertToSecureString();
            PasswordAuthorizationPolicy target = new PasswordAuthorizationPolicy(UserName, password);

            EvaluationContext evaluationContext = new EvaluationContextStub();
            IIdentity identity = new GenericIdentity(UserName);
            List<IIdentity> identities = new List<IIdentity>();
            Object state = null;

            // Configure the context
            identities.Add(identity);
            evaluationContext.Properties.Add("Identities", identities);

            Boolean actual = target.Evaluate(evaluationContext, ref state);

            Assert.IsTrue(actual, "Evaluate returned an incorrect value");
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