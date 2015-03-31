namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="TraceSourceResolverFactoryTests"/> 
    ///   class is used to test the <see cref="TraceSourceResolverFactory"/> class.
    /// </summary>
    [TestClass]
    public class TraceSourceResolverFactoryTests
    {
        /// <summary>
        /// Runs test for assigning A resolver type that is not I trace source resolver type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void AssigningAResolverTypeThatIsNotITraceSourceResolverTypeThrowsExceptionTest()
        {
            TraceSourceResolverFactory.ResolverType = typeof(NotSupportedException);
        }

        /// <summary>
        /// Runs test for can assign A resolver type that is I trace source resolver type.
        /// </summary>
        [TestMethod]
        public void CanAssignAResolverTypeThatIsITraceSourceResolverTypeTest()
        {
            TraceSourceResolverFactory.ResolverType = typeof(ConfigurationResolver);
        }

        /// <summary>
        /// Runs test for can set default resolver type to null.
        /// </summary>
        [TestMethod]
        public void CanSetDefaultResolverTypeToNullTest()
        {
            TraceSourceResolverFactory.ResolverType = null;

            Assert.IsNotNull(TraceSourceResolverFactory.ResolverType, "ResolverType failed to returned a value");
        }

        /// <summary>
        /// Runs test for create returns default resolver instance.
        /// </summary>
        [TestMethod]
        public void CreateReturnsDefaultResolverInstanceTest()
        {
            TraceSourceResolverFactory.ResolverType = null;

            ITraceSourceResolver actual = TraceSourceResolverFactory.Create();

            Assert.IsInstanceOfType(actual, typeof(ConfigurationResolver), "Create failed to return an instance of the correct type");
        }

        /// <summary>
        /// Runs test for default resolver type returns an I trace source resolver type.
        /// </summary>
        [TestMethod]
        public void DefaultResolverTypeReturnsAnITraceSourceResolverTypeTest()
        {
            Type actual = TraceSourceResolverFactory.ResolverType;

            Assert.IsNotNull(actual, "ResolverType failed to return an instance");
            Assert.IsTrue(typeof(ITraceSourceResolver).IsAssignableFrom(actual), "ResolverType returned an unsupported type");
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