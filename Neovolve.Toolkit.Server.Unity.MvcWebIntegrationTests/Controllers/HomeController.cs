namespace Neovolve.Toolkit.Server.Unity.MvcWebIntegrationTests.Controllers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// The home controller.
    /// </summary>
    [HandleError]
    public class HomeController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="hashCalculator">
        /// The hash calculator.
        /// </param>
        public HomeController(HashAlgorithm hashCalculator)
        {
            HashCalculator = hashCalculator;
        }

        /// <summary>
        /// The about.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [ValidateAntiForgeryToken]
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            String valueToHash = Guid.NewGuid().ToString();
            Byte[] valueInBytes = Encoding.UTF8.GetBytes(valueToHash);
            Byte[] hashBytes = HashCalculator.ComputeHash(valueInBytes);

            String values = valueToHash + " - " + Convert.ToBase64String(hashBytes);

            ViewData["Message"] = "Welcome to ASP.NET MVC! " + values;

            return View();
        }

        /// <summary>
        ///   Gets HashCalculator.
        /// </summary>
        /// <value>
        ///   The hash calculator.
        /// </value>
        protected HashAlgorithm HashCalculator
        {
            get;
            private set;
        }
    }
}