namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Configuration;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="ConfigurationElementStub"/>
    ///   class is used to test the <see cref="ConfigurationResolver"/> class.
    /// </summary>
    internal class ConfigurationElementStub : ConfigurationElement
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConfigurationElementStub" /> class.
        /// </summary>
        public ConfigurationElementStub()
        {
            Name = Guid.NewGuid().ToString();
        }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>
        ///   The name of the element.
        /// </value>
        public String Name
        {
            get;
            set;
        }
    }
}