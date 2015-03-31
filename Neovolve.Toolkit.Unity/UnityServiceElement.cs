namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Configuration;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="UnityServiceElement"/>
    ///   class is used to provide configuration support for defining a unity container via a service behavior.
    /// </summary>
    /// <remarks>
    /// <note>
    /// Use Neovolve.Toolkit.Server.Unity.UnityServiceHostFactory if custom unity configuration section names or defined unity container names are not required.
    ///     UnityServiceHostFactory has less impact on configuration compared to defining the configuration to use this custom behavior element.
    ///   </note>
    /// <para>
    /// The <see cref="UnityServiceElement"/> allows for custom Unity configuration section names and defined Unity container names to be used
    ///     when creating service instances with Unity.
    ///   </para>
    /// <para>
    /// The <see cref="UnitySectionName"/>, <see cref="UnityContainerName"/> and <see cref="UnityResolveName"/> properties are optional. 
    ///     The value of <c>unity</c> will be used if no value is provided for <see cref="UnitySectionName"/>.
    ///     The default container will be resolved if no value is provided for <see cref="UnityContainerName"/>.
    ///   </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The following configuration defines a custom unity configuration section name and defined unity container name to create a WCF service instance.
    ///   </para>
    /// <code lang="xml" title="Example of web.config">
    /// <![CDATA[
    /// <?xml version="1.0" ?>
    /// <configuration>
    ///     <configSections>
    ///         <section name="customUnitySection"
    ///                  type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///     </configSections>
    ///     <customUnitySection>
    ///         <containers>
    ///             <container name="customContainer">
    ///                 <register type="Neovolve.Toolkit.Unity.WebIntegrationTests.TestService, Neovolve.Toolkit.Unity.WebIntegrationTests"
    ///                     name="namedResolution">
    ///                     <constructor>
    ///                         <param name="hashCalculator">
    ///                             <dependency/>
    ///                         </param>
    ///                     </constructor>
    ///                 </register>
    ///                 <register type="System.Security.Cryptography.HashAlgorithm, mscorlib"
    ///                     mapTo="System.Security.Cryptography.SHA256CryptoServiceProvider, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"/>
    ///                 <extensions>
    ///                     <add type="Neovolve.Toolkit.Unity.DisposableStrategyExtension, Neovolve.Toolkit.Unity"/>
    ///                 </extensions>
    ///             </container>
    ///         </containers>
    ///     </customUnitySection>
    ///     <system.serviceModel>
    ///         <behaviors>
    ///             <serviceBehaviors>
    ///                 <behavior name="UnityBehavior">
    ///                     <unityService unitySectionName="customUnitySection"
    ///                                   unityContainerName="customContainer"
    ///                                   unityResolveName="namedResolution"/>
    ///                 </behavior>
    ///             </serviceBehaviors>
    ///         </behaviors>
    ///         <extensions>
    ///             <behaviorExtensions>
    ///                 <add name="unityService"
    ///                      type="Neovolve.Toolkit.Unity.UnityServiceElement, Neovolve.Toolkit.Unity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=911824a9aa319cb2"/>
    ///             </behaviorExtensions>
    ///         </extensions>
    ///         <services>
    ///             <service behaviorConfiguration="UnityBehavior"
    ///                      name="Neovolve.Toolkit.Unity.WebIntegrationTests.TestService">
    ///                 <endpoint address=""
    ///                           binding="basicHttpBinding"
    ///                           bindingConfiguration=""
    ///                           contract="Neovolve.Toolkit.Unity.WebIntegrationTests.ITestService"/>
    ///             </service>
    ///         </services>
    ///     </system.serviceModel>
    /// </configuration> 
    /// ]]>
    ///   </code>
    /// </example>
    public class UnityServiceElement : BehaviorExtensionElement
    {
        /// <summary>
        ///   Defines the attribute name of the unity container name configuration.
        /// </summary>
        public const String UnityContainerNameAttributeName = "unityContainerName";

        /// <summary>
        ///   Defines the attribute name of the unity resolution name configuration.
        /// </summary>
        public const String UnityResolveNameAttributeName = "unityResolveName";

        /// <summary>
        ///   Defines the attribute name of the unity section name configuration.
        /// </summary>
        public const String UnitySectionNameAttributeName = "unitySectionName";

        /// <summary>
        ///   Stores the set of properties for the element.
        /// </summary>
        private ConfigurationPropertyCollection _properties;

        /// <summary>
        /// Copies the content of the specified configuration element to this configuration element.
        /// </summary>
        /// <param name="from">
        /// The configuration element to be copied.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="from"/> is null.
        /// </exception>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// The configuration file is read-only.
        /// </exception>
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            UnityServiceElement element = from as UnityServiceElement;

            if (element == null)
            {
                return;
            }

            UnitySectionName = element.UnitySectionName;
            UnityContainerName = element.UnityContainerName;
            UnityResolveName = element.UnityResolveName;
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override Object CreateBehavior()
        {
            IUnityContainer container = UnityContainerResolver.Resolve(null, UnitySectionName, UnityContainerName);

            return new UnityServiceBehavior(container, UnityResolveName);
        }

        /// <summary>
        ///   Gets the type of behavior.
        /// </summary>
        /// <value>
        ///   The behavior type.
        /// </value>
        /// <returns>
        ///   A <see cref = "T:System.Type" />.
        /// </returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(UnityServiceBehavior);
            }
        }

        /// <summary>
        ///   Gets or sets the name of the unity container.
        /// </summary>
        /// <remarks>
        ///   This property is optional. The default container will be resolved if no value is specified.
        /// </remarks>
        /// <value>
        ///   The name of the unity container.
        /// </value>
        [ConfigurationProperty(UnityContainerNameAttributeName)]
        public String UnityContainerName
        {
            get
            {
                return (String)base[UnityContainerNameAttributeName];
            }

            set
            {
                base[UnityContainerNameAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the unity resolve action.
        /// </summary>
        /// <value>
        ///   The name of the unity resolve action.
        /// </value>
        [ConfigurationProperty(UnityResolveNameAttributeName)]
        public String UnityResolveName
        {
            get
            {
                return (String)base[UnityResolveNameAttributeName];
            }

            set
            {
                base[UnityResolveNameAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the unity section.
        /// </summary>
        /// <remarks>
        ///   This property is optional. The value of <c>unity</c> will be used if no value is defined.
        /// </remarks>
        /// <value>
        ///   The name of the unity section.
        /// </value>
        [ConfigurationProperty(UnitySectionNameAttributeName)]
        public String UnitySectionName
        {
            get
            {
                return (String)base[UnitySectionNameAttributeName];
            }

            set
            {
                base[UnitySectionNameAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets the collection of properties.
        /// </summary>
        /// <value>
        ///   The properties.
        /// </value>
        /// <returns>
        ///   The <see cref = "ConfigurationPropertyCollection" /> of properties for the element.
        /// </returns>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (_properties == null)
                {
                    ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

                    properties.Add(
                        new ConfigurationProperty(
                            UnitySectionNameAttributeName, typeof(String), String.Empty, null, null, ConfigurationPropertyOptions.None));

                    properties.Add(
                        new ConfigurationProperty(
                            UnityContainerNameAttributeName, typeof(String), String.Empty, null, null, ConfigurationPropertyOptions.None));

                    properties.Add(
                        new ConfigurationProperty(
                            UnityResolveNameAttributeName, typeof(String), String.Empty, null, null, ConfigurationPropertyOptions.None));

                    _properties = properties;
                }

                return _properties;
            }
        }
    }
}