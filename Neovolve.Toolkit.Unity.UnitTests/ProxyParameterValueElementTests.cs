namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ProxyParameterValueElementTests"/>
    ///   class is used to unit test the <see cref="ProxyParameterValueElement"/> class.
    /// </summary>
    [TestClass]
    public class ProxyParameterValueElementTests
    {
        /// <summary>
        /// Runs test for proxy parameter value element can assign and return name.
        /// </summary>
        [TestMethod]
        public void ProxyParameterValueElementCanAssignAndReturnNameTest()
        {
            ProxyParameterValueElement target = new ProxyParameterValueElement();
            String expected = Guid.NewGuid().ToString();

            target.Name = expected;

            String actual = target.Name;

            Assert.AreEqual(expected, actual, "Name returned an incorrect value");
        }

        /// <summary>
        /// Runs test for proxy parameter value element get injection parameter value throws exception for null parameter type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProxyParameterValueElementGetInjectionParameterValueThrowsExceptionForNullParameterTypeTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ProxyParameterValueElement target = new ProxyParameterValueElement();

            target.GetInjectionParameterValue(container, null);
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