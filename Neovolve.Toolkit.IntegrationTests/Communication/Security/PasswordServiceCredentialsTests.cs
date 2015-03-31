namespace Neovolve.Toolkit.IntegrationTests.Communication.Security
{
    using System;
    using System.Diagnostics;
    using System.IdentityModel.Selectors;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="PasswordServiceCredentialsTests"/>
    ///   class contains unit tests for the 
    ///   <see cref="PasswordServiceCredentials"/> implementation.
    /// </summary>
    [TestClass]
    public class PasswordServiceCredentialsTests
    {
        /// <summary>
        ///   Stores the certificate manager.
        /// </summary>
        private static readonly CertificateManager _certificateManager = new CertificateManager();

        #region Setup/Teardown

        /// <summary>
        /// Cleans up the class.
        /// </summary>
        [ClassCleanup]
        public static void CleanupClass()
        {
            _certificateManager.UninstallCertificates();
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _certificateManager.Certificates.Add(
                new CertificateDetails("Neovolve.Toolkit.pfx", "CN=Neovolve.Toolkit", "Neovolve.Toolkit", StoreName.My, StoreLocation.CurrentUser));

            _certificateManager.UninstallCertificates();
            _certificateManager.InstallCertificates();
        }

        #endregion

        /// <summary>
        /// Passwords the security on thread test.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"Communication\Security\Neovolve.Toolkit.pfx")]
        [ExpectedException(typeof(MessageSecurityException))]
        public void PasswordSecurityDefaultPasswordValidatorNoPasswordTest()
        {
            RunPasswordServiceCredentialsTest(new DefaultPasswordValidator(), PrincipalPermissionMode.Custom, String.Empty);
        }

        /// <summary>
        /// Runs test for password security default password validator no password windows validation.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"Communication\Security\Neovolve.Toolkit.pfx")]
        public void PasswordSecurityDefaultPasswordValidatorNoPasswordWindowsValidationTest()
        {
            RunPasswordServiceCredentialsTest(
                new DefaultPasswordValidator(), PrincipalPermissionMode.Custom, String.Empty, UserNamePasswordValidationMode.Windows);
        }

        /// <summary>
        /// Passwords the security on thread test.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"Communication\Security\Neovolve.Toolkit.pfx")]
        public void PasswordSecurityEmptyPasswordValidatorNoPasswordTest()
        {
            RunPasswordServiceCredentialsTest(new OptionalPasswordValidator(), PrincipalPermissionMode.Custom, String.Empty);
        }

        /// <summary>
        /// Passwords the security on thread test.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"Communication\Security\Neovolve.Toolkit.pfx")]
        public void PasswordSecurityIdentityNotOnThreadTest()
        {
            RunPasswordServiceCredentialsTest(new DefaultPasswordValidator(), PrincipalPermissionMode.None, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Passwords the security on thread test.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"Communication\Security\Neovolve.Toolkit.pfx")]
        public void PasswordSecurityIdentityOnThreadTest()
        {
            RunPasswordServiceCredentialsTest(new DefaultPasswordValidator(), PrincipalPermissionMode.Custom, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Passwords the security on thread test.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"Communication\Security\Neovolve.Toolkit.pfx")]
        public void PasswordSecurityNullValidatorTest()
        {
            RunPasswordServiceCredentialsTest(null, PrincipalPermissionMode.Custom, Guid.NewGuid().ToString());
        }

        #region Static Helper Methods

        /// <summary>
        /// Creates the service host.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="validator">
        /// The validator.
        /// </param>
        /// <param name="validationMode">
        /// The validation mode.
        /// </param>
        /// <param name="permissionMode">
        /// The permission mode.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceHost"/> instance.
        /// </returns>
        private static ServiceHost CreateServiceHost(
            Binding binding, 
            UserNamePasswordValidator validator, 
            UserNamePasswordValidationMode validationMode, 
            PrincipalPermissionMode permissionMode, 
            Uri address)
        {
            ServiceHost host = new ServiceHost(typeof(PasswordService));

            // Add debug support to the host
            ServiceDebugBehavior debugBehaviour = new ServiceDebugBehavior
                                                  {
                                                      IncludeExceptionDetailInFaults = true
                                                  };

            host.Description.Behaviors.Remove<ServiceDebugBehavior>();
            host.Description.Behaviors.Add(debugBehaviour);

            // Create the service credentials
            PasswordServiceCredentials credentials = new PasswordServiceCredentials();

            credentials.UserNameAuthentication.CustomUserNamePasswordValidator = validator;
            credentials.UserNameAuthentication.UserNamePasswordValidationMode = validationMode;

            // Assign the certificate from the CurrentUser store using the subject name of the certificate
            credentials.ServiceCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "Neovolve.Toolkit");

            host.Description.Behaviors.Remove<ServiceCredentials>();
            host.Description.Behaviors.Add(credentials);

            ServiceAuthorizationBehavior authorization = new ServiceAuthorizationBehavior
                                                         {
                                                             PrincipalPermissionMode = permissionMode
                                                         };

            host.Description.Behaviors.Remove<ServiceAuthorizationBehavior>();
            host.Description.Behaviors.Add(authorization);

            host.AddServiceEndpoint(typeof(IPasswordService), binding, address);

            return host;
        }

        /// <summary>
        /// Runs the password service credentials test.
        /// </summary>
        /// <param name="validator">
        /// The validator.
        /// </param>
        /// <param name="permissionMode">
        /// The permission mode.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        private static void RunPasswordServiceCredentialsTest(
            UserNamePasswordValidator validator, PrincipalPermissionMode permissionMode, String password)
        {
            RunPasswordServiceCredentialsTest(validator, permissionMode, password, UserNamePasswordValidationMode.Custom);
        }

        /// <summary>
        /// Runs the password service credentials test.
        /// </summary>
        /// <param name="validator">
        /// The validator.
        /// </param>
        /// <param name="permissionMode">
        /// The permission mode.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="validationMode">
        /// The validation mode.
        /// </param>
        private static void RunPasswordServiceCredentialsTest(
            UserNamePasswordValidator validator, 
            PrincipalPermissionMode permissionMode, 
            String password, 
            UserNamePasswordValidationMode validationMode)
        {
            const String ServiceAddress = "net.tcp://localhost:8081/PasswordServiceCredentialsTests";
            Uri address = new Uri(ServiceAddress);

            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            using (ServiceHost host = CreateServiceHost(binding, validator, validationMode, permissionMode, address))
            {
                try
                {
                    host.Open();

                    // We need to identify that the identity of the certificate is valid for the address of the service
                    EndpointAddress endpointAddress = new EndpointAddress(address, EndpointIdentity.CreateDnsIdentity("Neovolve.Toolkit"));

                    using (ChannelFactory<IPasswordService> factory = new ChannelFactory<IPasswordService>(binding, endpointAddress))
                    {
                        try
                        {
                            // We need to ignore certificate validation errors
                            // Because the certificate was created with makecert, there is no chain of trust on this certificate
                            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

                            String userName = Guid.NewGuid().ToString();

                            factory.Credentials.UserName.UserName = userName;
                            factory.Credentials.UserName.Password = password;

                            factory.Open();

                            IPasswordService channel = factory.CreateChannel();

                            if (validationMode == UserNamePasswordValidationMode.Custom)
                            {
                                Boolean isCorrectThreadIdentity;

                                try
                                {
                                    isCorrectThreadIdentity = channel.GetIsCorrectThreadIdentity();
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);

                                    throw;
                                }

                                String expectedThreadUserName = channel.GetThreadUserName();
                                String expectedThreadPassword = channel.GetThreadPassword();

                                if (permissionMode == PrincipalPermissionMode.Custom)
                                {
                                    Assert.IsTrue(isCorrectThreadIdentity, "GetIsCorrectThreadIdentity returned an incorrect value.");
                                    Assert.AreEqual(expectedThreadUserName, userName, "GetThreadUserName returned an incorrect value.");
                                    Assert.AreEqual(expectedThreadPassword, password, "GetThreadPassword returned an incorrect value.");
                                }
                                else
                                {
                                    Assert.IsFalse(isCorrectThreadIdentity, "GetIsCorrectThreadIdentity returned an incorrect value.");
                                    Assert.AreNotEqual(expectedThreadUserName, userName, "GetThreadUserName returned an incorrect value.");
                                    Assert.AreNotEqual(expectedThreadPassword, password, "GetThreadPassword returned an incorrect value.");
                                }

                                Assert.IsTrue(channel.GetIsCorrectContextIdentity(), "GetIsCorrectContextIdentity returned an incorrect value.");

                                String expectedContextUserName = channel.GetContextUserName();
                                Assert.AreEqual(expectedContextUserName, userName, "GetContextUserName returned an incorrect value.");

                                String expectedContextPassword = channel.GetContextPassword();
                                Assert.AreEqual(expectedContextPassword, password, "GetContextPassword returned an incorrect value.");
                            }

                            factory.Close();
                        }
                        finally
                        {
                            if (factory.State != CommunicationState.Faulted)
                            {
                                try
                                {
                                    factory.Close();
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (host.State != CommunicationState.Faulted)
                    {
                        try
                        {
                            host.Close();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
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