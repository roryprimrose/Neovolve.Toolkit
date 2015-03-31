namespace Neovolve.Toolkit.Server.UnitTests.Unity
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Server.Unity;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityControllerFactoryHttpModuleTests"/>
    ///   class is used to test the <see cref="UnityControllerFactoryHttpModule"/> class.
    /// </summary>
    [TestClass]
    public class UnityControllerFactoryHttpModuleTests
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
        /// Runs test for unity controller factory HTTP module dispose restores default controller factory.
        /// </summary>
        [TestMethod]
        public void UnityControllerFactoryHttpModuleDisposeRestoresDefaultControllerFactoryTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();
            ContainerElement container = new ContainerElement();
            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            section.Containers.Add(container);
            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Once();

            HttpApplication context = MockRepository.GenerateStub<HttpApplication>();
            UnityControllerFactoryHttpModule target = new UnityControllerFactoryHttpModule();

            target.Init(context);
            target.Dispose();

            DefaultControllerFactory factory = ControllerBuilder.Current.GetControllerFactory() as DefaultControllerFactory;

            Assert.IsNotNull(factory, "GetControllerFactory returned an incorrect factory type");
            Assert.IsNotInstanceOfType(factory, typeof(UnityControllerFactoryHttpModule), "Dispose failed to restore default controller factory");
        }

        /// <summary>
        /// Runs test for unity controller factory HTTP module init assigns container to controller factory.
        /// </summary>
        [TestMethod]
        public void UnityControllerFactoryHttpModuleInitAssignsContainerToControllerFactoryTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();
            ContainerElement container = new ContainerElement();
            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            section.Containers.Add(container);
            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Once();

            HttpApplication context = MockRepository.GenerateStub<HttpApplication>();
            UnityControllerFactoryHttpModule target = new UnityControllerFactoryHttpModule();

            target.Init(context);

            Assert.IsNotNull(UnityHttpModuleBase.Container, "Container failed to return an instance");
        }

        /// <summary>
        /// Runs test for unity controller factory HTTP module init assigns unity factory to controller factory.
        /// </summary>
        [TestMethod]
        public void UnityControllerFactoryHttpModuleInitAssignsUnityFactoryToControllerFactoryTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();
            ContainerElement container = new ContainerElement();
            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            section.Containers.Add(container);
            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Once();

            HttpApplication context = MockRepository.GenerateStub<HttpApplication>();
            UnityControllerFactoryHttpModule target = new UnityControllerFactoryHttpModule();

            target.Init(context);

            UnityControllerFactory factory = ControllerBuilder.Current.GetControllerFactory() as UnityControllerFactory;

            Assert.IsNotNull(factory, "GetControllerFactory failed to return the correct factory type");
        }

        /// <summary>
        /// Runs test for unity controller factory HTTP module init called multiple times only assigns unity factory to controller factory once.
        /// </summary>
        [TestMethod]
        public void UnityControllerFactoryHttpModuleInitCalledMultipleTimesOnlyAssignsUnityFactoryToControllerFactoryOnceTest()
        {
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection();
            ContainerElement container = new ContainerElement();
            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            section.Containers.Add(container);
            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section).Repeat.Once();

            HttpApplication context = MockRepository.GenerateStub<HttpApplication>();
            UnityControllerFactoryHttpModule target = new UnityControllerFactoryHttpModule();

            target.Init(context);

            HttpApplication secondContext = MockRepository.GenerateStub<HttpApplication>();
            UnityControllerFactoryHttpModule secondTarget = new UnityControllerFactoryHttpModule();

            secondTarget.Init(secondContext);

            UnityControllerFactory firstFactory = ControllerBuilder.Current.GetControllerFactory() as UnityControllerFactory;
            UnityControllerFactory secondFactory = ControllerBuilder.Current.GetControllerFactory() as UnityControllerFactory;

            Assert.AreSame(firstFactory, secondFactory, "GetControllerFactory failed to return the same factory instance");
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