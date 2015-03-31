namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="ConfigurationManagerStore"/>
    ///   class is used to provide a 
    ///   <see cref="IConfigurationStore"/> implementation based on the 
    ///   <see cref="ConfigurationManager"/> class.
    /// </summary>
    public class ConfigurationManagerStore : IConfigurationStore
    {
        /// <summary>
        ///   Defines the set of types supported by the <see cref = "Convert" /> class.
        /// </summary>
        private static readonly List<Type> _supportedConvertTypes = new List<Type>
                                                                    {
                                                                        typeof(Object), 
                                                                        typeof(DBNull), 
                                                                        typeof(Boolean), 
                                                                        typeof(Char), 
                                                                        typeof(SByte), 
                                                                        typeof(Byte), 
                                                                        typeof(Int16), 
                                                                        typeof(UInt16), 
                                                                        typeof(Int32), 
                                                                        typeof(UInt32), 
                                                                        typeof(Int64), 
                                                                        typeof(UInt64), 
                                                                        typeof(Single), 
                                                                        typeof(Double), 
                                                                        typeof(Decimal), 
                                                                        typeof(DateTime), 
                                                                        typeof(String)
                                                                    };

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConfigurationManagerStore" /> class.
        /// </summary>
        public ConfigurationManagerStore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManagerStore"/> class.
        /// </summary>
        /// <param name="cacheStore">
        /// The cache store.
        /// </param>
        public ConfigurationManagerStore(ICacheStore cacheStore)
            : this(cacheStore, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManagerStore"/> class.
        /// </summary>
        /// <param name="cacheStore">
        /// The cache store.
        /// </param>
        /// <param name="slidingExpiration">
        /// The sliding expiration.
        /// </param>
        public ConfigurationManagerStore(ICacheStore cacheStore, TimeSpan slidingExpiration)
        {
            CacheStore = cacheStore;
            SlidingExpiration = slidingExpiration;
        }

        /// <summary>
        /// Gets the application setting for the provided key and default value.
        /// </summary>
        /// <typeparam name="T">The type of setting value.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public T GetApplicationSetting<T>(String key)
        {
            Func<T> configurationResolver = () => LoadConfigurationValue<T>(key);

            if (CacheStore == null)
            {
                return configurationResolver();
            }

            Func<IExpirationPolicy[]> policyGenerator = null;

            if (SlidingExpiration > TimeSpan.Zero)
            {
                policyGenerator = () => new[]
                                        {
                                            new SlidingExpirationPolicy(SlidingExpiration)
                                        };
            }

            return CacheStore.GetItem(key, configurationResolver, policyGenerator);
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
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="key"/> is <c>null</c> or equals <see cref="String.Empty"/>.
        /// </exception>
        public ConnectionStringSettings GetConnectionSetting(String key)
        {
            return ConfigurationManager.ConnectionStrings[key];
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
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="sectionName"/> is <c>null</c> or equals <see cref="String.Empty"/>.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// The section defined in configuration is not the same type as <typeparamref name="T"/>.
        /// </exception>
        public T GetSection<T>(String sectionName) where T : ConfigurationSection
        {
            ConfigurationSection section = ConfigurationManager.GetSection(sectionName) as ConfigurationSection;

            if (section == null)
            {
                return null;
            }

            T typedSection = section as T;

            if (typedSection == null)
            {
                throw new ConfigurationErrorsException(
                    String.Format(
                        CultureInfo.InvariantCulture, 
                        Resources.InvalidConfigurationSectionTypeExceptionMessage, 
                        sectionName, 
                        typeof(T).FullName, 
                        section.GetType().FullName));
            }

            return typedSection;
        }

        /// <summary>
        /// Loads the configuration value.
        /// </summary>
        /// <typeparam name="T">The type of value to load.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        private static T LoadConfigurationValue<T>(String key)
        {
            String configurationValue = ConfigurationManager.AppSettings[key];

            Type returnType = typeof(T);

            if (returnType.Equals(typeof(String)))
            {
                return (T)(Object)configurationValue;
            }

            if (configurationValue == null)
            {
                // We probably can't convert null to the required type
                // We need to return the default value here to avoid type cast failures
                return default(T);
            }

            if (_supportedConvertTypes.Contains(returnType))
            {
                return (T)Convert.ChangeType(configurationValue, returnType, CultureInfo.CurrentCulture);
            }

            // Get the type converter for the type to return
            TypeConverter converter = TypeDescriptor.GetConverter(returnType);

            // Convert from a string to the type required
            return (T)converter.ConvertFromString(configurationValue);
        }

        /// <summary>
        ///   Gets or sets the cache store.
        /// </summary>
        /// <value>
        ///   The cache store.
        /// </value>
        private ICacheStore CacheStore
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the sliding expiration.
        /// </summary>
        /// <value>
        ///   The sliding expiration.
        /// </value>
        private TimeSpan SlidingExpiration
        {
            get;
            set;
        }
    }
}