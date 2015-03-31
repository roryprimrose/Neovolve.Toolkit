namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;
    using Neovolve.Toolkit.IntegrationTests.Communication.Security;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ChannelProxyHandlerTests"/>
    ///   class is used to test the <see cref="ChannelProxyHandler{T}"/> class.
    /// </summary>
    [TestClass]
    public class ChannelProxyHandlerTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            ICacheStore cacheStore = CacheStoreFactory.Create();

            cacheStore.Clear();
        }

        #endregion

        /// <summary>
        /// Runs test for channel proxy handler can call A service after first call fails.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerCanCallAServiceAfterFirstCallFailsTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    try
                    {
                        channel.DoSomething();
                    }
                    catch (CommunicationException)
                    {
                        // Ignore this exception
                    }

                    host.Open();

                    channel.DoSomething();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler can call service when previous call faulted the channel.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerCanCallServiceWhenPreviousCallFaultedTheChannelTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    try
                    {
                        channel.ThrowException();
                    }
                    catch (CommunicationException)
                    {
                        // Ignore this exception
                    }

                    channel.DoSomething();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler can destroy faulted channel.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerCanDestroyFaultedChannelTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    try
                    {
                        channel.ThrowException();
                    }
                    catch (CommunicationException)
                    {
                        // Ignore this exception
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler creates endpoint from configuration name.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerCreatesEndpointFromConfigurationNameTest()
        {
            const String ServiceAddress = "net.pipe://localhost/NamedChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(PasswordService)))
            {
                host.AddServiceEndpoint(typeof(IPasswordService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<IPasswordService> handler = new ChannelProxyHandler<IPasswordService>("NamedService"))
                {
                    IPasswordService channel = (IPasswordService)handler.GetTransparentProxy();

                    channel.GetThreadPassword();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler creates endpoint from default configuration.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerCreatesEndpointFromDefaultConfigurationTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    channel.DoSomething();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler creates endpoint from default configuration when named configuration exists.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerCreatesEndpointFromDefaultConfigurationWhenNamedConfigurationExistsTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            ClientSection section = new ClientSection();
            EndpointAddress firstAddress = new EndpointAddress("net.pipe://localhost/ChannelProxyHandlerTestsFirst");
            String contractType = typeof(ITestService).FullName;
            ChannelEndpointElement firstEndpoint = new ChannelEndpointElement(firstAddress, contractType)
                                                   {
                                                       Name = "FirstName"
                                                   };
            EndpointAddress secondAddress = new EndpointAddress(ServiceAddress);
            ChannelEndpointElement secondEndpoint = new ChannelEndpointElement(secondAddress, contractType);

            section.Endpoints.Add(firstEndpoint);
            section.Endpoints.Add(secondEndpoint);

            store.Stub(x => x.GetSection<ClientSection>("system.serviceModel/client")).Return(section);

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    channel.DoSomething();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler handles closing the channel when service is no longer available.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerHandlesClosingTheChannelWhenServiceIsNoLongerAvailableTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    channel.DoSomething();

                    host.Close();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler handles timeout exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void ChannelProxyHandlerHandlesTimeoutExceptionTest()
        {
            const String ServiceAddress = "net.pipe://localhost/NamedChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(PasswordService)))
            {
                host.AddServiceEndpoint(typeof(IPasswordService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<IPasswordService> handler = new ChannelProxyHandler<IPasswordService>("NamedService"))
                {
                    handler.Initialize((ChannelFactory factory) => factory.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(100));

                    IPasswordService channel = (IPasswordService)handler.GetTransparentProxy();

                    channel.Sleep(2000);
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler ignores unsupported initialize action.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerIgnoresUnsupportedInitializeActionTest()
        {
            const String ServiceAddress = "net.pipe://localhost/NamedChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(PasswordService)))
            {
                host.AddServiceEndpoint(typeof(IPasswordService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<IPasswordService> handler = new ChannelProxyHandler<IPasswordService>("NamedService"))
                {
                    IPasswordService channel = (IPasswordService)handler.GetTransparentProxy();

                    channel.GetThreadPassword();

                    Boolean initializeInvoked = false;

                    handler.Initialize<Boolean>(actionValue => initializeInvoked = true);

                    channel.GetThreadPassword();

                    Assert.IsFalse(initializeInvoked, "Initialize was invoked when it should not have been");
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler initializes factory after factory is already created.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerInitializesFactoryAfterFactoryIsAlreadyCreatedTest()
        {
            const String ServiceAddress = "net.pipe://localhost/NamedChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(PasswordService)))
            {
                host.AddServiceEndpoint(typeof(IPasswordService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<IPasswordService> handler = new ChannelProxyHandler<IPasswordService>("NamedService"))
                {
                    IPasswordService channel = (IPasswordService)handler.GetTransparentProxy();

                    channel.GetThreadPassword();

                    Boolean initializeInvoked = false;

                    handler.Initialize<ChannelFactory<IPasswordService>>(
                        factory =>
                        {
                            factory.Credentials.UserName.UserName = "Test";
                            initializeInvoked = true;
                        });

                    channel.GetThreadPassword();

                    Assert.IsTrue(initializeInvoked, "Initialize was not invoked");
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler initializes factory before factory is already created.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerInitializesFactoryBeforeFactoryIsAlreadyCreatedTest()
        {
            const String ServiceAddress = "net.pipe://localhost/NamedChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(PasswordService)))
            {
                host.AddServiceEndpoint(typeof(IPasswordService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<IPasswordService> handler = new ChannelProxyHandler<IPasswordService>("NamedService"))
                {
                    IPasswordService channel = (IPasswordService)handler.GetTransparentProxy();

                    Boolean initializeInvoked = false;

                    handler.Initialize<ChannelFactory>(
                        factory =>
                        {
                            factory.Credentials.UserName.UserName = "Test";
                            initializeInvoked = true;
                        });

                    channel.GetThreadPassword();

                    Assert.IsTrue(initializeInvoked, "Initialize was not invoked");
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler resolves default configuration name with no endpoints configured.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerResolvesDefaultConfigurationNameWithNoEndpointsConfiguredTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            store.Stub(x => x.GetSection<ClientSection>("system.serviceModel/client")).Return(null);

            using (new ChannelProxyHandler<IPasswordService>(store))
            {
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler resolves endpoint from type.
        /// </summary>
        [TestMethod]
        public void ChannelProxyHandlerResolvesEndpointFromTypeTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ChannelProxyHandlerTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);
                host.Open();

                using (ChannelProxyHandler<ITestService> handler = new ChannelProxyHandler<ITestService>())
                {
                    ITestService channel = (ITestService)handler.GetTransparentProxy();

                    channel.DoSomething();
                }
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler throws exception for non configured service.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ChannelProxyHandlerThrowsExceptionForNonConfiguredServiceTest()
        {
            using (new ChannelProxyHandler<IUnknownService>())
            {
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler throws exception with empty configuration name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChannelProxyHandlerThrowsExceptionWithEmptyConfigurationNameTest()
        {
            using (new ChannelProxyHandler<IUnknownService>(String.Empty))
            {
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler throws exception with multiple endpoint names found and no name specified.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ChannelProxyHandlerThrowsExceptionWithMultipleEndpointNamesFoundAndNoNameSpecifiedTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            ClientSection section = new ClientSection();

            EndpointAddress firstAddress = new EndpointAddress("net.pipe://localhost/ChannelProxyHandlerTestsFirst");
            String contractType = typeof(ITestService).FullName;
            ChannelEndpointElement firstEndpoint = new ChannelEndpointElement(firstAddress, contractType)
                                                   {
                                                       Name = "FirstName"
                                                   };
            EndpointAddress secondAddress = new EndpointAddress("net.pipe://localhost/ChannelProxyHandlerTestsSecond");
            ChannelEndpointElement secondEndpoint = new ChannelEndpointElement(secondAddress, contractType)
                                                    {
                                                        Name = "SecondName"
                                                    };
            section.Endpoints.Add(firstEndpoint);
            section.Endpoints.Add(secondEndpoint);

            store.Stub(x => x.GetSection<ClientSection>("system.serviceModel/client")).Return(section);

            using (new ChannelProxyHandler<ITestService>(store))
            {
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler throws exception with null configuration name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChannelProxyHandlerThrowsExceptionWithNullConfigurationNameTest()
        {
            using (new ChannelProxyHandler<IUnknownService>((String)null))
            {
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler throws exception with null configuration store.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChannelProxyHandlerThrowsExceptionWithNullConfigurationStoreTest()
        {
            using (new ChannelProxyHandler<IUnknownService>((IConfigurationStore)null))
            {
            }
        }

        /// <summary>
        /// Runs test for channel proxy handler throws exception with white space configuration name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChannelProxyHandlerThrowsExceptionWithWhiteSpaceConfigurationNameTest()
        {
            using (new ChannelProxyHandler<IUnknownService>(" "))
            {
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