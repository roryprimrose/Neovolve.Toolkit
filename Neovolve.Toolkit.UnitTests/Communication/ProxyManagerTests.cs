namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ProxyManagerTests"/>
    ///   class is used to test the <see cref="ProxyManager{T}"/> class.
    /// </summary>
    [TestClass]
    public class ProxyManagerTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            CacheStoreFactory.StoreType = null;
            ConfigurationStoreFactory.StoreType = null;
        }

        #endregion

        /// <summary>
        /// Runs test for proxy manager can be created with provided proxy handler.
        /// </summary>
        [TestMethod]
        public void ProxyManagerCanBeCreatedWithProvidedProxyHandlerTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            ProxyStub compareStub = new ProxyStub();
            ProxyHandler<ProxyStub> proxyHandler = new DefaultProxyHandler<ProxyStub>();
            const String InputValue = "This has been";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);

            String expected = compareStub.DoSomething(InputValue);

            using (ProxyManager<ProxyStub> manager = new ProxyManager<ProxyStub>(proxyHandler))
            {
                String actual = manager.Proxy.DoSomething(InputValue);

                Assert.AreEqual(expected, actual, "The proxy manager incorrectly handled the provided proxy handler");
            }
        }

        /// <summary>
        /// Runs test for proxy manager can be disposed multiple times.
        /// </summary>
        [TestMethod]
        public void ProxyManagerCanBeDisposedMultipleTimesTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            IConfigurationStore configurationStore = MockRepository.GenerateStub<IConfigurationStore>();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configurationStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            ProxyManager<ProxyStub> manager = new ProxyManager<ProxyStub>();

            manager.Dispose();
            manager.Dispose();
        }

        /// <summary>
        /// Runs test for proxy manager cannot be used once disposed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ProxyManagerCannotBeUsedOnceDisposedTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            IConfigurationStore configurationStore = MockRepository.GenerateStub<IConfigurationStore>();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configurationStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            const String InputValue = "This has been";

            ProxyManager<ProxyStub> manager = new ProxyManager<ProxyStub>();

            manager.Dispose();

            manager.Proxy.DoSomething(InputValue);
        }

        /// <summary>
        /// Runs test for proxy manager disposes proxy handler when disposed.
        /// </summary>
        [TestMethod]
        public void ProxyManagerDisposesProxyHandlerWhenDisposedTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            MockRepository mock = new MockRepository();
            DefaultProxyHandler<ProxyStub> handler =
                mock.PartialMultiMock(typeof(DefaultProxyHandler<ProxyStub>), typeof(IDisposable)) as DefaultProxyHandler<ProxyStub>;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);

            using (mock.Record())
            {
                ((IDisposable)handler).Dispose();
            }

            using (mock.Playback())
            {
                using (new ProxyManager<ProxyStub>(handler))
                {
                }
            }
        }

        /// <summary>
        /// Runs test for proxy manager uses channel proxy handler for service contract.
        /// </summary>
        [TestMethod]
        public void ProxyManagerUsesChannelProxyHandlerForServiceContractTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            IConfigurationStore configurationStore = MockRepository.GenerateStub<IConfigurationStore>();
            EndpointAddress endpointAddress = new EndpointAddress("http://localhost/toolkit");
            ChannelEndpointElement endpoint = new ChannelEndpointElement(endpointAddress, typeof(ITestService).FullName);
            ClientSection section = new ClientSection();

            section.Endpoints.Add(endpoint);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configurationStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            configurationStore.Stub(x => x.GetSection<ClientSection>("system.serviceModel/client")).Return(section);

            using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>())
            {
                Assert.IsInstanceOfType(
                    manager.ProxyHandler, typeof(ChannelProxyHandler<ITestService>), "ProxyHandler returned an incorrect instance type");
            }
        }

        /// <summary>
        /// Runs test for proxy manager uses default handler when no handler is provided.
        /// </summary>
        [TestMethod]
        public void ProxyManagerUsesDefaultHandlerWhenNoHandlerIsProvidedTest()
        {
            ProxyStub compareStub = new ProxyStub();
            const String InputValue = "This has been";
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            IConfigurationStore configurationStore = MockRepository.GenerateStub<IConfigurationStore>();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configurationStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            String expected = compareStub.DoSomething(InputValue);

            using (ProxyManager<ProxyStub> manager = new ProxyManager<ProxyStub>())
            {
                String actual = manager.Proxy.DoSomething(InputValue);

                Assert.AreEqual(expected, actual, "The proxy manager incorrectly handled the provided proxy handler");
            }
        }

        /// <summary>
        /// Runs test for proxy manager uses proxy handler resolved from configuration.
        /// </summary>
        [TestMethod]
        public void ProxyManagerUsesProxyHandlerResolvedFromConfigurationTest()
        {
            IConfigurationStore configurationStore = MockRepository.GenerateStub<IConfigurationStore>();
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();

            ConfigurationStoreMockWrapper.MockInstance = configurationStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);

            configurationStore.Stub(x => x.GetApplicationSetting<String>(typeof(ProxyHandler<ProxyStub>).AssemblyQualifiedName)).Return(
                typeof(DefaultProxyHandler<ProxyStub>).AssemblyQualifiedName);

            try
            {
                ProxyStub compareStub = new ProxyStub();
                const String InputValue = "This has been";

                String expected = compareStub.DoSomething(InputValue);

                using (ProxyManager<ProxyStub> manager = new ProxyManager<ProxyStub>())
                {
                    String actual = manager.Proxy.DoSomething(InputValue);

                    Assert.AreEqual(expected, actual, "The proxy manager incorrectly handled the provided proxy handler");
                }
            }
            finally
            {
                ConfigurationStoreFactory.StoreType = null;
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