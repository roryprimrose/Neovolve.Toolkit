namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.Configuration;
    using System.IO;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityContainerResolverTests"/>
    ///   class is used to test the <see cref="UnityContainerResolver"/> class.
    /// </summary>
    [TestClass]
    public class UnityContainerResolverTests
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
        /// Runs test for unity container resolver resolve can resolve container with default values.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverResolveCanResolveContainerWithDefaultValuesTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container = UnityContainerResolver.Resolve();

            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver resolve can resolve default container from named config.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverResolveCanResolveDefaultContainerFromNamedConfigTest()
        {
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            IUnityContainer container = UnityContainerResolver.Resolve(store, SectionName, String.Empty);

            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver resolve can resolve default container.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverResolveCanResolveDefaultContainerTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container = UnityContainerResolver.Resolve(store, String.Empty, String.Empty);

            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver resolve can resolve named container from named config.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverResolveCanResolveNamedContainerFromNamedConfigTest()
        {
            const String ContainerName = "NamedContainer";
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(ContainerName);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            IUnityContainer container = UnityContainerResolver.Resolve(store, SectionName, ContainerName);

            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver resolve can resolve named container.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverResolveCanResolveNamedContainerTest()
        {
            const String ContainerName = "NamedContainer";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(ContainerName);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container = UnityContainerResolver.Resolve(store, String.Empty, ContainerName);

            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver resolve throws exception when default container is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void UnityContainerResolverResolveThrowsExceptionWhenDefaultContainerIsNotFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection("NamedContainer");

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            UnityContainerResolver.Resolve(store, String.Empty, String.Empty);
        }

        /// <summary>
        /// Runs test for unity container resolver resolve throws exception when named container is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void UnityContainerResolverResolveThrowsExceptionWhenNamedContainerIsNotFoundTest()
        {
            const String ContainerName = "NamedContainer";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            UnityContainerResolver.Resolve(store, String.Empty, ContainerName);
        }

        /// <summary>
        /// Runs test for unity container resolver resolve throws exception when no containers found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void UnityContainerResolverResolveThrowsExceptionWhenNoContainersFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            UnityContainerResolver.Resolve(store, String.Empty, String.Empty);
        }

        /// <summary>
        /// Runs test for unity container resolver resolve throws exception when no unity config found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void UnityContainerResolverResolveThrowsExceptionWhenNoUnityConfigFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            UnityContainerResolver.Resolve(store, String.Empty, String.Empty);
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve can resolve container with default values.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveCanResolveContainerWithDefaultValuesTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container);

            Assert.IsTrue(actual, "TryResolve returned an incorrect value");
            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve can resolve default container from named config.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveCanResolveDefaultContainerFromNamedConfigTest()
        {
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, SectionName, String.Empty);

            Assert.IsTrue(actual, "TryResolve returned an incorrect value");
            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve can resolve default container.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveCanResolveDefaultContainerTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, String.Empty, String.Empty);

            Assert.IsTrue(actual, "TryResolve returned an incorrect value");
            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve can resolve named container from named config.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveCanResolveNamedContainerFromNamedConfigTest()
        {
            const String ContainerName = "NamedContainer";
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(ContainerName);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, SectionName, ContainerName);

            Assert.IsTrue(actual, "TryResolve returned an incorrect value");
            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns default container from named config.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveReturnsDefaultContainerFromNamedConfigTest()
        {
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, SectionName, String.Empty);

            Assert.IsTrue(actual, "TryResolve returned an incorrect value");
            Assert.IsNotNull(container, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns false and null container when configuration assembly is not found with named configuration and named container.
        /// </summary>
        [TestMethod]
        public void
            UnityContainerResolverTryResolveReturnsFalseAndNullContainerWhenConfigurationAssemblyIsNotFoundWithNamedConfigurationAndNamedContainerTest
            ()
        {
            const String ContainerName = "NamedContainer";
            const String SectionName = "NamedSection";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Throw(new IOException());

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, SectionName, ContainerName);

            Assert.IsNull(container, "Container returned an instance when null was expected");
            Assert.IsFalse(actual, "TryResolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns false and null container when default container is not found.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveReturnsFalseAndNullContainerWhenDefaultContainerIsNotFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection("NamedContainer");

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, String.Empty, String.Empty);

            Assert.IsFalse(actual, "TryResolve returned an incorrect value");
            Assert.IsNull(container, "TryResolve returned a container when it should not have");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns false and null container when named container is not found.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveReturnsFalseAndNullContainerWhenNamedContainerIsNotFoundTest()
        {
            const String ContainerName = "NamedContainer";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, String.Empty, ContainerName);

            Assert.IsFalse(actual, "TryResolve returned an incorrect value");
            Assert.IsNull(container, "TryResolve returned a container when it should not have");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns false and null container when no containers found.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveReturnsFalseAndNullContainerWhenNoContainersFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, String.Empty, String.Empty);

            Assert.IsFalse(actual, "TryResolve returned an incorrect value");
            Assert.IsNull(container, "TryResolve returned a container when it should not have");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns false and null container when no unity config found.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveReturnsFalseAndNullContainerWhenNoUnityConfigFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, String.Empty, String.Empty);

            Assert.IsFalse(actual, "TryResolve returned an incorrect value");
            Assert.IsNull(container, "TryResolve returned a container when it should not have");
        }

        /// <summary>
        /// Runs test for unity container resolver try resolve returns false when configuration assembly is not found.
        /// </summary>
        [TestMethod]
        public void UnityContainerResolverTryResolveReturnsFalseWhenConfigurationAssemblyIsNotFoundTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Throw(new IOException());

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container);

            Assert.IsNull(container, "Container returned an instance when null was expected");
            Assert.IsFalse(actual, "TryResolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity container try resolve can resolve named container.
        /// </summary>
        [TestMethod]
        public void UnityContainerTryResolveCanResolveNamedContainerTest()
        {
            const String ContainerName = "NamedContainer";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(ContainerName);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer container;

            Boolean actual = UnityContainerResolver.TryResolve(out container, store, String.Empty, ContainerName);

            Assert.IsTrue(actual, "TryResolve returned an incorrect value");
            Assert.IsNotNull(container, "Resolve failed to return an instance");
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