namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Configuration;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The channel configuration store.
    /// </summary>
    public class ChannelConfigurationStore : IConfigurationStore
    {
        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public T GetApplicationSetting<T>(String key)
        {
            return default(T);
        }

        /// <summary>
        /// Gets the connection setting for the provided key.
        /// </summary>
        /// <param name="key">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// A <see cref="System.Configuration.ConnectionStringSettings"/> instance or <c>null</c> if no configuration is found.
        /// </returns>
        public ConnectionStringSettings GetConnectionSetting(String key)
        {
            return null;
        }

        /// <summary>
        /// Gets the configuration section for the provided section name.
        /// </summary>
        /// <typeparam name="T">
        /// The type of configuration section.
        /// </typeparam>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance or <c>null</c> if the section is not defined in configuration.
        /// </returns>
        public T GetSection<T>(String sectionName) where T : ConfigurationSection
        {
            return (T)Section;
        }

        /// <summary>
        ///   Gets or sets the section.
        /// </summary>
        /// <value>
        ///   The section.
        /// </value>
        public static ConfigurationSection Section
        {
            get;
            set;
        }
    }
}