namespace Neovolve.Toolkit.Server.UnitTests.Unity
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Server.Unity;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityServiceHostFactoryTests"/>
    ///   class is used to test the <see cref="UnityServiceHostFactory"/> class.
    /// </summary>
    [TestClass]
    public class UnityServiceHostFactoryTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            ConfigurationStoreFactory.StoreType = null;
        }

        #endregion

        /// <summary>
        /// Runs test for unity service host factory can be created from store with default container.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCanBeCreatedFromStoreWithDefaultContainerTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            UnityServiceHostFactory target = new UnityServiceHostFactory(store, String.Empty, String.Empty);

            Assert.IsNotNull(target.Container, "Container returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service host factory can be created from store with named config.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCanBeCreatedFromStoreWithNamedConfigTest()
        {
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            UnityServiceHostFactory target = new UnityServiceHostFactory(store, SectionName, String.Empty);

            Assert.IsNotNull(target.Container, "Container returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service host factory can be created from store with named container.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCanBeCreatedFromStoreWithNamedContainerTest()
        {
            const String ContainerName = "NamedContainer";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(ContainerName);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            UnityServiceHostFactory target = new UnityServiceHostFactory(store, String.Empty, ContainerName);

            Assert.IsNotNull(target.Container, "Container returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service host factory can be created from store with named section and named container.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCanBeCreatedFromStoreWithNamedSectionAndNamedContainerTest()
        {
            const String ContainerName = "NamedContainer";
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(ContainerName);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            UnityServiceHostFactory target = new UnityServiceHostFactory(store, SectionName, ContainerName);

            Assert.IsNotNull(target.Container, "Container returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service host factory can be created with default constructor.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCanBeCreatedWithDefaultConstructorTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            new UnityServiceHostFactory();
        }

        /// <summary>
        /// Runs test for unity service host factory can be created with unity container.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCanBeCreatedWithUnityContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            UnityServiceHostFactory target = new UnityServiceHostFactory(container);

            Assert.AreSame(container, target.Container, "Container returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service host factory create service host returns unity service host.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostFactoryCreateServiceHostReturnsUnityServiceHostTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            DerivedUnityServiceHostFactory target = new DerivedUnityServiceHostFactory(container);

            Uri[] addresses = new[]
                              {
                                  new Uri("http://localhost/toolkit"), 
                              };
            ServiceHost actual = target.CreateHost(typeof(TestService), addresses);

            Assert.IsInstanceOfType(actual, typeof(UnityServiceHost), "CreateServiceHost returned an instance with an incorrect type");
        }

        /// <summary>
        /// Runs test for unity service host factory throws exception when created with null container.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityServiceHostFactoryThrowsExceptionWhenCreatedWithNullContainerTest()
        {
            new UnityServiceHostFactory(null);
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