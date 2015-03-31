namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// This is a test class for CacheResolverTests and is intended
    ///   to contain all CacheResolverTests Unit Tests.
    /// </summary>
    [TestClass]
    public class CacheResolverTests
    {
        /// <summary>
        /// Runs test for cache resolver caches trace source names as unique to child resolver type.
        /// </summary>
        [TestMethod]
        public void CacheResolverCachesTraceSourceNamesAsUniqueToChildResolverTypeTest()
        {
            // Create the first ChildResolver and resolver
            ITraceSourceResolver firstResolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            Collection<String> firstExpectedNames = new Collection<String>
                                                    {
                                                        "first"
                                                    };

            firstResolver.Stub(x => x.ResolveNames()).Return(firstExpectedNames);

            CacheResolver firstTarget = new CacheResolver(firstResolver);

            // Create the second ChildResolver and resolver
            ITraceSourceResolver secondResolver = new TestTraceSourceResolver();
            CacheResolver secondTarget = new CacheResolver(secondResolver);

            Assert.AreNotEqual(
                firstResolver.GetType().AssemblyQualifiedName, secondResolver.GetType().AssemblyQualifiedName, "Resolvers have the same name");

            ICacheStore cacheStore = CacheStoreFactory.Create();

            // Clear the caches
            cacheStore.Clear();

            Collection<String> firstResult = firstTarget.ResolveNames();

            Assert.AreEqual(firstExpectedNames, firstResult, "ResolveNames returned an incorrect value");

            Collection<String> secondResult = secondTarget.ResolveNames();

            Assert.AreNotEqual(firstResult, secondResult, "Resolvers returned the same names");
        }

        /// <summary>
        /// Runs test for cache resolver caches trace sources as unique to child resolver type.
        /// </summary>
        [TestMethod]
        public void CacheResolverCachesTraceSourcesAsUniqueToChildResolverTypeTest()
        {
            ITraceSourceResolver firstResolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            Collection<String> firstExpectedNames = new Collection<String>
                                                    {
                                                        TestTraceSourceResolver.SourceName
                                                    };
            TraceSource firstSource = new TraceSource("some other name");

            firstResolver.Stub(x => x.ResolveNames()).Return(firstExpectedNames);
            firstResolver.Stub(x => x.Resolve(TestTraceSourceResolver.SourceName, StringComparison.OrdinalIgnoreCase)).Return(firstSource);

            CacheResolver firstTarget = new CacheResolver(firstResolver);

            // Create the second ChildResolver and resolver
            ITraceSourceResolver secondResolver = new TestTraceSourceResolver();
            CacheResolver secondTarget = new CacheResolver(secondResolver);

            Assert.AreNotEqual(
                firstResolver.GetType().AssemblyQualifiedName, secondResolver.GetType().AssemblyQualifiedName, "Resolvers have the same name");

            ICacheStore cacheStore = CacheStoreFactory.Create();

            // Clear the caches
            cacheStore.Clear();

            TraceSource firstResult = firstTarget.Resolve(TestTraceSourceResolver.SourceName, StringComparison.OrdinalIgnoreCase);

            Assert.AreSame(firstSource, firstResult, "Resolve returned an incorrect result");

            TraceSource secondResult = secondTarget.Resolve(TestTraceSourceResolver.SourceName, StringComparison.OrdinalIgnoreCase);

            Assert.AreNotSame(firstResult, secondResult, "Resolvers returned the same instances");
            Assert.AreNotEqual(firstResult.Name, secondResult.Name, "Resolvers returned the same sources");
        }

        /// <summary>
        /// Runs test for can create with resolver.
        /// </summary>
        [TestMethod]
        public void CanCreateWithResolverTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.Stub<ITraceSourceResolver>();
            CacheResolver target = new CacheResolver(resolver);

            Assert.AreEqual(resolver, target.ChildResolver, "ChildResolver returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating with null resolver throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingWithNullResolverThrowsExceptionTest()
        {
            new CacheResolver(null);
        }

        /// <summary>
        /// Runs test for reload clears resolve names from cache.
        /// </summary>
        [TestMethod]
        public void ReloadClearsResolveNamesFromCacheTest()
        {
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            CacheResolver target = new CacheResolver(resolver);
            ICacheStore traceCache = CacheStoreFactory.Create();

            Assert.IsNotNull(traceCache, "Failed to obtain reference to the cache");

            traceCache.Clear();

            Collection<String> expected = new Collection<String>
                                          {
                                              "test", 
                                              "more"
                                          };

            resolver.Stub(x => x.ResolveNames()).Return(expected).Repeat.Twice();

            Collection<String> firstResolveNames = target.ResolveNames();

            target.Reload();

            Collection<String> secondResolveNames = target.ResolveNames();

            Assert.AreSame(expected, firstResolveNames, "ResolveNames returned an incorrect value");
            Assert.AreSame(expected, secondResolveNames, "ResolveNames returned an incorrect value");
        }

        /// <summary>
        /// Runs test for reload clears trace source from cache.
        /// </summary>
        [TestMethod]
        public void ReloadClearsTraceSourceFromCacheTest()
        {
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            CacheResolver target = new CacheResolver(resolver);
            ICacheStore traceCache = CacheStoreFactory.Create();

            Assert.IsNotNull(traceCache, "Failed to obtain reference to the cache");

            traceCache.Clear();

            String traceSourceName = Guid.NewGuid().ToString();
            TraceSource expected = new TraceSource(traceSourceName);

            resolver.Stub(x => x.Resolve(traceSourceName, StringComparison.OrdinalIgnoreCase)).Return(expected).Repeat.Twice();

            TraceSource firstResult = target.Resolve(traceSourceName, StringComparison.OrdinalIgnoreCase);

            target.Reload();

            TraceSource secondResult = target.Resolve(traceSourceName, StringComparison.OrdinalIgnoreCase);

            Assert.AreSame(expected, firstResult, "ResolveNames returned an incorrect value");
            Assert.AreSame(expected, secondResult, "ResolveNames returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve names returns cached value.
        /// </summary>
        [TestMethod]
        public void ResolveNamesReturnsCachedValueTest()
        {
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            CacheResolver target = new CacheResolver(resolver);
            ICacheStore traceCache = CacheStoreFactory.Create();

            Assert.IsNotNull(traceCache, "Failed to obtain reference to the cache");

            traceCache.Clear();

            Collection<String> expected = new Collection<String>
                                          {
                                              "test", 
                                              "more"
                                          };

            resolver.Stub(x => x.ResolveNames()).Return(expected).Repeat.Once();

            Collection<String> firstResolveNames = target.ResolveNames();
            Collection<String> secondResolveNames = target.ResolveNames();

            Assert.AreSame(expected, firstResolveNames, "ResolveNames returned an incorrect value");
            Assert.AreSame(expected, secondResolveNames, "ResolveNames returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve returns cached trace source.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsCachedTraceSourceTest()
        {
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            const String TestName = "test";
            Collection<String> expectedNames = new Collection<String>
                                               {
                                                   TestName, 
                                               };

            TraceSource testSource = new TraceSource(TestName);

            resolver.Stub(x => x.ResolveNames()).Return(expectedNames);
            resolver.Stub(x => x.Resolve(TestName, StringComparison.OrdinalIgnoreCase)).Return(testSource).Repeat.Once();

            CacheResolver target = new CacheResolver(resolver);
            ICacheStore traceCache = CacheStoreFactory.Create();

            traceCache.Clear();
            TraceSource testResponse = target.Resolve(TestName, StringComparison.OrdinalIgnoreCase);

            Assert.AreSame(testSource, testResponse, "Resolve returned an incorrect value");

            TraceSource nextTestResponse = target.Resolve(TestName, StringComparison.OrdinalIgnoreCase);

            Assert.AreSame(testSource, nextTestResponse, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve returns null response from dependency.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsNullResponseFromDependencyTest()
        {
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            CacheResolver target = new CacheResolver(resolver);
            String name = Guid.NewGuid().ToString();

            resolver.Stub(x => x.Resolve(name, StringComparison.OrdinalIgnoreCase)).Return(null);

            ICacheStore traceCache = CacheStoreFactory.Create();

            traceCache.Clear();

            TraceSource actual = target.Resolve(name, StringComparison.OrdinalIgnoreCase);

            Assert.IsNull(actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve with empty trace source name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveWithEmptyTraceSourceNameThrowsExceptionTest()
        {
            CacheResolver target = new CacheResolver(new TestTraceSourceResolver());

            target.Resolve(String.Empty, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Runs test for resolve with null trace source name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveWithNullTraceSourceNameThrowsExceptionTest()
        {
            CacheResolver target = new CacheResolver(new TestTraceSourceResolver());

            target.Resolve(null, StringComparison.OrdinalIgnoreCase);
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