namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="ConfigurationElementCollectionStub"/>
    ///   class is used to test the <see cref="ConfigurationResolver"/> class.
    /// </summary>
    internal class ConfigurationElementCollectionStub : ConfigurationElementCollection
    {
        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        public void Add(ConfigurationElementStub element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigurationElementStub();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">
        /// The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override Object GetElementKey(ConfigurationElement element)
        {
            ConfigurationElementStub stub = element as ConfigurationElementStub;

            Debug.Assert(stub != null, "Invalid element encountered");

            return stub.Name;
        }
    }
}