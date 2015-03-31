namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity.Properties;

    /// <summary>
    /// The <see cref="AppSettingsParameterValueElement"/>
    ///   class is used to configure a Unity injection parameter value to be determined from an AppSettings value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="TypeConverter"/> returned from <see cref="TypeDescriptor.GetConverter(Type)">TypeDescriptor.GetConverter(Type)</see> 
    ///     will be used if the type of parameter is not a <see cref="String"/> and no value has been defined for
    ///     <see cref="TypeConverterTypeName"/>.
    ///   </para>
    /// <para>
    /// The <see cref="AppSettingsParameterValueElement"/> is configured for a container by the <see cref="SectionExtensionInitiator"/> class.
    ///   </para>
    /// <note>
    /// The <see cref="SectionExtensionInitiator"/> class must be configured for the container in order for the appSetting injection element
    ///     to be understood.
    ///   </note>
    /// </remarks>
    /// <example>
    /// <para>
    /// The following example shows how the element is configured and used without a type converter. 
    ///     It implicitly uses the type converter for <see cref="Int64"/> to convert the AppSetting value to the correct type
    ///     before it is injected into the constructor.
    ///   </para>
    /// <code lang="xml" title="Application configuration without type converter">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <appSettings>
    ///     <add key="MyTestSetting"
    ///          value="234234"/>
    ///   </appSettings>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IDoSomething, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.CachedSomethingDone, Neovolve.Toolkit.Unity.IntegrationTests">
    ///           <constructor>
    ///             <param name="dependency">
    ///               <dependency name="CacheSomething"/>
    ///             </param>
    ///             <param name="maxAgeInMilliseconds"
    ///                    type="System.Int64, mscorlib, Version=2.0.0.0">
    ///               <appSetting appSettingKey="MyTestSetting"/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IDoSomething, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.SomethingDone, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   name="CacheSomething"/> 
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// <para>
    /// The following example demonstrates how the same scenario can be achieved by specifying the type converter to use. 
    ///   </para>
    /// <code lang="xml" title="Application configuration with a type converter defined">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <appSettings>
    ///     <add key="MyTestSetting"
    ///          value="234234"/>
    ///   </appSettings>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IDoSomething, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.CachedSomethingDone, Neovolve.Toolkit.Unity.IntegrationTests">
    ///           <constructor>
    ///             <param name="dependency">
    ///               <dependency name="CacheSomething"/>
    ///             </param>
    ///             <param name="maxAgeInMilliseconds"
    ///                    type="System.Int64, mscorlib, Version=2.0.0.0">
    ///               <appSetting appSettingKey="MyTestSetting"
    ///                           typeConverter="System.ComponentModel.Int64Converter, System, Version=2.0.0.0"/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IDoSomething, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.SomethingDone, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   name="CacheSomething"/> 
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// </example>
    /// <seealso cref="SectionExtensionInitiator"/>
    public class AppSettingsParameterValueElement : ParameterValueElement
    {
        /// <summary>
        ///   Defines the attribute name for the <see cref = "AppSettingKey" /> property.
        /// </summary>
        public const String AppSettingKeyAttributeName = "appSettingKey";

        /// <summary>
        ///   Defines the configuration element name used to identify this type of parameter value element.
        /// </summary>
        public const String ElementName = "appSetting";

        /// <summary>
        ///   Defines the attribute name for the <see cref = "TypeConverterTypeName" /> property.
        /// </summary>
        public const String TypeConverterAttributeName = "typeConverter";

        /// <summary>
        ///   Stores the configuration store.
        /// </summary>
        private readonly IConfigurationStore _config;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AppSettingsParameterValueElement" /> class.
        /// </summary>
        public AppSettingsParameterValueElement()
        {
            _config = ConfigurationStoreFactory.Create();
        }

        /// <summary>
        /// Creates the value.
        /// </summary>
        /// <param name="parameterType">
        /// Type of the parameter.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter")]
        public Object CreateValue(Type parameterType)
        {
            Contract.Requires<ArgumentNullException>(parameterType != null);

            Contract.Assume(String.IsNullOrWhiteSpace(AppSettingKey) == false);
            String configurationValue = Config.GetApplicationSetting<String>(AppSettingKey);
            Object injectionValue;

            if (parameterType == typeof(String))
            {
                injectionValue = configurationValue;
            }
            else
            {
                TypeConverter converter = GetTypeConverter(parameterType);

                try
                {
                    injectionValue = converter.ConvertFromInvariantString(configurationValue);
                }
                catch (NotSupportedException ex)
                {
                    String settingValue = configurationValue ?? Resources.NullValueMarker;

                    String failureMessage = String.Format(
                        CultureInfo.InvariantCulture, 
                        Resources.AppSettingsParameterValueElement_CannotConvertAppSettingValue, 
                        AppSettingKey, 
                        settingValue, 
                        parameterType.FullName);

                    throw new ConfigurationErrorsException(failureMessage, ex);
                }
            }

            return injectionValue;
        }

        /// <summary>
        /// Generate an <see cref="T:Microsoft.Practices.Unity.InjectionParameterValue"/> object
        ///   that will be used to configure the container for a type registration.
        /// </summary>
        /// <param name="container">
        /// Container that is being configured. Supplied in order
        ///   to let custom implementations retrieve services; do not configure the container
        ///   directly in this method.
        /// </param>
        /// <param name="parameterType">
        /// Type of the.
        /// </param>
        /// <returns>
        /// An <see cref="InjectionParameterValue"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="parameterType"/> value is <c>null</c>.
        /// </exception>
        public override InjectionParameterValue GetInjectionParameterValue(IUnityContainer container, Type parameterType)
        {
            if (parameterType == null)
            {
                throw new ArgumentNullException("parameterType");
            }

            Object injectionValue = CreateValue(parameterType);

            return new InjectionParameter(parameterType, injectionValue);
        }

        /// <summary>
        /// Gets the type converter.
        /// </summary>
        /// <param name="parameterType">
        /// Type of the parameter.
        /// </param>
        /// <returns>
        /// A <see cref="TypeConverter"/> instance.
        /// </returns>
        private TypeConverter GetTypeConverter(Type parameterType)
        {
            Contract.Requires<ArgumentNullException>(parameterType != null);

            if (String.IsNullOrEmpty(TypeConverterTypeName) == false)
            {
                Type converterType = Type.GetType(TypeConverterTypeName);

                if (converterType == null)
                {
                    String message = String.Format(
                        CultureInfo.InvariantCulture, Resources.AppSettingsParameterValueElement_TypeLoadFailure, TypeConverterTypeName);

                    throw new ConfigurationErrorsException(message);
                }

                if (typeof(TypeConverter).IsAssignableFrom(converterType) == false)
                {
                    String message = String.Format(
                        CultureInfo.InvariantCulture, 
                        Resources.AppSettingsParameterValueElement_InvalidType, 
                        TypeConverterTypeName, 
                        typeof(TypeConverter).FullName);

                    throw new ConfigurationErrorsException(message);
                }

                return (TypeConverter)Activator.CreateInstance(converterType);
            }

            return TypeDescriptor.GetConverter(parameterType);
        }

        /// <summary>
        /// Defines the contracts that ensure the state of this instance is correct.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_config != null);
        }

        /// <summary>
        ///   Gets or sets the app setting key.
        /// </summary>
        /// <value>
        ///   The app setting key.
        /// </value>
        [ConfigurationProperty(AppSettingKeyAttributeName, IsRequired = true)]
        public String AppSettingKey
        {
            get
            {
                return (String)base[AppSettingKeyAttributeName];
            }

            set
            {
                base[AppSettingKeyAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the type converter type.
        /// </summary>
        /// <value>
        ///   The name of the type converter type.
        /// </value>
        [ConfigurationProperty(TypeConverterAttributeName, IsRequired = false, DefaultValue = null)]
        public String TypeConverterTypeName
        {
            get
            {
                return (String)base[TypeConverterAttributeName];
            }

            set
            {
                base[TypeConverterAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets the config.
        /// </summary>
        /// <value>
        ///   The config.
        /// </value>
        private IConfigurationStore Config
        {
            get
            {
                Contract.Ensures(_config != null);

                return _config;
            }
        }
    }
}