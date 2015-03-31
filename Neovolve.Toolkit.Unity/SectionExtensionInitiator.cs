namespace Neovolve.Toolkit.Unity
{
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// The <see cref="SectionExtensionInitiator"/>
    ///   class is used to initiate a <see cref="SectionExtension"/> 
    ///   with configuration element support for custom parameter injection values.
    /// </summary>
    /// <example>
    /// <para>
    /// The following example demonstrates how the <see cref="SectionExtensionInitiator"/> class is configured for a unity configuration section.
    ///   </para>
    /// <code lang="xml" title="Application configuration">
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
    ///       </container>
    ///     </containers>
    ///   </unity>
    /// </configuration>]]>
    ///   </code>
    /// </example>
    public class SectionExtensionInitiator : SectionExtension
    {
        /// <summary>
        /// Adds the extensions.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public override void AddExtensions(SectionExtensionContext context)
        {
            if (context == null)
            {
                return;
            }

            context.AddElement<AppSettingsParameterValueElement>(AppSettingsParameterValueElement.ElementName);
            context.AddElement<ConnectionStringParameterValueElement>(ConnectionStringParameterValueElement.ElementName);
            context.AddElement<ProxyParameterValueElement>(ProxyParameterValueElement.ElementName);
        }
    }
}