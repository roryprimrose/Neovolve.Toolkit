namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.ServiceModel.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;

    /// <summary>
    /// The <see cref="ConfigurationManagerStoreTests"/>
    ///   class is used to test the <see cref="ConfigurationManagerStore"/> class.
    /// </summary>
    [TestClass]
    public class ConfigurationManagerStoreTests
    {
        /// <summary>
        /// Runs test for configuration manager store can create with null cache store and time span.
        /// </summary>
        [TestMethod]
        public void ConfigurationManagerStoreCanCreateWithNullCacheStoreAndTimeSpanTest()
        {
            new ConfigurationManagerStore(null, TimeSpan.Zero);
        }

        /// <summary>
        /// Runs test for configuration manager store can create with null cache store.
        /// </summary>
        [TestMethod]
        public void ConfigurationManagerStoreCanCreateWithNullCacheStoreTest()
        {
            new ConfigurationManagerStore(null);
        }

        /// <summary>
        /// Runs test for configuration manager store returns value from cache when value already calculated.
        /// </summary>
        [TestMethod]
        public void ConfigurationManagerStoreReturnsValueFromCacheWhenValueAlreadyCalculatedTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            ConfigurationManagerStore target = new ConfigurationManagerStore(cacheStore);
            String key = Guid.NewGuid().ToString();
            Int32 expected = Environment.TickCount;

            cacheStore.Stub(x => x.Contains(key)).Return(true);
            cacheStore.Stub(x => x.GetItem(key)).Return(expected);

            Int32 actual = target.GetApplicationSetting<Int32>(key);

            Assert.AreEqual(expected, actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for configuration manager store stores value in cache when value is not already calculated.
        /// </summary>
        [TestMethod]
        public void ConfigurationManagerStoreStoresValueInCacheWhenValueIsNotAlreadyCalculatedTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            ConfigurationManagerStore target = new ConfigurationManagerStore(cacheStore);
            String key = Guid.NewGuid().ToString();
            Int32 expected = Environment.TickCount;

            ConfigurationManager.AppSettings[key] = expected.ToString(CultureInfo.InvariantCulture);

            cacheStore.Stub(x => x.Contains(key)).Return(false);

            target.GetApplicationSetting<Int32>(key);

            cacheStore.AssertWasCalled(x => x.Add(key, expected, null));
        }

        /// <summary>
        /// Runs test for configuration manager stores value in cache with expiration policy when value is not already calculated.
        /// </summary>
        [TestMethod]
        public void ConfigurationManagerStoresValueInCacheWithExpirationPolicyWhenValueIsNotAlreadyCalculatedTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateStub<ICacheStore>();
            TimeSpan expiration = TimeSpan.FromMinutes(3);
            ConfigurationManagerStore target = new ConfigurationManagerStore(cacheStore, expiration);
            String key = Guid.NewGuid().ToString();
            Int32 expected = Environment.TickCount;

            ConfigurationManager.AppSettings[key] = expected.ToString(CultureInfo.InvariantCulture);

            cacheStore.Stub(x => x.Contains(key)).Return(false);

            target.GetApplicationSetting<Int32>(key);

            cacheStore.AssertWasCalled(
                x => x.Add(null, null, null), 
                opt => opt.Constraints(Is.Equal(key), Is.Equal(expected), List.Element(0, Property.Value("Expiration", expiration))));
        }

        /// <summary>
        /// Runs test for get application setting returns correct convert supported value.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsCorrectConvertSupportedValueTest()
        {
            String key = Guid.NewGuid().ToString();
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            ConfigurationManager.AppSettings[key] = "123";

            Int32 actual = target.GetApplicationSetting<Int32>(key);

            Assert.AreEqual(123, actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting returns correct type converter value.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsCorrectTypeConverterValueTest()
        {
            String key = Guid.NewGuid().ToString();
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            ConfigurationManager.AppSettings[key] = "123, 456";

            Point actual = target.GetApplicationSetting<Point>(key);

            Assert.AreEqual(123, actual.X, "GetApplicationSetting returned an incorrect value");
            Assert.AreEqual(456, actual.Y, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting returns correct value.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsCorrectValueTest()
        {
            String key = Guid.NewGuid().ToString();
            String expected = Guid.NewGuid().ToString();

            // Store the test value
            ConfigurationManager.AppSettings[key] = expected;

            ConfigurationManagerStore target = new ConfigurationManagerStore();

            String actual = target.GetApplicationSetting<String>(key);

            // Clean up the configuration
            ConfigurationManager.AppSettings[key] = null;

            Assert.AreEqual(expected, actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting with empty key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetApplicationSettingWithEmptyKeyThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetApplicationSetting<String>(String.Empty);
        }

        /// <summary>
        /// Runs test for get application setting with invalid type conversion throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetApplicationSettingWithInvalidTypeConversionThrowsExceptionTest()
        {
            String key = Guid.NewGuid().ToString();

            // Store the test value
            ConfigurationManager.AppSettings[key] = "true";

            ConfigurationManagerStore target = new ConfigurationManagerStore();

            try
            {
                target.GetApplicationSetting<Int32>(key);
            }
            finally
            {
                // Clean up the configuration
                ConfigurationManager.AppSettings[key] = null;
            }
        }

        /// <summary>
        /// Runs test for get application setting with null key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetApplicationSettingWithNullKeyThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetApplicationSetting<String>(null);
        }

        /// <summary>
        /// Runs test for get connection setting returns correct value.
        /// </summary>
        [TestMethod]
        public void GetConnectionSettingReturnsCorrectValueTest()
        {
            const String Key = "TestConnectionSetting";
            ConnectionStringSettings expected = ConfigurationManager.ConnectionStrings[Key];
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            ConnectionStringSettings actual = target.GetConnectionSetting(Key);

            Assert.AreSame(expected, actual, "GetConnectionSetting return an incorrect value");
        }

        /// <summary>
        /// Runs test for get connection setting returns null for missing configuration.
        /// </summary>
        [TestMethod]
        public void GetConnectionSettingReturnsNullForMissingConfigurationTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();
            ConnectionStringSettings actual = target.GetConnectionSetting(Guid.NewGuid().ToString());

            Assert.IsNull(actual, "GetConfigurationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get connection setting with empty key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetConnectionSettingWithEmptyKeyThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetConnectionSetting(String.Empty);
        }

        /// <summary>
        /// Runs test for get connection setting with null key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetConnectionSettingWithNullKeyThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetConnectionSetting(null);
        }

        /// <summary>
        /// Runs test for get section returns correct type.
        /// </summary>
        [TestMethod]
        public void GetSectionReturnsCorrectTypeTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            ClientSection section = target.GetSection<ClientSection>("system.serviceModel/client");

            Assert.IsNotNull(section, "GetSection returned an incorrect valuue");
        }

        /// <summary>
        /// Runs test for get section returns null for missing configuration.
        /// </summary>
        [TestMethod]
        public void GetSectionReturnsNullForMissingConfigurationTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            ConfigurationSection actual = target.GetSection<ConfigurationSection>(Guid.NewGuid().ToString());

            Assert.IsNull(actual, "GetSection returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get section with empty key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSectionWithEmptyKeyThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetSection<ConfigurationSection>(String.Empty);
        }

        /// <summary>
        /// Runs test for get section with empty null throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSectionWithEmptyNullThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetSection<ConfigurationSection>(null);
        }

        /// <summary>
        /// Runs test for get section with incorrect section type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void GetSectionWithIncorrectSectionTypeThrowsExceptionTest()
        {
            ConfigurationManagerStore target = new ConfigurationManagerStore();

            target.GetSection<AppSettingsSection>("system.serviceModel/client");
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