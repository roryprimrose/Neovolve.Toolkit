namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.Configuration;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="DomainContainerTests"/>
    ///   class is used to test the <see cref="DomainContainer"/> class.
    /// </summary>
    [TestClass]
    public class DomainContainerTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            DomainContainer.Destroy();
        }

        #endregion

        /// <summary>
        /// Runs test for domain container current returns container only once.
        /// </summary>
        [TestMethod]
        public void DomainContainerCurrentReturnsContainerOnlyOnceTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer firstContainer = DomainContainer.Current;
            IUnityContainer secondContainer = DomainContainer.Current;
            IUnityContainer thirdContainer = DomainContainer.Current;
            IUnityContainer fourthContainer = DomainContainer.Current;
            IUnityContainer fifthContainer = DomainContainer.Current;

            Assert.IsNotNull(firstContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(secondContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(thirdContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(fourthContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(fifthContainer, "Resolve failed to return an instance");
            Assert.AreSame(firstContainer, secondContainer, "Different containers where returned");
            Assert.AreSame(secondContainer, thirdContainer, "Different containers where returned");
            Assert.AreSame(thirdContainer, fourthContainer, "Different containers where returned");
            Assert.AreSame(fourthContainer, fifthContainer, "Different containers where returned");
        }

        /// <summary>
        /// Runs test for domain container current throws exception when container is not resolved.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void DomainContainerCurrentThrowsExceptionWhenContainerIsNotResolvedTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(null);

            IUnityContainer unityContainer = DomainContainer.Current;

            Assert.Fail("Container was resolved when it should not have been - " + unityContainer);
        }

        /// <summary>
        /// Runs test for domain container destroy can be executed multiple times.
        /// </summary>
        [TestMethod]
        public void DomainContainerDestroyCanBeExecutedMultipleTimesTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer firstContainer = DomainContainer.Current;

            DomainContainer.Destroy();
            DomainContainer.Destroy();
            DomainContainer.Destroy();

            Assert.IsNotNull(firstContainer, "Resolve failed to return an instance");
        }

        /// <summary>
        /// Runs test for domain container destroy current causes current to build new instance.
        /// </summary>
        [TestMethod]
        public void DomainContainerDestroyCausesCurrentToBuildNewInstanceTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer firstContainer = DomainContainer.Current;
            IUnityContainer secondContainer = DomainContainer.Current;

            DomainContainer.Destroy();

            IUnityContainer thirdContainer = DomainContainer.Current;
            IUnityContainer fourthContainer = DomainContainer.Current;
            IUnityContainer fifthContainer = DomainContainer.Current;

            Assert.IsNotNull(firstContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(secondContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(thirdContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(fourthContainer, "Resolve failed to return an instance");
            Assert.IsNotNull(fifthContainer, "Resolve failed to return an instance");
            Assert.AreSame(firstContainer, secondContainer, "Different containers where returned");
            Assert.AreNotSame(secondContainer, thirdContainer, "Different containers where returned");
            Assert.AreSame(thirdContainer, fourthContainer, "Different containers where returned");
            Assert.AreSame(fourthContainer, fifthContainer, "Different containers where returned");
        }

        /// <summary>
        /// Runs test for domain container try get current caches failed resolution outcome.
        /// </summary>
        [TestMethod]
        public void DomainContainerTryGetCurrentCachesFailedResolutionOutcomeTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(null).Repeat.Once();

            IUnityContainer optionalContainer;

            DomainContainer.TryGetCurrent(out optionalContainer);
            DomainContainer.TryGetCurrent(out optionalContainer);
            Boolean outcome = DomainContainer.TryGetCurrent(out optionalContainer);

            Assert.IsFalse(outcome, "TryGetCurrent returned an incorrect value");
            Assert.IsNull(optionalContainer, "TryGetCurrent failed to return container instance");
        }

        /// <summary>
        /// Runs test for domain container try get current returns false and null container when container is not available.
        /// </summary>
        [TestMethod]
        public void DomainContainerTryGetCurrentReturnsFalseAndNullContainerWhenContainerIsNotAvailableTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(null);

            IUnityContainer optionalContainer;

            Boolean outcome = DomainContainer.TryGetCurrent(out optionalContainer);

            Assert.IsFalse(outcome, "TryGetCurrent returned an incorrect value");
            Assert.IsNull(optionalContainer, "TryGetCurrent failed to return container instance");
        }

        /// <summary>
        /// Runs test for domain container try get current returns previously resolved container.
        /// </summary>
        [TestMethod]
        public void DomainContainerTryGetCurrentReturnsPreviouslyResolvedContainerTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Once();

            IUnityContainer optionalContainer;

            // Get configuration above should only be consulted once
            DomainContainer.TryGetCurrent(out optionalContainer);
            DomainContainer.TryGetCurrent(out optionalContainer);
            DomainContainer.TryGetCurrent(out optionalContainer);
        }

        /// <summary>
        /// Runs test for domain container try get current returns releases resolved container when destroyed.
        /// </summary>
        [TestMethod]
        public void DomainContainerTryGetCurrentReturnsReleasesResolvedContainerWhenDestroyedTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Twice();

            IUnityContainer optionalContainer;

            // Get configuration above should only be consulted twice
            DomainContainer.TryGetCurrent(out optionalContainer);
            DomainContainer.TryGetCurrent(out optionalContainer);
            DomainContainer.Destroy();
            DomainContainer.TryGetCurrent(out optionalContainer);
            DomainContainer.TryGetCurrent(out optionalContainer);
        }

        /// <summary>
        /// Runs test for domain container try get current returns resolved container.
        /// </summary>
        [TestMethod]
        public void DomainContainerTryGetCurrentReturnsResolvedContainerTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            IUnityContainer optionalContainer;

            Boolean outcome = DomainContainer.TryGetCurrent(out optionalContainer);

            Assert.IsTrue(outcome, "TryGetCurrent returned an incorrect value");
            Assert.IsNotNull(optionalContainer, "TryGetCurrent failed to return container instance");

            IUnityContainer currentContainer = DomainContainer.Current;

            Assert.AreSame(optionalContainer, currentContainer, "Different resolved containers where returned");
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