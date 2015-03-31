namespace Neovolve.Toolkit.IntegrationTests.Communication.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// The <see cref="CertificateManager"/>
    ///   class is used to install and uninstall certificates in the certificate store.
    /// </summary>
    internal class CertificateManager
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CertificateManager" /> class.
        /// </summary>
        public CertificateManager()
        {
            Certificates = new List<CertificateDetails>();
        }

        /// <summary>
        /// Installs the certificates.
        /// </summary>
        public void InstallCertificates()
        {
            // Loop through each configured certificate
            for (Int32 index = 0; index < Certificates.Count; index++)
            {
                CertificateDetails certificate = Certificates[index];

                // Install this certificate
                InstallCertificate(certificate);
            }
        }

        /// <summary>
        /// Uninstalls the certificates.
        /// </summary>
        public void UninstallCertificates()
        {
            // Loop through each configured certificate
            for (Int32 index = 0; index < Certificates.Count; index++)
            {
                CertificateDetails certificate = Certificates[index];

                // Uninstall this certificate
                UninstallCertificate(certificate);
            }
        }

        /// <summary>
        /// Installs the certificate.
        /// </summary>
        /// <param name="certificateDetails">
        /// The certificate information.
        /// </param>
        private static void InstallCertificate(CertificateDetails certificateDetails)
        {
            X509Certificate2 certificate = new X509Certificate2(
                certificateDetails.Filepath, certificateDetails.Password, X509KeyStorageFlags.PersistKeySet);
            X509Store store = new X509Store(certificateDetails.StoreName, certificateDetails.StoreLocation);

            try
            {
                store.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                store.Add(certificate);
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary>
        /// Uninstalls the certificate.
        /// </summary>
        /// <param name="certificateDetails">
        /// The certificate information.
        /// </param>
        private static void UninstallCertificate(CertificateDetails certificateDetails)
        {
            X509Store store = new X509Store(certificateDetails.StoreName, certificateDetails.StoreLocation);

            try
            {
                store.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                // Loop through each certificate in the store in reverse order
                for (Int32 index = store.Certificates.Count - 1; index >= 0; index--)
                {
                    X509Certificate2 certificate = store.Certificates[index];

                    // Check if the subject names match
                    if (certificate.Subject == certificateDetails.Subject)
                    {
                        store.Remove(certificate);
                    }
                }
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary>
        ///   Gets the certificates.
        /// </summary>
        /// <value>
        ///   The certificates.
        /// </value>
        public List<CertificateDetails> Certificates
        {
            get;
            private set;
        }
    }
}