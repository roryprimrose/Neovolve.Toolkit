namespace Neovolve.Toolkit.Server.Unity
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Practices.Unity;
    using Neovolve.Toolkit.Server.Properties;

    /// <summary>
    /// The <see cref="UnityControllerFactory"/>
    ///   class is used to provide a controller factory that creates controllers using a <see cref="IUnityContainer"/> instance.
    /// </summary>
    public class UnityControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityControllerFactory"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public UnityControllerFactory(IUnityContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            Container = container;
        }

        /// <summary>
        /// Releases the specified controller.
        /// </summary>
        /// <param name="controller">
        /// The controller to release.
        /// </param>
        public override void ReleaseController(IController controller)
        {
            Container.Teardown(controller);

            base.ReleaseController(controller);
        }

        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">
        /// The context of the HTTP request, which includes the HTTP context and route data.
        /// </param>
        /// <param name="controllerType">
        /// The type of the controller.
        /// </param>
        /// <returns>
        /// The controller instance.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">
        /// <paramref name="controllerType"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="controllerType"/> cannot be assigned.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// An instance of <paramref name="controllerType"/> cannot be created.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            Contract.Assume(requestContext != null);

            IController controller;

            if (controllerType == null)
            {
                String message = String.Format(
                    CultureInfo.CurrentUICulture, Resources.UnityControllerFactory_NoControllerFound, requestContext.HttpContext.Request.Path);

                throw new HttpException(404, message);
            }

            if (typeof(IController).IsAssignableFrom(controllerType) == false)
            {
                String message = String.Format(
                    CultureInfo.CurrentUICulture, Resources.UnityControllerFactory_TypeDoesNotSubclassControllerBase, controllerType);

                throw new ArgumentException(message, "controllerType");
            }

            try
            {
                controller = (IController)Container.Resolve(controllerType);
            }
            catch (Exception exception)
            {
                String message = String.Format(CultureInfo.CurrentUICulture, Resources.UnityControllerFactory_ErrorCreatingController, controllerType);

                throw new InvalidOperationException(message, exception);
            }

            return controller;
        }

        /// <summary>
        ///   Gets the container.
        /// </summary>
        /// <value>
        ///   The container.
        /// </value>
        protected IUnityContainer Container
        {
            get;
            private set;
        }
    }
}