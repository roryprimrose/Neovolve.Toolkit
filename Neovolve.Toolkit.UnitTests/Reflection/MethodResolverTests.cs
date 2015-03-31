namespace Neovolve.Toolkit.UnitTests.Reflection
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Reflection;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// This is a test class for MethodResolverTests and is intended
    ///   to contain all MethodResolverTests Unit Tests.
    /// </summary>
    [TestClass]
    public class MethodResolverTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            CacheStoreFactory.StoreType = null;
        }

        #endregion

        /// <summary>
        /// Runs test for can resolve from cache.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void CanResolveFromCacheTest()
        {
            ICacheStore cache = MockRepository.GenerateStub<ICacheStore>();

            CacheStoreMockWrapper.MockInstance = cache;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);

            MethodBase expected = MethodBase.GetCurrentMethod();
            MethodCacheKey key = new MethodCacheKey(GetType(), expected.Name);

            cache.Stub(x => x.Contains(key.ToString())).Return(true);
            cache.Add(key.ToString(), expected.MethodHandle);

            MethodInfo actual = MethodResolver.Resolve(GetType(), key.MethodName);

            Assert.AreSame(expected, actual, "ResolveInternal returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve method from cache.
        /// </summary>
        [TestMethod]
        public void CanResolveMethodFromCacheTest()
        {
            MethodBase expected = MethodBase.GetCurrentMethod();

            MethodBase actual = MethodResolver.Resolve(GetType(), expected.Name, null);

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");

            MethodBase cachedActual = MethodResolver.Resolve(GetType(), expected.Name, null);

            Assert.AreSame(actual, cachedActual, "Resolve returned an incorrect item from the cache");
        }

        /// <summary>
        /// Runs test for can resolve method with null parameter types.
        /// </summary>
        [TestMethod]
        public void CanResolveMethodWithNullParameterTypesTest()
        {
            MethodBase expected = MethodBase.GetCurrentMethod();

            MethodBase actual = MethodResolver.Resolve(GetType(), expected.Name, null);

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve method with parameter types.
        /// </summary>
        [TestMethod]
        public void CanResolveMethodWithParameterTypesTest()
        {
            RunResolveWithParametersTest("SomeValue", "AnotherValue");
        }

        /// <summary>
        /// Runs test for can resolve method without parameter types.
        /// </summary>
        [TestMethod]
        public void CanResolveMethodWithoutParameterTypesTest()
        {
            MethodBase expected = MethodBase.GetCurrentMethod();

            MethodBase actual = MethodResolver.Resolve(GetType(), expected.Name);

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Methods the resolver can resolve method from interface implemented by an interface test.
        /// </summary>
        [TestMethod]
        public void MethodResolverCanResolveMethodFromInterfaceImplementedByAnInterfaceTest()
        {
            MethodBase expected = typeof(IDisposable).GetMethod("Dispose");

            MethodBase actual = MethodResolver.Resolve(typeof(ITester), "Dispose", null);

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for method resolver throws exception when method cannot be found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void MethodResolverThrowsExceptionWhenMethodCannotBeFoundTest()
        {
            MethodResolver.Resolve(typeof(IDisposable), "SomeRandomMethodName");
        }

        /// <summary>
        /// Runs test for resolve with empty method name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveWithEmptyMethodNameThrowsExceptionTest()
        {
            MethodResolver.Resolve(GetType(), String.Empty);
        }

        /// <summary>
        /// Runs test for resolve with null declaring type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveWithNullDeclaringTypeThrowsExceptionTest()
        {
            MethodResolver.Resolve(null, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Runs test for resolve with null method name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveWithNullMethodNameThrowsExceptionTest()
        {
            MethodResolver.Resolve(GetType(), null);
        }

        #region Helper Methods

        /// <summary>
        /// Runs the resolve with parameters test.
        /// </summary>
        /// <param name="firstValue">
        /// The first value.
        /// </param>
        /// <param name="secondValue">
        /// The second value.
        /// </param>
        private void RunResolveWithParametersTest(String firstValue, String secondValue)
        {
            Trace.WriteLine("First Value: " + firstValue);
            Trace.WriteLine("Second Value: " + secondValue);

            MethodBase expected = MethodBase.GetCurrentMethod();
            MethodBase actual = MethodResolver.Resolve(GetType(), expected.Name, typeof(String), typeof(String));

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        #endregion

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