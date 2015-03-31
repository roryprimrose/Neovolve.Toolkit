namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System.Configuration;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="ConfigurationSectionStub"/>
    ///   class is a stub used to test the <see cref="ConfigurationResolver"/> class.
    /// </summary>
    internal class ConfigurationSectionStub : ConfigurationSection
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConfigurationSectionStub" /> class.
        /// </summary>
        public ConfigurationSectionStub()
        {
            Sources = new ConfigurationElementCollectionStub();
        }

        /// <summary>
        ///   Gets or sets the sources.
        /// </summary>
        /// <value>
        ///   The sources.
        /// </value>
        public ConfigurationElementCollectionStub Sources
        {
            get;
            set;
        }
    }
}