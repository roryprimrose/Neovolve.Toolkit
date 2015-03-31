namespace Neovolve.Toolkit.Unity.LoadTests
{
    using System;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="DisposableStrategyExtensionTests"/>
    ///   class is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    [TestClass]
    public class DisposableStrategyExtensionTests
    {
        /// <summary>
        ///   Stores the _container for the test run.
        /// </summary>
        private static IUnityContainer _container;

        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running the unit tests in a class.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            _container.Dispose();
            _container = null;
        }

        /// <summary>
        /// Initializes the class for running unit tests.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _container = new UnityContainer();

            DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension();

            _container.AddExtension(disposableStrategyExtension);
            _container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
            _container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
            _container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
            _container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());
        }

        #endregion

        /// <summary>
        /// Runs test for disposable strategy extension can create tree that is not disposed.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionCanCreateTreeThatIsNotDisposedTest()
        {
            using (IFirstDisposable actual = _container.Resolve<IFirstDisposable>())
            {
                Assert.IsNotNull(actual, "No instance was created");
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes all instances in fragmented tree.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllInstancesInFragmentedTreeTest()
        {
            IFirstDisposable actual = _container.Resolve<IFirstDisposable>();
            ISecondDisposable secondDisposable = actual.Second;

            actual.Second = null;

            GC.Collect();

            _container.Teardown(actual);

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
            Assert.IsTrue(secondDisposable.IsDisposed, "First child was not disposed when it should have been");
            Assert.IsTrue(secondDisposable.Third.IsDisposed, "The dependency was not disposed when it should have been");
            Assert.IsTrue(secondDisposable.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose resolved child instance if parent is still in lifetime manager.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllInstancesInTreeTest()
        {
            IFirstDisposable actual = _container.Resolve<IFirstDisposable>();

            _container.Teardown(actual);

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.IsDisposed, "First child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.IsDisposed, "The dependency was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes all instances in tree when parent is disposed.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllInstancesInTreeWhenParentIsDisposedTest()
        {
            IFirstDisposable actual = _container.Resolve<IFirstDisposable>();

            actual.Dispose();

            _container.Teardown(actual);

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.IsDisposed, "First child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.IsDisposed, "The dependency was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes built up child instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesBuiltUpChildInstanceTest()
        {
            using (ThirdDisposable actual = new ThirdDisposable())
            {
                _container.BuildUp(actual);

                _container.Teardown(actual);

                Assert.IsFalse(actual.IsDisposed, "Root was disposed when it should not have been");
                Assert.IsTrue(actual.Fourth.IsDisposed, "The dependency was not disposed");
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes resolved child instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesResolvedChildInstanceTest()
        {
            IThirdDisposable actual = _container.Resolve<IThirdDisposable>();

            _container.Teardown(actual);

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.Fourth.IsDisposed, "The dependency was not disposed");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes resolved single instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesResolvedSingleInstanceTest()
        {
            IFourthDisposable actual = _container.Resolve<IFourthDisposable>();

            _container.Teardown(actual);

            Assert.IsTrue(actual.IsDisposed, "TearDown failed to dispose the instance");
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose built up root instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeBuiltUpRootInstanceTest()
        {
            using (IThirdDisposable actual = new ThirdDisposable())
            {
                _container.BuildUp(actual);

                _container.Teardown(actual);

                Assert.IsFalse(actual.IsDisposed, "TearDown failed to dispose the instance");
            }
        }
    }
}