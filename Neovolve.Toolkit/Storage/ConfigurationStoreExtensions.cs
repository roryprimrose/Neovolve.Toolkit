namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="ConfigurationStoreExtensions"/>
    ///   class is used to define extension methods for the <see cref="IConfigurationStore"/> interface.
    /// </summary>
    public static class ConfigurationStoreExtensions
    {
        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value to resolve.
        /// </typeparam>
        /// <param name="store">
        /// The configuration store.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <param name="isRequired">
        /// if set to <c>true</c> then a configuration value must be defined; otherwise <c>false</c>.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> value, or <paramref name="defaultValue"/> if no configuration is found.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public static T GetApplicationSetting<T>(this IConfigurationStore store, String key, T defaultValue, Boolean isRequired)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            T configurationValue = store.GetApplicationSetting<T>(key);
            Boolean isDefaultValue;

            if (typeof(T).IsValueType)
            {
                isDefaultValue = configurationValue.Equals(default(T));
            }
            else
            {
                isDefaultValue = configurationValue == null;
            }

            if (isDefaultValue == false)
            {
                return configurationValue;
            }

            // There is no configured value
            if (isRequired)
            {
                String message = String.Format(CultureInfo.CurrentCulture, Resources.ConfigurationManagerStore_RequiredValueMissing, key);

                throw new ConfigurationErrorsException(message);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <typeparam name="T">
        /// The type of setting value.
        /// </typeparam>
        /// <param name="store">
        /// The configuration store.
        /// </param>
        /// <param name="key">
        /// The configuration key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public static T GetApplicationSetting<T>(this IConfigurationStore store, String key, T defaultValue)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return store.GetApplicationSetting(key, defaultValue, false);
        }
    }
}