namespace Neovolve.Toolkit.UnitTests
{
    using System;
    using System.Configuration;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="ConfigurationStoreMockWrapper"/>
    ///   class is used to provide a concrete type that wraps a mock of
    ///   <see cref="IConfigurationStore"/>.
    /// </summary>
    public class ConfigurationStoreMockWrapper : IConfigurationStore
    {
        /// <summary>
        /// Gets the application setting for the provided key.
        /// </summary>
        /// <typeparam name="T">
        /// The type of setting value.
        /// </typeparam>
        /// <param name="key">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> value.
        /// </returns>
        public T GetApplicationSetting<T>(String key)
        {
            return MockInstance.GetApplicationSetting<T>(key);
        }

        /// <summary>
        /// Gets the connection setting for the provided key.
        /// </summary>
        /// <param name="key">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// A <see cref="ConnectionStringSettings"/> instance or <c>null</c> if no configuration is found.
        /// </returns>
        public ConnectionStringSettings GetConnectionSetting(String key)
        {
            return MockInstance.GetConnectionSetting(key);
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value returned.
        /// </typeparam>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> value found in configuration.
        /// </returns>
        public T GetSection<T>(String sectionName) where T : ConfigurationSection
        {
            return MockInstance.GetSection<T>(sectionName);
        }

        /// <summary>
        ///   Gets or sets the mock instance.
        /// </summary>
        /// <value>
        ///   The mock instance.
        /// </value>
        public static IConfigurationStore MockInstance
        {
            get;
            set;
        }
    }
}