namespace Neovolve.Toolkit.Server.UnitTests.Unity
{
    using System.Web;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Server.Unity;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;
    using Rhino.Mocks.Interfaces;

    /// <summary>
    /// The <see cref="UnityHttpModuleBaseTests"/>
    ///   class is used to test the <see cref="UnityHttpModule"/> class.
    /// </summary>
    [TestClass]
    public class UnityHttpModuleBaseTests
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
        /// Runs test for unity HTTP module base can dispose with null container.
        /// </summary>
        [TestMethod]
        public void UnityHttpModuleBaseCanDisposeWithNullContainerTest()
        {
            UnityHttpModuleBase target = MockRepository.GenerateStub<UnityHttpModuleBase>();

            target.Dispose();
        }

        /// <summary>
        /// Runs test for unity HTTP module base can manually assign container.
        /// </summary>
        [TestMethod]
        public void UnityHttpModuleBaseCanManuallyAssignContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            IUnityContainer secondContainer = MockRepository.GenerateStub<IUnityContainer>();

            Assert.AreNotSame(container, secondContainer, "Guard clause in the test has failed");

            UnityHttpModuleBase.Container = container;
            UnityHttpModuleBase.Container = secondContainer;

            Assert.AreSame(secondContainer, UnityHttpModuleBase.Container, "Container returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity HTTP module base disposes container when disposed.
        /// </summary>
        [TestMethod]
        public void UnityHttpModuleBaseDisposesContainerWhenDisposedTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            UnityHttpModuleBase target = MockRepository.GenerateStub<UnityHttpModuleBase>();

            target.Stub(x => x.Dispose()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            UnityHttpModuleBase.Container = container;

            target.Dispose();

            container.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for unity HTTP module base ignores disposed container when disposed.
        /// </summary>
        [TestMethod]
        public void UnityHttpModuleBaseIgnoresDisposedContainerWhenDisposedTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            UnityHttpModuleBase target = MockRepository.GenerateStub<UnityHttpModuleBase>();
            UnityHttpModuleBase secondTarget = MockRepository.GenerateStub<UnityHttpModuleBase>();

            Assert.AreNotSame(target, secondTarget, "Targets refer to the same instance");

            target.Stub(x => x.Dispose()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            UnityHttpModuleBase.Container = container;

            target.Dispose();
            secondTarget.Dispose();

            container.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for unity HTTP module base resolves container on init only once.
        /// </summary>
        [TestMethod]
        public void UnityHttpModuleBaseResolvesContainerOnInitOnlyOnceTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();
            ContainerElement container = new ContainerElement();
            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            section.Containers.Add(container);
            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Once();

            using (HttpApplication firstApplication = new HttpApplication())
            {
                UnityHttpModuleBase firstModule = MockRepository.GenerateStub<UnityHttpModuleBase>();
                firstModule.Init(firstApplication);
            }

            using (HttpApplication secondApplication = new HttpApplication())
            {
                UnityHttpModuleBase secondModule = MockRepository.GenerateStub<UnityHttpModuleBase>();

                secondModule.Init(secondApplication);
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