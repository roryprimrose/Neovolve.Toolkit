namespace Neovolve.Toolkit.Unity.IntegrationTests
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
        /// Runs test for disposable strategy extension dispose child of instance in lifetime manager when extension is disposed.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposeChildOfInstanceInLifetimeManagerWhenExtensionIsDisposedTest()
        {
            IFirstDisposable actual;

            using (ContainerControlledLifetimeManager containerControlledLifetimeManager = new ContainerControlledLifetimeManager())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    using (UnityContainer container = new UnityContainer())
                    {
                        container.AddExtension(disposableStrategyExtension);
                        container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                        container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), containerControlledLifetimeManager);
                        container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                        container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                        actual = container.Resolve<IFirstDisposable>();

                        container.Teardown(actual);

                        Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
                        Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
                        Assert.IsTrue(
                            actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
                        Assert.IsFalse(actual.Second.IsDisposed, "First child was disposed when it should not have been");
                        Assert.IsFalse(actual.Second.Third.IsDisposed, "The dependency was disposed when it should not have been");
                        Assert.IsFalse(actual.Second.Third.Fourth.IsDisposed, "The dependency was disposed when it should not have been");
                    }

                    // At this point the container should have already disposed the extension and the lifetime manager
                    Assert.IsTrue(actual.Second.IsDisposed, "First child was not disposed when it should have been");
                    Assert.IsTrue(actual.Second.Third.IsDisposed, "The dependency was not disposed when it should have been");
                    Assert.IsTrue(actual.Second.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes all instances in fragmented tree.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllInstancesInFragmentedTreeTest()
        {
            IFirstDisposable actual;
            ISecondDisposable secondDisposable;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<IFirstDisposable>();
                    secondDisposable = actual.Second;

                    actual.Second = null;

                    GC.Collect();

                    container.Teardown(actual);
                }
            }

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
            IFirstDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<IFirstDisposable>();

                    container.Teardown(actual);
                }
            }

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.IsDisposed, "First child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.IsDisposed, "The dependency was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes all instances in tree when child is not unique.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllInstancesInTreeWhenChildIsNotUniqueTest()
        {
            IFirstDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new PerResolveLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<IFirstDisposable>();

                    actual.Dispose();

                    container.Teardown(actual);
                }
            }

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
            IFirstDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<IFirstDisposable>();

                    actual.Dispose();

                    container.Teardown(actual);
                }
            }

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.IsDisposed, "First child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.IsDisposed, "The dependency was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes all instances in tree without root type mapping.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllInstancesInTreeWithoutRootTypeMappingTest()
        {
            IFirstDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(FirstDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<FirstDisposable>();

                    container.Teardown(actual);
                }
            }

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
            Assert.IsTrue(actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.IsDisposed, "First child was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.IsDisposed, "The dependency was not disposed when it should have been");
            Assert.IsTrue(actual.Second.Third.Fourth.IsDisposed, "The dependency was not disposed when it should have been");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes all trees when disposed.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesAllTreesWhenDisposedTest()
        {
            IFirstDisposable firstDisposable;
            ISecondDisposable secondDisposable;
            IThirdDisposable thirdDisposable;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    firstDisposable = container.Resolve<IFirstDisposable>();
                    secondDisposable = container.Resolve<ISecondDisposable>();
                    thirdDisposable = container.Resolve<IThirdDisposable>();
                }
            }

            Assert.IsTrue(firstDisposable.IsDisposed, "First resolved instance was not disposed");
            Assert.IsTrue(secondDisposable.IsDisposed, "Second resolved instance was not disposed");
            Assert.IsTrue(thirdDisposable.IsDisposed, "Third resolved instance was not disposed");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes built up child instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesBuiltUpChildInstanceTest()
        {
            ThirdDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = new ThirdDisposable();

                    container.BuildUp(actual);

                    container.Teardown(actual);
                }
            }

            Assert.IsFalse(actual.IsDisposed, "Root was disposed when it should not have been");
            Assert.IsTrue(actual.Fourth.IsDisposed, "The dependency was not disposed");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes resolved child instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesResolvedChildInstanceTest()
        {
            IThirdDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<IThirdDisposable>();

                    container.Teardown(actual);
                }
            }

            Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
            Assert.IsTrue(actual.Fourth.IsDisposed, "The dependency was not disposed");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes resolved single instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesResolvedSingleInstanceTest()
        {
            IFourthDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<IFourthDisposable>();

                    container.Teardown(actual);
                }
            }

            Assert.IsTrue(actual.IsDisposed, "TearDown failed to dispose the instance");
        }

        /// <summary>
        /// Runs test for disposable strategy extension disposes single instance resolved with no type mapping.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDisposesSingleInstanceResolvedWithNoTypeMappingTest()
        {
            IFourthDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(FourthDisposable), new TransientLifetimeManager());

                    actual = container.Resolve<FourthDisposable>();

                    container.Teardown(actual);
                }
            }

            Assert.IsTrue(actual.IsDisposed, "TearDown failed to dispose the instance");
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose built up child instance if parent is still in lifetime manager.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeBuiltUpChildInstanceIfParentIsStillInLifetimeManagerTest()
        {
            FirstDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (ContainerControlledLifetimeManager containerControlledLifetimeManager = new ContainerControlledLifetimeManager())
                {
                    using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                    {
                        container.AddExtension(disposableStrategyExtension);
                        container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), containerControlledLifetimeManager);
                        container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                        container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                        actual = new FirstDisposable();

                        container.BuildUp(actual);

                        ((DisposableTester)actual.Second).AllowMultipleDisposal = true;

                        container.Teardown(actual);

                        Assert.IsFalse(actual.IsDisposed, "Root was disposed when it should not have been");
                        Assert.IsFalse(actual.Second.IsDisposed, "First child was disposed when it should not have been");
                        Assert.IsFalse(actual.Second.Third.IsDisposed, "The dependency was disposed when it should not have been");
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose built up child instance if still in lifetime manager.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeBuiltUpChildInstanceIfStillInLifetimeManagerTest()
        {
            ThirdDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (ContainerControlledLifetimeManager containerControlledLifetimeManager = new ContainerControlledLifetimeManager())
                {
                    using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                    {
                        container.AddExtension(disposableStrategyExtension);
                        container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), containerControlledLifetimeManager);

                        actual = new ThirdDisposable();

                        container.BuildUp(actual);

                        ((DisposableTester)actual.Fourth).AllowMultipleDisposal = true;

                        container.Teardown(actual);

                        Assert.IsFalse(actual.IsDisposed, "Root was disposed when it should not have been");
                        Assert.IsFalse(actual.Fourth.IsDisposed, "The dependency was disposed when it should not have been");
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose built up root instance.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeBuiltUpRootInstanceTest()
        {
            IThirdDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);

                    actual = new ThirdDisposable();

                    container.BuildUp(actual);

                    container.Teardown(actual);
                }
            }

            Assert.IsFalse(actual.IsDisposed, "TearDown failed to dispose the instance");
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose resolved child instance if parent is still in lifetime manager.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeResolvedChildInstanceIfParentIsStillInLifetimeManagerTest()
        {
            IFirstDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (ContainerControlledLifetimeManager containerControlledLifetimeManager = new ContainerControlledLifetimeManager())
                {
                    using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                    {
                        container.AddExtension(disposableStrategyExtension);
                        container.RegisterType(typeof(IFirstDisposable), typeof(FirstDisposable), new TransientLifetimeManager());
                        container.RegisterType(typeof(ISecondDisposable), typeof(SecondDisposable), containerControlledLifetimeManager);
                        container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                        container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                        actual = container.Resolve<IFirstDisposable>();

                        ((DisposableTester)actual.Second).AllowMultipleDisposal = true;

                        container.Teardown(actual);

                        Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
                        Assert.IsTrue(actual.DuplicateDependency.IsDisposed, "Duplicate dependency was not disposed when it should have been");
                        Assert.IsTrue(
                            actual.DuplicateDependency.Fourth.IsDisposed, "Duplicate dependency child was not disposed when it should have been");
                        Assert.IsFalse(actual.Second.IsDisposed, "First child was disposed when it should not have been");
                        Assert.IsFalse(actual.Second.Third.IsDisposed, "The dependency was disposed when it should not have been");
                        Assert.IsFalse(actual.Second.Third.Fourth.IsDisposed, "The dependency was disposed when it should not have been");
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose resolved child instance when in lifetime manager.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeResolvedChildInstanceWhenInLifetimeManagerTest()
        {
            IThirdDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (ContainerControlledLifetimeManager containerControlledLifetimeManager = new ContainerControlledLifetimeManager())
                {
                    using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                    {
                        container.AddExtension(disposableStrategyExtension);
                        container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                        container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), containerControlledLifetimeManager);

                        actual = container.Resolve<IThirdDisposable>();

                        ((DisposableTester)actual.Fourth).AllowMultipleDisposal = true;

                        container.Teardown(actual);

                        Assert.IsTrue(actual.IsDisposed, "Root was not disposed when it should have been");
                        Assert.IsFalse(actual.Fourth.IsDisposed, "The dependency was disposed when it should not have been");
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension does not dispose resolved single instance when in lifetime manager.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionDoesNotDisposeResolvedSingleInstanceWhenInLifetimeManagerTest()
        {
            IFourthDisposable actual;

            using (UnityContainer container = new UnityContainer())
            {
                using (ContainerControlledLifetimeManager containerControlledLifetimeManager = new ContainerControlledLifetimeManager())
                {
                    using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                    {
                        container.AddExtension(disposableStrategyExtension);
                        container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), containerControlledLifetimeManager);

                        actual = container.Resolve<IFourthDisposable>();

                        container.Teardown(actual);

                        Assert.IsFalse(actual.IsDisposed, "Root was disposed when it should not have been");
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension tracks lazy injection build trees.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionTracksLazyInjectionBuildTreesTest()
        {
            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    LazyBuildTest target = container.Resolve<LazyBuildTest>();

                    IThirdDisposable third = target.Third();
                    IFourthDisposable fourth = target.Forth();

                    container.Teardown(target);

                    Assert.IsTrue(third.IsDisposed, "IsDisposed returned an incorrect value");
                    Assert.IsTrue(fourth.IsDisposed, "IsDisposed returned an incorrect value");
                }
            }
        }

        /// <summary>
        /// Runs test for disposable strategy extension tracks multiple invocations of lazy injection build trees.
        /// </summary>
        [TestMethod]
        public void DisposableStrategyExtensionTracksMultipleInvocationsOfLazyInjectionBuildTreesTest()
        {
            using (UnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.AddExtension(disposableStrategyExtension);
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable), new TransientLifetimeManager());
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable), new TransientLifetimeManager());

                    LazyBuildTest target = container.Resolve<LazyBuildTest>();

                    IThirdDisposable firstInstance = target.Third();
                    IThirdDisposable secondInstance = target.Third();

                    container.Teardown(target);

                    Assert.IsTrue(firstInstance.IsDisposed, "IsDisposed returned an incorrect value");
                    Assert.IsTrue(secondInstance.IsDisposed, "IsDisposed returned an incorrect value");
                }
            }
        }
    }
}