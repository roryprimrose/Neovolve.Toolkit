namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ConfigurationTypeLoaderTests"/>
    ///   class is used to test the <see cref="ConfigurationTypeLoader"/> class.
    /// </summary>
    [TestClass]
    public class ConfigurationTypeLoaderTests
    {
        /// <summary>
        /// Runs test for determine store type returns configuration manager store for empty configuration.
        /// </summary>
        [TestMethod]
        public void DetermineStoreTypeReturnsConfigurationManagerStoreForEmptyConfigurationTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();
            String key = Guid.NewGuid().ToString();
            Type expected = typeof(ConfigurationManagerStore);

            using (mock.Record())
            {
                store.GetApplicationSetting<String>(key);
                LastCall.Return(String.Empty);
            }

            using (mock.Playback())
            {
                Type actual = ConfigurationTypeLoader.DetermineStoreType(store, key, typeof(IConfigurationStore), expected);

                Assert.AreEqual(expected, actual, "DetermineStoreType returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for determine store type returns correct type defined in configuration.
        /// </summary>
        [TestMethod]
        public void DetermineStoreTypeReturnsCorrectTypeDefinedInConfigurationTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();
            String key = Guid.NewGuid().ToString();
            Type expected = typeof(DictionaryCacheStore);

            using (mock.Record())
            {
                store.GetApplicationSetting<String>(key);
                LastCall.Return(typeof(DictionaryCacheStore).AssemblyQualifiedName);
            }

            using (mock.Playback())
            {
                Type actual = ConfigurationTypeLoader.DetermineStoreType(store, key, typeof(ICacheStore), typeof(DictionaryCacheStore));

                Assert.AreEqual(expected, actual, "DetermineStoreType returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for determine store type throws exception on invalid type configuration.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void DetermineStoreTypeThrowsExceptionOnInvalidTypeConfigurationTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();
            String key = Guid.NewGuid().ToString();
            Type expected = typeof(DictionaryCacheStore);

            using (mock.Record())
            {
                store.GetApplicationSetting<String>(key);
                LastCall.Return(typeof(DictionaryCacheStore).AssemblyQualifiedName);
            }

            using (mock.Playback())
            {
                Type actual = ConfigurationTypeLoader.DetermineStoreType(store, key, typeof(IConfigurationStore), typeof(DictionaryCacheStore));

                Assert.AreEqual(expected, actual, "DetermineStoreType returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for determine store type throws exception on type load failure.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void DetermineStoreTypeThrowsExceptionOnTypeLoadFailureTest()
        {
            MockRepository mock = new MockRepository();
            IConfigurationStore store = mock.StrictMock<IConfigurationStore>();
            String key = Guid.NewGuid().ToString();
            Type expected = typeof(DictionaryCacheStore);

            using (mock.Record())
            {
                store.GetApplicationSetting<String>(key);
                LastCall.Return(Guid.NewGuid().ToString());
            }

            using (mock.Playback())
            {
                Type actual = ConfigurationTypeLoader.DetermineStoreType(store, key, typeof(IConfigurationStore), typeof(DictionaryCacheStore));

                Assert.AreEqual(expected, actual, "DetermineStoreType returned an incorrect value");
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