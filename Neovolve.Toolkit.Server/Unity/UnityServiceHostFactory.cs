namespace Neovolve.Toolkit.Server.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity;

    /// <summary>
    /// The <see cref="UnityServiceHostFactory"/>
    ///   class is used to create a <see cref="ServiceHost"/> instance that supports creating service instances with Unity.
    /// </summary>
    /// <remarks>
    /// <note>
    /// The <see cref="UnityServiceHostFactory"/> class does not support custom Unity configuration section names or defined Unity container names.
    ///     Only default containers will be resolved from the <c>unity</c> configuration section. Use the <see cref="UnityServiceElement"/> configuration
    ///     if this functionality is required.
    ///   </note>
    /// </remarks>
    /// <example>
    /// <para>
    /// The following example shows how the factory can be configured in the mark-up of a svc file.
    ///   </para>
    /// <code lang="xml" title="Example of svc markup">
    /// <![CDATA[
    /// <%@ ServiceHost 
    ///     Language="C#" 
    ///     Debug="true" 
    ///     Service="Neovolve.Toolkit.Unity.WebIntegrationTests.TestService" 
    ///     Factory="Neovolve.Toolkit.Unity.UnityServiceHostFactory" %>
    /// ]]>
    ///   </code>
    /// <para>
    /// The 4.0 framework now supports defining service endpoints without associated svc files. Service host factories for these endpoints are defined
    ///     in the web.config.
    ///   </para>
    /// <code lang="xml" title="Example of web.config">
    /// <![CDATA[
    /// <configuration>
    ///     <configSections>
    ///         <section name="unity"
    ///                  type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///     </configSections>
    ///     <unity>
    ///         <containers>
    ///             <container>
    ///                 <register type="Neovolve.Toolkit.Unity.WebIntegrationTests.TestService, Neovolve.Toolkit.Unity.WebIntegrationTests">
    ///                     <constructor>
    ///                         <param name="prefix">
    ///                             <value value="Injected by default unity section and container"/>
    ///                         </param>
    ///                     </constructor>
    ///                 </register>
    ///             </container>
    ///         </containers>
    ///     </unity>
    ///     <system.serviceModel>
    ///         <serviceHostingEnvironment>
    ///             <serviceActivations>
    ///                 <add service="Neovolve.Toolkit.Unity.WebIntegrationTests.TestService" 
    ///                      factory="Neovolve.Toolkit.Unity.UnityServiceHostFactory" 
    ///                      relativeAddress="/TestService.svc"/>
    ///             </serviceActivations>
    ///         </serviceHostingEnvironment>
    ///     </system.serviceModel>
    /// </configuration>
    /// ]]>
    ///   </code>
    /// </example>
    /// <seealso cref="UnityServiceElement"/>
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "UnityServiceHostFactory" /> class.
        /// </summary>
        public UnityServiceHostFactory()
            : this(null, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHostFactory"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="unitySectionName">
        /// Name of the unity section.
        /// </param>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        public UnityServiceHostFactory(IConfigurationStore configuration, String unitySectionName, String containerName)
        {
            Container = UnityContainerResolver.Resolve(configuration, unitySectionName, containerName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHostFactory"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public UnityServiceHostFactory(IUnityContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            Container = container;
        }

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">
        /// Specifies the type of service to host.
        /// </param>
        /// <param name="baseAddresses">
        /// The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            Contract.Assume(Container != null);

            return new UnityServiceHost(serviceType, Container, baseAddresses);
        }

        /// <summary>
        ///   Gets the container.
        /// </summary>
        /// <value>
        ///   The container.
        /// </value>
        public IUnityContainer Container
        {
            get;
            private set;
        }
    }
}