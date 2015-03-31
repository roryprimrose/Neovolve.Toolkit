namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ConfigurationStoreExtensionTests"/>
    ///   class is used to test the extension methods for the <see cref="IConfigurationStore"/> interface.
    /// </summary>
    [TestClass]
    public class ConfigurationStoreExtensionTests
    {
        /// <summary>
        /// Runs test for get application setting returns default value for reference type for missing string configuration.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsDefaultValueForReferenceTypeForMissingStringConfigurationTest()
        {
            String key = Guid.NewGuid().ToString();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            Object actual = store.GetApplicationSetting<Object>(key, null);

            Assert.IsNull(actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting returns default value for value type for missing string configuration.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsDefaultValueForValueTypeForMissingStringConfigurationTest()
        {
            String key = Guid.NewGuid().ToString();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            Int32 actual = store.GetApplicationSetting(key, 0);

            Assert.AreEqual(0, actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting returns defined default value for missing string configuration.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsDefinedDefaultValueForMissingStringConfigurationTest()
        {
            String key = Guid.NewGuid().ToString();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            Int32 expected = Environment.TickCount;

            Int32 actual = store.GetApplicationSetting(key, expected);

            Assert.AreEqual(expected, actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting returns stored value when store has value and value is required.
        /// </summary>
        [TestMethod]
        public void GetApplicationSettingReturnsStoredValueWhenStoreHasValueAndValueIsRequiredTest()
        {
            String key = Guid.NewGuid().ToString();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            Int32 expected = Environment.TickCount;

            store.Stub(x => x.GetApplicationSetting<Int32>(key)).Return(expected);

            Int32 actual = store.GetApplicationSetting(key, 0, true);

            Assert.AreEqual(expected, actual, "GetApplicationSetting returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get application setting throws exception when required value not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void GetApplicationSettingThrowsExceptionWhenRequiredValueNotFoundTest()
        {
            String key = Guid.NewGuid().ToString();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            store.GetApplicationSetting(key, 0, true);
        }

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}