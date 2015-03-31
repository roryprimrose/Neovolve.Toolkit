namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="AppSettingsParameterValueElementTests"/>
    ///   class is used to test the <see cref="AppSettingsParameterValueElement"/> class.
    /// </summary>
    [TestClass]
    public class AppSettingsParameterValueElementTests
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

        #endregion

        /// <summary>
        /// Runs test for app settings parameter value element create value returns app setting boolean.
        /// </summary>
        [TestMethod]
        public void AppSettingsParameterValueElementCreateValueReturnsAppSettingBooleanTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return("true");

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            Object actual = target.CreateValue(typeof(Boolean));

            Assert.IsTrue((Boolean)actual, "CreateValue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value returns app setting string.
        /// </summary>
        [TestMethod]
        public void AppSettingsParameterValueElementCreateValueReturnsAppSettingStringTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            String expected = Guid.NewGuid().ToString();
            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return(expected);

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            Object actual = target.CreateValue(typeof(String));

            Assert.AreEqual(expected, actual, "CreateValue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value returns app setting using type converter.
        /// </summary>
        [TestMethod]
        public void AppSettingsParameterValueElementCreateValueReturnsAppSettingUsingTypeConverterTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return("true");

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey, 
                                                          TypeConverterTypeName = typeof(BooleanConverter).AssemblyQualifiedName
                                                      };

            Object actual = target.CreateValue(typeof(Boolean));

            Assert.IsTrue((Boolean)actual, "CreateValue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value throws exception with invalid type converter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AppSettingsParameterValueElementCreateValueThrowsExceptionWithInvalidTypeConverterTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return("true");

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey, 
                                                          TypeConverterTypeName = typeof(String).FullName
                                                      };

            target.CreateValue(typeof(Boolean));
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value throws exception with null parameter type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppSettingsParameterValueElementCreateValueThrowsExceptionWithNullParameterTypeTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            String expected = Guid.NewGuid().ToString();
            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return(expected);

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            target.CreateValue(null);
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value throws exception with unknown type converter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AppSettingsParameterValueElementCreateValueThrowsExceptionWithUnknownTypeConverterTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return("true");

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey, 
                                                          TypeConverterTypeName = Guid.NewGuid().ToString()
                                                      };

            target.CreateValue(typeof(Boolean));
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value throws exception with unsupported type conversion of null value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AppSettingsParameterValueElementCreateValueThrowsExceptionWithUnsupportedTypeConversionOfNullValueTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return(null);

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            try
            {
                target.CreateValue(typeof(Stream));
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(NotSupportedException), "InnerException returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for app settings parameter value element create value throws exception with unsupported type conversion.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AppSettingsParameterValueElementCreateValueThrowsExceptionWithUnsupportedTypeConversionTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return(Guid.NewGuid().ToString());

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            try
            {
                target.CreateValue(typeof(Stream));
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(NotSupportedException), "InnerException returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for app settings parameter value element get injection parameter value creates from app setting value with type name.
        /// </summary>
        [TestMethod]
        public void AppSettingsParameterValueElementGetInjectionParameterValueCreatesFromAppSettingValueWithTypeNameTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return(Guid.NewGuid().ToString());

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            InjectionParameterValue actual = target.GetInjectionParameterValue(null, typeof(Guid));

            Assert.IsNotNull(actual, "GetInjectionParameterValue returned ann incorrect value");
        }

        /// <summary>
        /// Runs test for app settings parameter value element get injection parameter value creates from app setting value without type name.
        /// </summary>
        [TestMethod]
        public void AppSettingsParameterValueElementGetInjectionParameterValueCreatesFromAppSettingValueWithoutTypeNameTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            store.Stub(x => x.GetApplicationSetting<String>(ConfigurationKey)).Return(Guid.NewGuid().ToString());

            AppSettingsParameterValueElement target = new AppSettingsParameterValueElement
                                                      {
                                                          AppSettingKey = ConfigurationKey
                                                      };

            InjectionParameterValue actual = target.GetInjectionParameterValue(null, typeof(Guid));

            Assert.IsNotNull(actual, "GetInjectionParameterValue returned ann incorrect value");
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