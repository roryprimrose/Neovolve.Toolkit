namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="ConfigurationTypeLoader"/>
    ///   class is used to load a type from a configuration value.
    /// </summary>
    internal static class ConfigurationTypeLoader
    {
        /// <summary>
        /// Determines the type of configuration store to create.
        /// </summary>
        /// <param name="configurationStore">
        /// The configuration store.
        /// </param>
        /// <param name="configurationKey">
        /// The configuration key.
        /// </param>
        /// <param name="assignableToType">
        /// The assignable to type.
        /// </param>
        /// <param name="defaultType">
        /// The default type.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// Failed to load the type defined in configuration.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The type defined in configuration is an invalid type.
        /// </exception>
        public static Type DetermineStoreType(
            IConfigurationStore configurationStore, String configurationKey, Type assignableToType, Type defaultType)
        {
            Contract.Requires<ArgumentNullException>(configurationStore != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(configurationKey) == false);
            Contract.Requires<ArgumentNullException>(assignableToType != null);
            Contract.Requires<ArgumentNullException>(defaultType != null);

            String storeTypeName = configurationStore.GetApplicationSetting<String>(configurationKey);

            if (String.IsNullOrEmpty(storeTypeName) == false)
            {
                // Attempt to load this type
                Type storeType = Type.GetType(storeTypeName);

                if (storeType == null)
                {
                    throw new TypeLoadException(
                        String.Format(
                            CultureInfo.InvariantCulture, 
                            Resources.TypeLoadFromConfigurationKeyFailed, 
                            storeTypeName, 
                            configurationKey, 
                            assignableToType.FullName));
                }

                // Check that the type loaded is the correct type
                if (assignableToType.IsAssignableFrom(storeType) == false)
                {
                    throw new InvalidCastException(
                        String.Format(
                            CultureInfo.InvariantCulture, 
                            Resources.InvalidTypeLoadFromConfigurationFailed, 
                            storeType.FullName, 
                            configurationKey, 
                            assignableToType.FullName));
                }

                return storeType;
            }

            return defaultType;
        }
    }
}