namespace Neovolve.Toolkit.Server.UnitTests.Unity
{
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Server.Unity;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityControllerFactoryTests"/>
    ///   class is used to test the <see cref="UnityControllerFactory"/> class.
    /// </summary>
    [TestClass]
    public class UnityControllerFactoryTests
    {
        /// <summary>
        /// Runs test for unity container factory release controller tears down instance using container.
        /// </summary>
        [TestMethod]
        public void UnityContainerFactoryReleaseControllerTearsDownInstanceUsingContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            UnityControllerFactory target = new UnityControllerFactory(container);
            IController instance = MockRepository.GenerateStub<IController>();

            target.ReleaseController(instance);

            container.AssertWasCalled(x => x.Teardown(instance));
        }

        /// <summary>
        /// Runs test for unity controller factory can be created with container.
        /// </summary>
        [TestMethod]
        public void UnityControllerFactoryCanBeCreatedWithContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            new UnityControllerFactory(container);
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