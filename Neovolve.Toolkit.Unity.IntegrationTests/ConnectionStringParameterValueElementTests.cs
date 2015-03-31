namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;
    using System.Configuration;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="ConnectionStringParameterValueElementTests"/>
    ///   class is used to test the <see cref="ConnectionStringParameterValueElement"/> class.
    /// </summary>
    [TestClass]
    public class ConnectionStringParameterValueElementTests
    {
        /// <summary>
        /// Runs test for connection string parameter value element injects connection setting.
        /// </summary>
        [TestMethod]
        public void ConnectionStringParameterValueElementInjectsConnectionSettingsTest()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            const String Expected = "Data Source=localhost;Database=SomeDatabase;Integrated Security=SSPI;";

            using (UnityContainer container = new UnityContainer())
            {
                section.Configure(container);

                ConnectionTest something = (ConnectionTest)container.Resolve<IDoSomething>("ConnectionSettingTesting");

                Assert.IsNotNull(something, "Instance was not resolved from container");
                Assert.IsNotNull(something.ConnectionSetting, "ConnectionSetting returned an incorrect value");
                Assert.AreEqual(something.ConnectionSetting.ConnectionString, Expected, "ConnectionString was not injected correctly.");
            }
        }

        /// <summary>
        /// Runs test for connection string parameter value element injects connection string.
        /// </summary>
        [TestMethod]
        public void ConnectionStringParameterValueElementInjectsConnectionStringTest()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            const String Expected = "Data Source=localhost;Database=SomeDatabase;Integrated Security=SSPI;";

            using (UnityContainer container = new UnityContainer())
            {
                section.Configure(container);

                ConnectionTest something = (ConnectionTest)container.Resolve<IDoSomething>("ConnectionStringTesting");

                Assert.IsNotNull(something, "Instance was not resolved from container");
                Assert.AreEqual(something.Execute(), Expected, "ConnectionString was not injected correctly.");
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