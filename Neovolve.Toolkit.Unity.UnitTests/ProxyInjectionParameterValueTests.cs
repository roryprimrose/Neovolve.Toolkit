namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.Security.Principal;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="ProxyInjectionParameterValueTests"/>
    ///   class is used to test the <see cref="ProxyInjectionParameterValue"/> class.
    /// </summary>
    [TestClass]
    public class ProxyInjectionParameterValueTests
    {
        /// <summary>
        /// Runs test for proxy injection parameter value can get resolver policy throws exception for interface.
        /// </summary>
        [TestMethod]
        public void ProxyInjectionParameterValueCanGetResolverPolicyThrowsExceptionForInterfaceTest()
        {
            ProxyInjectionParameterValue target = new ProxyInjectionParameterValue(typeof(IPrincipal));

            IDependencyResolverPolicy dependencyResolverPolicy = target.GetResolverPolicy(typeof(IPrincipal));

            Assert.IsNotNull(dependencyResolverPolicy, "GetResolverPolicy failed to return an instance");
        }

        /// <summary>
        /// Runs test for proxy injection parameter value can get resolver policy throws exception for marshal by ref object.
        /// </summary>
        [TestMethod]
        public void ProxyInjectionParameterValueCanGetResolverPolicyThrowsExceptionForMarshalByRefObjectTest()
        {
            ProxyInjectionParameterValue target = new ProxyInjectionParameterValue(typeof(TestProxyTarget), Guid.NewGuid().ToString());

            IDependencyResolverPolicy dependencyResolverPolicy = target.GetResolverPolicy(typeof(TestProxyTarget));

            Assert.IsNotNull(dependencyResolverPolicy, "GetResolverPolicy failed to return an instance");
        }

        /// <summary>
        /// Runs test for proxy injection parameter value get resolver policy throws exception for unsupported type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProxyInjectionParameterValueGetResolverPolicyThrowsExceptionForUnsupportedTypeTest()
        {
            ProxyInjectionParameterValue target = new ProxyInjectionParameterValue(typeof(String));

            target.GetResolverPolicy(typeof(String));
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