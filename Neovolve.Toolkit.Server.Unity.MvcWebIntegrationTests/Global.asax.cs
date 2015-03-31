namespace Neovolve.Toolkit.Server.Unity.MvcWebIntegrationTests
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// The mvc application.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Parameter defaults
            var defaults = new
                           {
                               // URL with parameters
                               controller = "Home", 
                               action = "Index", 
                               id = UrlParameter.Optional
                           };
            const String RouteName = "{controller}/{action}/{id}";

            routes.MapRoute("Default", RouteName, defaults);
        }

        /// <summary>
        /// The application_ start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}