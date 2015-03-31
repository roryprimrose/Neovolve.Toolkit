namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System.Configuration;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="AppSettingsParameterValueElementTests"/>
    ///   class is used to test the <see cref="AppSettingsParameterValueElement"/> class.
    /// </summary>
    [TestClass]
    public class AppSettingsParameterValueElementTests
    {
        /// <summary>
        /// Runs test for app settings parameter value element injects app setting.
        /// </summary>
        [TestMethod]
        public void AppSettingsParameterValueElementInjectsAppSettingTest()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);

            using (UnityContainer container = new UnityContainer())
            {
                section.Configure(container);

                CachedSomethingDone something = (CachedSomethingDone)container.Resolve<IDoSomething>();

                Assert.IsNotNull(something, "Instance was not resolved from container");
                Assert.AreEqual(234234, something.MaxAgeInMilliseconds, "AppSetting was not injected correctly.");
                Assert.AreEqual("Test string value", something.PropertyTest, "Property injection was not correctly configured.");
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