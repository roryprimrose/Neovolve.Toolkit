namespace Neovolve.Toolkit.Server.Unity.WebIntegrationTests
{
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The _ default.
    /// </summary>
    public partial class _Default : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (HashCalculator == null)
            {
                throw new ConfigurationErrorsException("Unity was not used to build up this page");
            }

            String valueToHash = Guid.NewGuid().ToString();
            Byte[] valueInBytes = Encoding.UTF8.GetBytes(valueToHash);
            Byte[] hashBytes = HashCalculator.ComputeHash(valueInBytes);

            Original.Text = valueToHash;
            HashValue.Text = Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        ///   Gets or sets HashCalculator.
        /// </summary>
        /// <value>
        ///   The hash calculator.
        /// </value>
        [Dependency]
        public HashAlgorithm HashCalculator
        {
            get;
            set;
        }
    }
}