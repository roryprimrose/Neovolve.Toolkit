namespace Neovolve.Toolkit.Server.Unity
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="UnityControllerFactoryHttpModule"/>
    ///   class is used to build up ASP.Net MVC controller instances using an <see cref="IUnityContainer"/>.
    /// </summary>
    /// <threadsafety instance="true"/>
    /// <remarks>
    /// <para>
    /// <see cref="UnityControllerFactoryHttpModule"/> resolves the default <see cref="IUnityContainer"/>
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
    /// The following web.config defines a default container to assign a HashAlgorithm type to an ASP.Net MVC controller.
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
    ///                           mapTo="System.Security.Cryptography.SHA256CryptoServiceProvider, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"/>
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
    ///             <add type="Neovolve.Toolkit.Unity.UnityControllerFactoryHttpModule"
    ///                  name="UnityControllerFactoryHttpModule"/>
    ///         </httpModules>
    ///     </system.web>
    ///     <system.webServer>
    ///         <validation validateIntegratedModeConfiguration="false"/>
    ///         <modules runAllManagedModulesForAllRequests="true">
    ///             <add type="Neovolve.Toolkit.Unity.UnityControllerFactoryHttpModule"
    ///                  name="UnityControllerFactoryHttpModule"/>
    ///         </modules>
    ///     </system.webServer>
    /// </configuration>
    /// ]]>
    ///   </code>
    /// <para>
    /// The web.config provided above is used by <see cref="UnityControllerFactoryHttpModule"/> to create MVC controllers.
    ///     The following is an example of a controller that gets created using this configuration.
    ///   </para>
    /// <code lang="xml" title="Controller created using Unity">
    /// <![CDATA[
    /// using System;
    /// using System.Security.Cryptography;
    /// using System.Text;
    /// using System.Web.Mvc;
    /// 
    ///     namespace Neovolve.Toolkit.Unity.MvcWebIntegrationTests.Controllers
    ///     {
    ///         [HandleError]
    ///         public class HomeController : Controller
    ///         {
    ///             public HomeController(HashAlgorithm hashCalulator)
    ///             {
    ///                 HashCalculator = hashCalulator;
    ///             }
    /// 
    ///             public ActionResult About()
    ///             {
    ///                 return View();
    ///             }
    /// 
    ///             public ActionResult Index()
    ///             {
    ///                 String valueToHash = Guid.NewGuid().ToString();
    ///                 Byte[] valueInBytes = Encoding.UTF8.GetBytes(valueToHash);
    ///                 Byte[] hashBytes = HashCalculator.ComputeHash(valueInBytes);
    /// 
    ///                 String values = valueToHash + " - " + Convert.ToBase64String(hashBytes);
    /// 
    ///                 ViewData["Message"] = "Welcome to ASP.NET MVC! " + values;
    /// 
    ///                 return View();
    ///             }
    /// 
    ///             protected HashAlgorithm HashCalculator
    ///             {
    ///                 get;
    ///                 private set;
    ///             }
    ///         }
    ///     }
    /// ]]>
    ///   </code>
    /// </example>
    public class UnityControllerFactoryHttpModule : UnityHttpModuleBase
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="IHttpModule"/>.
        /// </summary>
        public override void Dispose()
        {
            IControllerFactory defaultFactory = new DefaultControllerFactory();

            ControllerBuilder.Current.SetControllerFactory(defaultFactory);

            base.Dispose();
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events
        ///   common to all application objects within an ASP.NET application.
        /// </param>
        public override void Init(HttpApplication context)
        {
            base.Init(context);

            UnityControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory() as UnityControllerFactory;

            if (controllerFactory != null)
            {
                return;
            }

            UnityControllerFactory factory = new UnityControllerFactory(Container);

            ControllerBuilder.Current.SetControllerFactory(factory);
        }
    }
}