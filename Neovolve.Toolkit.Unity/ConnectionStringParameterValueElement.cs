namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity.Properties;

    /// <summary>
    /// The <see cref="ConnectionStringParameterValueElement"/>
    ///   class is used to configure a Unity injection parameter value to be determined from a <see cref="ConnectionStringSettings"/> value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ConnectionStringParameterValueElement"/> is configured for a container by the <see cref="SectionExtensionInitiator"/> class.
    ///     It only supports injection of <see cref="String"/>
    ///     and <see cref="ConnectionStringSettings"/> parameter types.
    ///   </para>
    /// <note>
    /// The <see cref="SectionExtensionInitiator"/> class must be configured for the container in order for the connectionSetting injection element
    ///     to be understood.
    ///   </note>
    /// <para>
    /// Injecting a connection string value should use a string parameter as the injection value. 
    ///     This will then decouple the dependency being created from the System.Configuration assembly. 
    ///     The only time that the ConnectionStringSettings class should be used as the injection parameter type is when the provider information 
    ///     on the connection setting is required for some application logic.
    ///   </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The following example shows how the element is configured. 
    ///   </para>
    /// <code lang="xml" title="Application configuration">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <connectionStrings>
    ///     <add name="TestConnection" connectionString="Data Source=localhost;Database=SomeDatabase;Integrated Security=SSPI;"/>
    ///   </connectionStrings>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IDoSomething, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.ConnectionTest, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   name="ConnectionSettingTesting">
    ///           <constructor>
    ///             <param name="connectionSetting">
    ///               <connectionSetting connectionStringKey="TestConnection"/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// </example>
    /// <seealso cref="SectionExtensionInitiator"/>
    public class ConnectionStringParameterValueElement : ParameterValueElement
    {
        /// <summary>
        ///   Defines the attribute name for the <see cref = "ConnectionStringKey" /> property.
        /// </summary>
        public const String ConnectionStringKeyAttributeName = "connectionStringKey";

        /// <summary>
        ///   Defines the configuration element name used to identify this type of parameter value element.
        /// </summary>
        public const String ElementName = "connectionSetting";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConnectionStringParameterValueElement" /> class.
        /// </summary>
        public ConnectionStringParameterValueElement()
        {
            Config = ConfigurationStoreFactory.Create();
        }

        /// <summary>
        /// Creates the value.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public ConnectionStringSettings CreateValue()
        {
            Contract.Assume(String.IsNullOrWhiteSpace(ConnectionStringKey) == false);
            ConnectionStringSettings configurationValue = Config.GetConnectionSetting(ConnectionStringKey);

            if (configurationValue == null)
            {
                String message = String.Format(
                    CultureInfo.InvariantCulture, Resources.ConnectionStringParameterValueElement_ConnectionStringKeyNotFound, ConnectionStringKey);

                throw new ConfigurationErrorsException(message);
            }

            return configurationValue;
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
        public override InjectionParameterValue GetInjectionParameterValue(IUnityContainer container, Type parameterType)
        {
            if (parameterType == null)
            {
                throw new ArgumentNullException("parameterType");
            }

            ConnectionStringSettings injectionValue = CreateValue();

            if (parameterType.Equals(typeof(String)))
            {
                return new InjectionParameter(parameterType, injectionValue.ConnectionString);
            }

            if (parameterType.Equals(typeof(ConnectionStringSettings)))
            {
                return new InjectionParameter(parameterType, injectionValue);
            }

            String message = String.Format(
                CultureInfo.InvariantCulture, Resources.ConnectionStringParameterValueElement_InvalidParameterType, parameterType.FullName);

            throw new InvalidOperationException(message);
        }

        /// <summary>
        ///   Gets or sets the connection string key.
        /// </summary>
        /// <value>
        ///   The connection string key.
        /// </value>
        [ConfigurationProperty(ConnectionStringKeyAttributeName, IsRequired = true)]
        public String ConnectionStringKey
        {
            get
            {
                return (String)base[ConnectionStringKeyAttributeName];
            }

            set
            {
                base[ConnectionStringKeyAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the config.
        /// </summary>
        /// <value>
        ///   The config.
        /// </value>
        private IConfigurationStore Config
        {
            get;
            set;
        }
    }
}