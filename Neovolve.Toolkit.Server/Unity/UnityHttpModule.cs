namespace Neovolve.Toolkit.Server.Unity
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.UI;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="UnityHttpModule"/>
    ///   class is used to build up ASP.Net pages with property and method injection 
    ///   after they are created but before they are used for request processing.
    /// </summary>
    /// <threadsafety instance="true"/>
    /// <remarks>
    /// <para>
    /// <see cref="UnityHttpModule"/> resolves the default <see cref="IUnityContainer"/>
    ///     defined in configuration when it is created. Resolving named containers from configuration is not supported.
    ///   </para>
    /// <para>
    /// Runtime configuration of the resolved container can be done by accessing the <see cref="UnityHttpModuleBase.Container"/> property.
    ///     A custom container can also be assigned to this property at runtime to provide a different container. 
    ///     Changes to the <see cref="UnityHttpModuleBase.Container"/> property should be done early in the application life cycle to prevent
    ///     unpredictable application behaviour.
    ///   </para>
    /// <note>
    /// Property and method injection requires that injection items are decorated with the 
    ///     <see cref="DependencyAttribute"/>. The properties must be scoped as <c>public</c> for Unity to inject values.
    ///   </note>
    /// </remarks>
    /// <example>
    /// <para>
    /// The following web.config defines a default container to assign a HashAlgorithm type to an ASP.Net page.
    ///   </para>
    /// <code lang="xml" title="Example of web.config">
    /// <![CDATA[
    /// <?xml version="1.0" ?>
    /// <configuration>
    ///     <configSections>
    ///         <section name="unity"
    ///                  type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration"/>
    ///     </configSections>
    ///     <unity>
    ///         <containers>
    ///             <container>
    ///                 <register type="System.Security.Cryptography.HashAlgorithm, mscorlib"
    ///                     mapTo="System.Security.Cryptography.SHA256CryptoServiceProvider, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"/>
    ///                 <extensions>
    ///                     <add type="Neovolve.Toolkit.Unity.DisposableStrategyExtension, Neovolve.Toolkit.Unity"/>
    ///                 </extensions>
    ///             </container>
    ///         </containers>
    ///     </unity>
    ///     <system.web>
    ///         <compilation debug="true"
    ///                      targetFramework="4.0"/>
    ///         <authentication mode="None"></authentication>
    ///         <httpModules>
    ///             <add type="Neovolve.Toolkit.Unity.UnityHttpModule"
    ///                  name="UnityHttpModule"/>
    ///         </httpModules>
    ///     </system.web>
    ///     <system.webServer>
    ///         <validation validateIntegratedModeConfiguration="false"/>
    ///         <modules runAllManagedModulesForAllRequests="true">
    ///             <add type="Neovolve.Toolkit.Unity.UnityHttpModule"
    ///                  name="UnityHttpModule"/>
    ///         </modules>
    ///     </system.webServer>
    /// </configuration>
    /// ]]>
    ///   </code>
    /// <para>
    /// The web.config provided above is used by <see cref="UnityHttpModule"/> to build up a web page using property injection.
    ///     The following is an example of the code behind of such a page.
    ///   </para>
    /// <code lang="xml" title="Code behind of page that relies on property injection">
    /// <![CDATA[
    /// using System;
    /// using System.Configuration;
    /// using System.Security.Cryptography;
    /// using System.Text;
    /// using System.Web.UI;
    /// using Microsoft.Practices.Unity;
    /// 
    /// namespace Neovolve.Toolkit.Unity.WebIntegrationTests
    /// {
    ///     public partial class _Default : Page
    ///     {
    ///         protected void Page_Load(Object sender, EventArgs e)
    ///         {
    ///             if (HashCalculator == null)
    ///             {
    ///                 throw new ConfigurationErrorsException("Unity was not used to build up this page");
    ///             }
    /// 
    ///             String valueToHash = Guid.NewGuid().ToString();
    ///             Byte[] valueInBytes = Encoding.UTF8.GetBytes(valueToHash);
    ///             Byte[] hashBytes = HashCalculator.ComputeHash(valueInBytes);
    /// 
    ///             Original.Text = valueToHash;
    ///             HashValue.Text = Convert.ToBase64String(hashBytes);
    ///         }
    /// 
    ///         [Dependency]
    ///         public HashAlgorithm HashCalculator
    ///         {
    ///             get;
    ///             set;
    ///         }
    ///     }
    /// }
    /// ]]>
    ///   </code>
    /// </example>
    public class UnityHttpModule : UnityHttpModuleBase
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events 
        ///   common to all application objects within an ASP.NET application.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        public override void Init(HttpApplication context)
        {
            Contract.Assume(context != null);

            base.Init(context);

            context.PreRequestHandlerExecute += OnPreRequest;
            context.PostRequestHandlerExecute += OnRequestCompleted;
        }

        /// <summary>
        /// Processes the unity action.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="unityAction">
        /// The unity action.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", 
            Justification = "The code contract ensures that this value is not null.")]
        protected static void ProcessUnityAction(Object sender, Action<Page> unityAction)
        {
            Contract.Requires<ArgumentNullException>(sender != null);
            Contract.Requires<ArgumentNullException>(unityAction != null);

            if (HttpContext.Current == null)
            {
                return;
            }

            Page page = HttpContext.Current.Handler as Page;

            if (page == null)
            {
                return;
            }

            unityAction(page);
        }

        /// <summary>
        /// Called before a page request is processed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private static void OnPreRequest(Object sender, EventArgs e)
        {
            Contract.Assume(sender != null);

            ProcessUnityAction(sender, page => Container.BuildUp(page.GetType(), page, null));
        }

        /// <summary>
        /// Called when a page request has been completed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private static void OnRequestCompleted(Object sender, EventArgs e)
        {
            Contract.Assume(sender != null);

            ProcessUnityAction(sender, page => Container.Teardown(page));
        }
    }
}