namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Activities.Hosting;
    using System.Configuration;
    using System.Globalization;
    using System.ServiceModel.Activities;
    using System.ServiceModel.Configuration;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="WorkflowExtensionElement"/>
    ///   class is used to provide configuration support for adding the <see cref="WorkflowExtensionBehavior"/>
    ///   to the configuration of a service.
    /// </summary>
    /// <example>
    /// <para>
    /// The following configuration adds an extension to the service.
    ///   </para>
    /// <code lang="xml" title="Example of web.config">
    /// <![CDATA[
    /// <?xml version="1.0" ?>
    /// <configuration>
    ///     <system.serviceModel>
    ///         <behaviors>
    ///             <serviceBehaviors>
    ///                 <behavior name="ExtensionBehavior">
    ///                     <workflowExtension type="Neovolve.Toolkit.Workflow.Extensions.InstanceManagerExtension, Neovolve.Toolkit.Workflow"/>
    ///                     <workflowExtension 
    ///                         type="MyApplication.SingleInstanceExtension, MyApplication"
    ///                         isSingleton="true"/>
    ///                 </behavior>
    ///             </serviceBehaviors>
    ///         </behaviors>
    ///         <extensions>
    ///             <behaviorExtensions>
    ///                 <add name="workflowExtension"
    ///                      type="Neovolve.Toolkit.Workflow.WorkflowExtensionElement, Neovolve.Toolkit.Workflow"/>
    ///             </behaviorExtensions>
    ///         </extensions>
    ///         <services>
    ///             <service behaviorConfiguration="ExtensionBehavior"
    ///                      name="Neovolve.Toolkit.IntegrationTests.Communication.TestService">
    ///                 <endpoint address=""
    ///                           binding="basicHttpBinding"
    ///                           bindingConfiguration=""
    ///                           contract="Neovolve.Toolkit.IntegrationTests.Communication.ITestService"/>
    ///             </service>
    ///         </services>
    ///     </system.serviceModel>
    /// </configuration> 
    /// ]]>
    ///   </code>
    /// </example>
    /// <seealso cref="WorkflowExtensionBehavior"/>
    public class WorkflowExtensionElement : BehaviorExtensionElement
    {
        /// <summary>
        ///   Defines the attribute name for the error handler type.
        /// </summary>
        public const String ExtensionTypeAttributeName = "type";

        /// <summary>
        ///   Defines the attribute name for the is singleton value.
        /// </summary>
        public const String IsSingletonAttributeName = "isSingleton";

        /// <summary>
        ///   Stores the set of properties for the element.
        /// </summary>
        private ConfigurationPropertyCollection _properties;

        /// <summary>
        /// Creates a behaviour extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behaviour extension.
        /// </returns>
        protected override Object CreateBehavior()
        {
            Type extensionType = Type.GetType(ExtensionType);

            if (extensionType == null)
            {
                String message = String.Format(CultureInfo.CurrentCulture, Resources.WorkflowExtensionElement_ExtensionTypeNotLoaded, ExtensionType);

                throw new ConfigurationErrorsException(message);
            }

            return new WorkflowExtensionBehavior(extensionType, IsSingleton);
        }

        /// <summary>
        ///   Gets the type of behavior.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.Type" />.
        /// </returns>
        /// <value>
        ///   The behavior type.
        /// </value>
        public override Type BehaviorType
        {
            get
            {
                return typeof(WorkflowExtensionBehavior);
            }
        }

        /// <summary>
        ///   Gets or sets the type of the extension.
        /// </summary>
        /// <value>
        ///   The type of the extension.
        /// </value>
        [ConfigurationProperty(ExtensionTypeAttributeName)]
        public String ExtensionType
        {
            get
            {
                return (String)base[ExtensionTypeAttributeName];
            }

            set
            {
                base[ExtensionTypeAttributeName] = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the extension is a singleton.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the extension is a singleton; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        ///   <para>
        ///     Only one instance of the extension will exist for the <see cref = "WorkflowServiceHost" /> if
        ///     <see cref = "IsSingleton" /> is <c>true</c>. 
        ///     A new instance of the extension will be created for each <see cref = "WorkflowInstance" /> in the
        ///     <see cref = "WorkflowServiceHost" /> if <see cref = "IsSingleton" /> is <c>false</c>.
        ///   </para>
        ///   <para>
        ///     The default value is <c>false</c>.
        ///   </para>
        /// </remarks>
        [ConfigurationProperty(IsSingletonAttributeName)]
        public Boolean IsSingleton
        {
            get
            {
                return (Boolean)base[IsSingletonAttributeName];
            }

            set
            {
                base[IsSingletonAttributeName] = value;
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
                            ExtensionTypeAttributeName, typeof(String), String.Empty, null, null, ConfigurationPropertyOptions.IsRequired));

                    properties.Add(new ConfigurationProperty(IsSingletonAttributeName, typeof(Boolean), false));

                    _properties = properties;
                }

                return _properties;
            }
        }
    }
}