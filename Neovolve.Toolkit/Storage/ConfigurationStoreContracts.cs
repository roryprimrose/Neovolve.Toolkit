namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ConfigurationStoreContracts"/>
    ///   class is used to define the code contracts for the <see cref="IConfigurationStore"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IConfigurationStore))]
    internal abstract class ConfigurationStoreContracts : IConfigurationStore
    {
        /// <summary>
        /// Gets the application setting for the provided key and default value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of setting value.
        /// </typeparam>
        /// <param name="key">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public T GetApplicationSetting<T>(String key)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return default(T);
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
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

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
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(sectionName) == false);

            return default(T);
        }
    }
}