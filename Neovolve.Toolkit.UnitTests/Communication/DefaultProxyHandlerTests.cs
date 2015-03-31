namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="DefaultProxyHandlerTests"/>
    ///   class is used to test the <see cref="DefaultProxyHandler{T}"/> class.
    /// </summary>
    [TestClass]
    public class DefaultProxyHandlerTests
    {
        /// <summary>
        /// Runs test for default proxy handler invokes method on created instance.
        /// </summary>
        [TestMethod]
        public void DefaultProxyHandlerInvokesMethodOnCreatedInstanceTest()
        {
            const String InputValue = "haha";
            ProxyStub compareInstance = new ProxyStub();
            String expected = compareInstance.DoSomething(InputValue);

            DefaultProxyHandler<ProxyStub> target = new DefaultProxyHandler<ProxyStub>();

            ProxyStub proxy = (ProxyStub)target.GetTransparentProxy();

            String actual = proxy.DoSomething(InputValue);

            Assert.AreEqual(expected, actual, "DoSomething returned an incorrect value");
        }

        /// <summary>
        /// Runs test for default proxy handler invokes method on provided instance.
        /// </summary>
        [TestMethod]
        public void DefaultProxyHandlerInvokesMethodOnProvidedInstanceTest()
        {
            const String InputValue = "haha";
            ProxyStub instance = MockRepository.GenerateStub<ProxyStub>();

            DefaultProxyHandler<ProxyStub> target = new DefaultProxyHandler<ProxyStub>(instance);

            ProxyStub proxy = (ProxyStub)target.GetTransparentProxy();

            proxy.DoSomething(InputValue);

            instance.AssertWasCalled(x => x.DoSomething(InputValue));
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