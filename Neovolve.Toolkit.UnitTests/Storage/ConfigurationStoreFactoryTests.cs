namespace Neovolve.Toolkit.UnitTests.Storage
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="ConfigurationStoreFactoryTests"/>
    ///   class is used to test the <see cref="ConfigurationStoreFactory"/> class.
    /// </summary>
    [TestClass]
    public class ConfigurationStoreFactoryTests
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
        /// Runs test for assigning A store type that is not I configuration store type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void AssigningAStoreTypeThatIsNotIConfigurationStoreTypeThrowsExceptionTest()
        {
            ConfigurationStoreFactory.StoreType = typeof(NotSupportedException);
        }

        /// <summary>
        /// Runs test for can assign A store type that is I configuration store type.
        /// </summary>
        [TestMethod]
        public void CanAssignAStoreTypeThatIsIConfigurationStoreTypeTest()
        {
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationManagerStore);
        }

        /// <summary>
        /// Runs test for can set configuration store type to null.
        /// </summary>
        [TestMethod]
        public void CanSetConfigurationStoreTypeToNullTest()
        {
            ConfigurationStoreFactory.StoreType = null;
        }

        /// <summary>
        /// Runs test for configuration store type returns an instance.
        /// </summary>
        [TestMethod]
        public void ConfigurationStoreTypeReturnsAnIConfigurationStoreTypeTest()
        {
            Type actual = ConfigurationStoreFactory.StoreType;

            Assert.IsNotNull(actual, "StoreType failed to return an instance");
            Assert.IsTrue(typeof(IConfigurationStore).IsAssignableFrom(actual), "StoreType returned an unsupported type");
        }

        /// <summary>
        /// Runs test for create returns configuration store instance.
        /// </summary>
        [TestMethod]
        public void CreateReturnsConfigurationStoreInstanceTest()
        {
            IConfigurationStore actual = ConfigurationStoreFactory.Create();

            Assert.IsNotNull(actual, "Create failed to return an instance");
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