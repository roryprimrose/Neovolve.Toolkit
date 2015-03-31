namespace Neovolve.Toolkit.IntegrationTests.Communication.Security
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// The <see cref="CertificateDetails"/>
    ///   struct is used to store information about a certificate for installing and uninstalling a certificate in a certificate store.
    /// </summary>
    internal struct CertificateDetails
    {
        /// <summary>
        ///   Stores the Filepath of the certificate.
        /// </summary>
        public String Filepath;

        /// <summary>
        ///   Stores the password of the certificate.
        /// </summary>
        public String Password;

        /// <summary>
        ///   Stores the store location of the certificate when the certificate is installed.
        /// </summary>
        public StoreLocation StoreLocation;

        /// <summary>
        ///   Stores the store name of the certificate when the certificate is installed.
        /// </summary>
        public StoreName StoreName;

        /// <summary>
        ///   Stores the subject of the certificate.
        /// </summary>
        public String Subject;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateDetails"/> struct.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="subject">
        /// The subject.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="storeName">
        /// Name of the store.
        /// </param>
        /// <param name="storeLocation">
        /// The store location.
        /// </param>
        public CertificateDetails(String filePath, String subject, String password, StoreName storeName, StoreLocation storeLocation)
        {
            Filepath = filePath;
            Subject = subject;
            Password = password;
            StoreName = storeName;
            StoreLocation = storeLocation;
        }
    }
}