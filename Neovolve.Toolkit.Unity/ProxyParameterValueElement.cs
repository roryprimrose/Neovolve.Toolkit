namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Configuration;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ProxyParameterValueElement"/>
    ///   class is used to configure a Unity parameter value to be determined from a proxy value created by <see cref="ProxyManager{T}"/>.
    /// </summary>
    /// <remarks>
    /// A Unity build operation will be performed on named resolutions of <see cref="ProxyManager{T}"/> if a value has been provided for <see cref="Name"/>.
    ///   A search will be performed for unnamed type registrations on the container for <see cref="ProxyManager{T}"/> and then <see cref="ProxyHandler{T}"/>
    ///   where no name is provided. A build operation that uses the <see cref="ProxyManager{T}(ProxyHandler{T})"/>
    ///   constructor will be performed if a registration for either of these types is found. A build operation for the
    ///   <see cref="ProxyManager{T}()"/> constructor will be performed if none of these registrations can be found.
    /// </remarks>
    /// <example>
    /// <para>
    /// The following example demonstrates a simple usage of proxy injection that uses the default <see cref="ProxyManager{T}"/> constructor.
    ///   </para>
    /// <code lang="xml" title="Application configuration for simple proxy injection">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <extensions>
    ///           <add type="Neovolve.Toolkit.Unity.DisposableStrategyExtension, Neovolve.Toolkit.Unity"/>
    ///         </extensions>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.Root, Neovolve.Toolkit.Unity.IntegrationTests">
    ///           <constructor>
    ///             <param name="tester">
    ///               <proxy/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// <para>
    /// The following example demonstrates a named resolution of <see cref="ProxyManager{T}"/> that results in using the 
    ///     <see cref="ProxyManager{T}(ProxyHandler{T})"/> constructor.
    ///   </para>
    /// <code lang="xml" title="Application configuration for named proxy injection">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <extensions>
    ///           <add type="Neovolve.Toolkit.Unity.DisposableStrategyExtension, Neovolve.Toolkit.Unity"/>
    ///         </extensions>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.ISecondDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.SecondDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   name="NamedProxyTest">
    ///           <constructor>
    ///             <param name="third">
    ///               <!-- 
    ///                   The parameter third on SecondDisposable is of type IThirdDisposable 
    ///                   The resolution here will create ProxyManager<IThirdDisposable> and return a proxy of this type
    ///               -->
    ///               <proxy name="NamedProxy"/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Communication.ProxyManager`1, Neovolve.Toolkit"
    ///                   name="NamedProxy">
    ///           <constructor>
    ///             <param name="proxyHandler">
    ///               <dependency name="NamedProxyHandler"/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Communication.ProxyHandler`1, Neovolve.Toolkit"
    ///                   mapTo="Neovolve.Toolkit.Communication.DefaultProxyHandler`1, Neovolve.Toolkit"
    ///                   name="NamedProxyHandler">
    ///           <constructor>
    ///             <param name="target">
    ///               <dependency/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IThirdDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.ThirdDisposable, Neovolve.Toolkit.Unity.IntegrationTests">
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IFourthDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.FourthDisposable, Neovolve.Toolkit.Unity.IntegrationTests"/>
    /// 
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// <para>
    /// The following example demonstrates an unnamed resolution of <see cref="ProxyManager{T}"/> that results in using the 
    ///     <see cref="ProxyManager{T}(ProxyHandler{T})"/> constructor.
    ///   </para>
    /// <code lang="xml" title="Application configuration for unnamed ProxyManager injection">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <extensions>
    ///           <add type="Neovolve.Toolkit.Unity.DisposableStrategyExtension, Neovolve.Toolkit.Unity"/>
    ///         </extensions>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.ISecondDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.SecondDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   name="NamedProxyTest">
    ///           <constructor>
    ///             <param name="third">
    ///               <!-- 
    ///                   The parameter third on SecondDisposable is of type IThirdDisposable 
    ///                   The resolution here will create ProxyManager<IThirdDisposable> and return a proxy of this type
    ///               -->
    ///               <proxy/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Communication.ProxyManager`1, Neovolve.Toolkit">
    ///           <constructor>
    ///             <param name="proxyHandler">
    ///               <dependency name="NamedProxyHandler"/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Communication.ProxyHandler`1, Neovolve.Toolkit"
    ///                   mapTo="Neovolve.Toolkit.Communication.DefaultProxyHandler`1, Neovolve.Toolkit"
    ///                   name="NamedProxyHandler">
    ///           <constructor>
    ///             <param name="target">
    ///               <dependency/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IThirdDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.ThirdDisposable, Neovolve.Toolkit.Unity.IntegrationTests">
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IFourthDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.FourthDisposable, Neovolve.Toolkit.Unity.IntegrationTests"/>
    /// 
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// <para>
    /// The following example demonstrates an unnamed resolution of <see cref="ProxyHandler{T}"/> that results in using the 
    ///     <see cref="ProxyManager{T}(ProxyHandler{T})"/> constructor.
    ///   </para>
    /// <code lang="xml" title="Application configuration for unnamed ProxyHandler injection">
    /// <![CDATA[<?xml version="1.0"
    ///       encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="unity"
    ///              type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///   </configSections>
    ///   <unity>
    ///     <sectionExtension type="Neovolve.Toolkit.Unity.SectionExtensionInitiator, Neovolve.Toolkit.Unity"/>
    ///     <containers>
    ///       <container>
    ///         <extensions>
    ///           <add type="Neovolve.Toolkit.Unity.DisposableStrategyExtension, Neovolve.Toolkit.Unity"/>
    ///         </extensions>
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.ISecondDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.SecondDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   name="NamedProxyTest">
    ///           <constructor>
    ///             <param name="third">
    ///               <!-- 
    ///                   The parameter third on SecondDisposable is of type IThirdDisposable 
    ///                   The resolution here will create ProxyManager<IThirdDisposable> and return a proxy of this type
    ///               -->
    ///               <proxy/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Communication.ProxyHandler`1, Neovolve.Toolkit"
    ///                   mapTo="Neovolve.Toolkit.Communication.DefaultProxyHandler`1, Neovolve.Toolkit">
    ///           <constructor>
    ///             <param name="target">
    ///               <dependency/>
    ///             </param>
    ///           </constructor>
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IThirdDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.ThirdDisposable, Neovolve.Toolkit.Unity.IntegrationTests">
    ///         </register>
    /// 
    ///         <register type="Neovolve.Toolkit.Unity.IntegrationTests.IFourthDisposable, Neovolve.Toolkit.Unity.IntegrationTests"
    ///                   mapTo="Neovolve.Toolkit.Unity.IntegrationTests.FourthDisposable, Neovolve.Toolkit.Unity.IntegrationTests"/>
    /// 
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// </example>
    public class ProxyParameterValueElement : ParameterValueElement
    {
        /// <summary>
        ///   Defines the configuration element name used to identify this type of parameter value element.
        /// </summary>
        public const String ElementName = "proxy";

        /// <summary>
        ///   Defines the attribute name for the <see cref = "Name" /> property.
        /// </summary>
        public const String NameAttributeName = "name";

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

            return new ProxyInjectionParameterValue(parameterType, Name);
        }

        /// <summary>
        ///   Gets or sets the name of the dependency.
        /// </summary>
        /// <value>
        ///   The name of the dependency.
        /// </value>
        [ConfigurationProperty(NameAttributeName, IsRequired = false, DefaultValue = null)]
        public String Name
        {
            get
            {
                return (String)base[NameAttributeName];
            }

            set
            {
                base[NameAttributeName] = value;
            }
        }
    }
}