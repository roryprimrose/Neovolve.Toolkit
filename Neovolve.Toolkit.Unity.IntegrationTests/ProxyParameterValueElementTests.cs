namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ProxyParameterValueElementTests"/>
    ///   class is used to test the <see cref="ProxyParameterValueElement"/> class.
    /// </summary>
    [TestClass]
    public class ProxyParameterValueElementTests
    {
        /// <summary>
        /// Proxies the parameter value element creates named proxy in build tree test.
        /// </summary>
        [TestMethod]
        public void ProxyParameterValueElementCreatesNamedProxyInBuildTreeTest()
        {
            using (IUnityContainer container = UnityContainerResolver.Resolve())
            {
                SecondDisposable actual = (SecondDisposable)container.Resolve<ISecondDisposable>("NamedProxyTest");

                Assert.IsNotNull(actual, "Instance was not resolved from container");
                Assert.IsNotNull(actual.Third, "Proxy dependency was not resolved");

                actual.Third.Dispose();

                Assert.IsTrue(actual.Third.IsDisposed, "IsDisposed returned an incorrect value");

                container.Teardown(actual);
            }
        }

        /// <summary>
        /// Runs test for proxy parameter value element creates proxy for unnamed proxy handler configuration.
        /// </summary>
        [TestMethod]
        public void ProxyParameterValueElementCreatesProxyForUnnamedProxyHandlerConfigurationTest()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.RegisterType(
                        typeof(ISecondDisposable), 
                        typeof(SecondDisposable), 
                        new InjectionConstructor(new ProxyInjectionParameterValue(typeof(IThirdDisposable))));
                    container.RegisterType(typeof(ProxyHandler<>), typeof(DefaultProxyHandler<>), new InjectionConstructor(new GenericParameter("T")));
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable));
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable));
                    container.AddExtension(disposableStrategyExtension);

                    SecondDisposable actual = (SecondDisposable)container.Resolve<ISecondDisposable>();

                    Assert.IsNotNull(actual, "Instance was not resolved from container");
                    Assert.IsNotNull(actual.Third, "Proxy dependency was not resolved");

                    actual.Third.Dispose();

                    Assert.IsTrue(actual.Third.IsDisposed, "IsDisposed returned an incorrect value");

                    container.Teardown(actual);
                }
            }
        }

        /// <summary>
        /// Runs test for proxy parameter value element creates proxy for unnamed proxy manager configuration.
        /// </summary>
        [TestMethod]
        public void ProxyParameterValueElementCreatesProxyForUnnamedProxyManagerConfigurationTest()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                using (DisposableStrategyExtension disposableStrategyExtension = new DisposableStrategyExtension())
                {
                    container.RegisterType(
                        typeof(ISecondDisposable), 
                        typeof(SecondDisposable), 
                        new InjectionConstructor(new ProxyInjectionParameterValue(typeof(IThirdDisposable))));
                    container.RegisterType(typeof(ProxyManager<>), typeof(ProxyManager<>));
                    container.RegisterType(typeof(ProxyHandler<>), typeof(DefaultProxyHandler<>), new InjectionConstructor(new GenericParameter("T")));
                    container.RegisterType(typeof(IThirdDisposable), typeof(ThirdDisposable));
                    container.RegisterType(typeof(IFourthDisposable), typeof(FourthDisposable));
                    container.AddExtension(disposableStrategyExtension);

                    SecondDisposable actual = (SecondDisposable)container.Resolve<ISecondDisposable>();

                    Assert.IsNotNull(actual, "Instance was not resolved from container");
                    Assert.IsNotNull(actual.Third, "Proxy dependency was not resolved");

                    actual.Third.Dispose();

                    Assert.IsTrue(actual.Third.IsDisposed, "IsDisposed returned an incorrect value");

                    container.Teardown(actual);
                }
            }
        }

        /// <summary>
        /// Runs test for proxy parameter value element creates proxy in build tree.
        /// </summary>
        [TestMethod]
        public void ProxyParameterValueElementCreatesProxyInBuildTreeTest()
        {
            using (IUnityContainer container = UnityContainerResolver.Resolve())
            {
                Root actual = container.Resolve<Root>();

                Assert.IsNotNull(actual, "Instance was not resolved from container");

                actual.RunTest();

                container.Teardown(actual);
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