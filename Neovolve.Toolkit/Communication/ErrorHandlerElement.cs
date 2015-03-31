namespace Neovolve.Toolkit.Communication
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.ServiceModel.Configuration;

    /// <summary>
    /// The <see cref="ErrorHandlerElement"/>
    ///   class is used to provide configuration support for defining an error handler via a service behavior.
    /// </summary>
    /// <example>
    /// <para>
    /// The following configuration defines an error handler for a service.
    ///   </para>
    /// <code lang="xml" title="Example of web.config">
    /// <![CDATA[
    /// <?xml version="1.0" ?>
    /// <configuration>
    ///     <system.serviceModel>
    ///         <behaviors>
    ///             <serviceBehaviors>
    ///                 <behavior name="ErrorHandlerBehavior">
    ///                     <errorHandler type="Neovolve.Toolkit.IntegrationTests.Communication.KnownErrorHandler, Neovolve.Toolkit.IntegrationTests"/>
    ///                 </behavior>
    ///             </serviceBehaviors>
    ///         </behaviors>
    ///         <extensions>
    ///             <behaviorExtensions>
    ///                 <add name="errorHandler"
    ///                      type="Neovolve.Toolkit.Communication.ErrorHandlerElement, Neovolve.Toolkit"/>
    ///             </behaviorExtensions>
    ///         </extensions>
    ///         <services>
    ///             <service behaviorConfiguration="ErrorHandlerBehavior"
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
    /// <seealso cref="ErrorHandlerAttribute"/>
    public class ErrorHandlerElement : BehaviorExtensionElement
    {
        /// <summary>
        ///   Defines the attribute name for the error handler type.
        /// </summary>
        public const String ErrorHandlerTypeAttributeName = "type";

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
            Contract.Assume(String.IsNullOrWhiteSpace(ErrorHandlerType) == false);

            return new ErrorHandlerAttribute(ErrorHandlerType);
        }

        /// <summary>
        ///   Gets the type of behaviour.
        /// </summary>
        /// <value>
        ///   The type of behaviour.
        /// </value>
        /// <returns>
        ///   A <see cref = "T:System.Type" />.
        /// </returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(ErrorHandlerAttribute);
            }
        }

        /// <summary>
        ///   Gets or sets the type of the error handler.
        /// </summary>
        /// <value>
        ///   The type of the error handler.
        /// </value>
        [ConfigurationProperty(ErrorHandlerTypeAttributeName, IsRequired = true)]
        public String ErrorHandlerType
        {
            get
            {
                return (String)base[ErrorHandlerTypeAttributeName];
            }

            set
            {
                base[ErrorHandlerTypeAttributeName] = value;
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
                            ErrorHandlerTypeAttributeName, typeof(String), String.Empty, null, null, ConfigurationPropertyOptions.IsRequired));

                    _properties = properties;
                }

                return _properties;
            }
        }
    }
}