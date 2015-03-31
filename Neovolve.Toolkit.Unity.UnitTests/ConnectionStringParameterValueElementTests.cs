namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.Configuration;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ConnectionStringParameterValueElementTests"/>
    ///   class is used to test the <see cref="ConnectionStringParameterValueElement"/> class.
    /// </summary>
    [TestClass]
    public class ConnectionStringParameterValueElementTests
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
        /// Runs test for connections string parameter value element get injection parameter value returns connection setting.
        /// </summary>
        [TestMethod]
        public void ConnectionStringParameterValueElementGetInjectionParameterValueReturnsConnectionSettingTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            String expected = Guid.NewGuid().ToString();
            ConnectionStringSettings settings = new ConnectionStringSettings(ConfigurationKey, expected);
            store.Stub(x => x.GetConnectionSetting(ConfigurationKey)).Return(settings);

            ConnectionStringParameterValueElement target = new ConnectionStringParameterValueElement
                                                           {
                                                               ConnectionStringKey = ConfigurationKey
                                                           };

            InjectionParameterValue actual = target.GetInjectionParameterValue(null, typeof(ConnectionStringSettings));

            store.AssertWasCalled(x => x.GetConnectionSetting(ConfigurationKey));
            Assert.IsNotNull(actual, "GetInjectionParameterValue returned an incorrect value");

            IDependencyResolverPolicy resolverPolicy = actual.GetResolverPolicy(typeof(String));

            ConnectionStringSettings value = resolverPolicy.Resolve(null) as ConnectionStringSettings;

            Assert.AreSame(settings, value, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for connection string parameter value element get injection parameter value returns connection string.
        /// </summary>
        [TestMethod]
        public void ConnectionStringParameterValueElementGetInjectionParameterValueReturnsConnectionStringTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            String expected = Guid.NewGuid().ToString();
            ConnectionStringSettings settings = new ConnectionStringSettings(ConfigurationKey, expected);
            store.Stub(x => x.GetConnectionSetting(ConfigurationKey)).Return(settings);

            ConnectionStringParameterValueElement target = new ConnectionStringParameterValueElement
                                                           {
                                                               ConnectionStringKey = ConfigurationKey
                                                           };

            InjectionParameterValue actual = target.GetInjectionParameterValue(null, typeof(String));

            store.AssertWasCalled(x => x.GetConnectionSetting(ConfigurationKey));
            Assert.IsNotNull(actual, "GetInjectionParameterValue returned an incorrect value");

            IDependencyResolverPolicy resolverPolicy = actual.GetResolverPolicy(typeof(String));

            String value = resolverPolicy.Resolve(null) as String;

            Assert.AreEqual(expected, value, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for connection string parameter value element get injection parameter value throws exception when connection setting not defined.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ConnectionStringParameterValueElementGetInjectionParameterValueThrowsExceptionWhenConnectionSettingNotDefinedTest()
        {
            const String ConfigurationKey = "TestKey";

            ConnectionStringParameterValueElement target = new ConnectionStringParameterValueElement
                                                           {
                                                               ConnectionStringKey = ConfigurationKey
                                                           };

            target.GetInjectionParameterValue(null, typeof(String));
        }

        /// <summary>
        /// Runs test for connection string parameter value element get injection parameter value throws exception with invalid parameter type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConnectionStringParameterValueElementGetInjectionParameterValueThrowsExceptionWithInvalidParameterTypeTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            String expected = Guid.NewGuid().ToString();
            ConnectionStringSettings settings = new ConnectionStringSettings(ConfigurationKey, expected);
            store.Stub(x => x.GetConnectionSetting(ConfigurationKey)).Return(settings);

            ConnectionStringParameterValueElement target = new ConnectionStringParameterValueElement
                                                           {
                                                               ConnectionStringKey = ConfigurationKey
                                                           };

            target.GetInjectionParameterValue(null, typeof(Boolean));
        }

        /// <summary>
        /// Connections the string parameter value element get injection parameter value throws exception with null parameter type test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectionStringParameterValueElementGetInjectionParameterValueThrowsExceptionWithNullParameterTypeTest()
        {
            const String ConfigurationKey = "TestKey";
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            ConnectionStringParameterValueElement target = new ConnectionStringParameterValueElement
                                                           {
                                                               ConnectionStringKey = ConfigurationKey
                                                           };

            target.GetInjectionParameterValue(null, null);
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