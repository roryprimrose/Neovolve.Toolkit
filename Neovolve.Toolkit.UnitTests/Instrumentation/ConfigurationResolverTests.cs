namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// This is a test class for ConfigurationResolverTests and is intended
    ///   to contain all ConfigurationResolverTests Unit Tests.
    /// </summary>
    [TestClass]
    public class ConfigurationResolverTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            ConfigurationStoreFactory.StoreType = null;
        }

        /// <summary>
        /// Initializes the test for running unit tests.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            ConfigurationResolver resolver = new ConfigurationResolver();

            // Clear the statically stored names
            resolver.Reload();
        }

        #endregion

        /// <summary>
        /// Runs test for child resolver returns null.
        /// </summary>
        [TestMethod]
        public void ChildResolverReturnsNullTest()
        {
            ConfigurationResolver target = new ConfigurationResolver();

            Assert.IsNull(target.ChildResolver, "ChildResolver returned an instance when null was expected");
        }

        /// <summary>
        /// Runs test for reload clears stored names.
        /// </summary>
        [TestMethod]
        public void ReloadClearsStoredNamesTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section).Repeat.Twice();
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    Collection<String> actual = target.ResolveNames();

                    target.Reload();

                    Collection<String> newNames = target.ResolveNames();

                    Assert.AreNotSame(actual, newNames, "Names returned were not cached");
                    Assert.AreEqual(actual.Count, newNames.Count, "Incorrect number of items returned");
                    Assert.AreEqual(actual[0], newNames[0], "Incorrect name was returned");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve names returns cached names.
        /// </summary>
        [TestMethod]
        public void ResolveNamesReturnsCachedNamesTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    Collection<String> actual = target.ResolveNames();
                    Collection<String> cachedNames = target.ResolveNames();

                    Assert.AreSame(actual, cachedNames, "Names returned were not cached");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve names returns configured names.
        /// </summary>
        [TestMethod]
        public void ResolveNamesReturnsConfiguredNamesTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    Collection<String> actual = target.ResolveNames();

                    Assert.IsNotNull(actual, "ResolveSources failed to return an instance");
                    Assert.AreEqual(1, actual.Count, "Count returned an incorrect value");
                    Assert.AreEqual(element.Name, actual[0], "Invalid name was returned");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve names with empty section returns empty name list.
        /// </summary>
        [TestMethod]
        public void ResolveNamesWithEmptySectionReturnsEmptyNameListTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    Collection<String> actual = target.ResolveNames();

                    Assert.IsNotNull(actual, "ResolveSources failed to return an instance");
                    Assert.AreEqual(0, actual.Count, "Count returned an incorrect value");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve names with null configuration element collection returns empty name list.
        /// </summary>
        [TestMethod]
        public void ResolveNamesWithNullConfigurationElementCollectionReturnsEmptyNameListTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();

            section.Sources = null;

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    Collection<String> actual = target.ResolveNames();

                    Assert.IsNotNull(actual, "ResolveSources failed to return an instance");
                    Assert.AreEqual(0, actual.Count, "Count returned an incorrect value");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve names with null section returns empty name list.
        /// </summary>
        [TestMethod]
        public void ResolveNamesWithNullSectionReturnsEmptyNameListTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    Collection<String> actual = target.ResolveNames();

                    Assert.IsNotNull(actual, "ResolveSources failed to return an instance");
                    Assert.AreEqual(0, actual.Count, "Count returned an incorrect value");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve returns null with case mismatch.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsNullWithCaseMismatchTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            element.Name = "Test";

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    TraceSource source = target.Resolve("test", StringComparison.Ordinal);

                    Assert.IsNull(source, "Case sensitive match returned a match when null was expected");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve returns trace source with case insensitive match.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsTraceSourceWithCaseInsensitiveMatchTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            element.Name = "Test";

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    TraceSource source = target.Resolve("test", StringComparison.OrdinalIgnoreCase);

                    Assert.IsNotNull(source, "Resolve failed to return an instance");
                    Assert.AreEqual(element.Name, source.Name, "Name returned an incorrect value");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve returns trace source for each configured name using different string comparisons.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsTraceSourceWithCaseSensitiveMatchTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            element.Name = "Test";

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    TraceSource source = target.Resolve("Test", StringComparison.Ordinal);

                    Assert.IsNotNull(source, "Resolve failed to return an instance");
                    Assert.AreEqual(element.Name, source.Name, "Name returned an incorrect value");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
        }

        /// <summary>
        /// Runs test for resolve with invalid name returns null trace source.
        /// </summary>
        [TestMethod]
        public void ResolveWithInvalidNameReturnsNullTraceSourceTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();

            ConfigurationSectionStub section = new ConfigurationSectionStub();
            ConfigurationElementStub element = new ConfigurationElementStub();

            section.Sources.Add(element);

            using (mock.Record())
            {
                store.GetSection<ConfigurationSection>("system.diagnostics");
                LastCall.Return(section);
            }

            using (mock.Playback())
            {
                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
                    ConfigurationStoreMockWrapper.MockInstance = store;

                    ConfigurationResolver target = new ConfigurationResolver();
                    TraceSource source = target.Resolve(Guid.NewGuid().ToString(), StringComparison.Ordinal);

                    Assert.IsNull(source, "Resolve returned an instance for a name that should not match");
                }
                finally
                {
                    ConfigurationStoreMockWrapper.MockInstance = null;
                    ConfigurationStoreFactory.StoreType = null;
                }
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