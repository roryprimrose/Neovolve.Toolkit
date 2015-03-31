namespace Neovolve.Toolkit.Communication.Security
{
    using System;
    using System.Security;
    using System.Security.Principal;

    /// <summary>
    /// The <see cref="PasswordIdentity"/>
    ///   class provides an <see cref="IIdentity"/> that exposes the password related to the username.
    /// </summary>
    public class PasswordIdentity : GenericIdentity
    {
        /// <summary>
        ///   Stores the IsAuthenticated value.
        /// </summary>
        private readonly Boolean _isAuthenticated;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordIdentity"/> class.
        /// </summary>
        /// <param name="userName">
        /// Name of the user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="isAuthenticated">
        /// <c>true</c>, if the identity is authenticated; otherwise, <c>false</c>.
        /// </param>
        /// <param name="authenticationType">
        /// Type of the authentication.
        /// </param>
        public PasswordIdentity(String userName, SecureString password, Boolean isAuthenticated, String authenticationType)
            : base(userName, authenticationType)
        {
            Password = password;
            _isAuthenticated = isAuthenticated;
        }

        /// <summary>
        ///   Gets a value indicating whether the user has been authenticated.
        /// </summary>
        /// <value>
        ///   A value indicating whether the user has been authenticated.
        /// </value>
        /// <returns>
        ///   <c>true</c> if the user was authenticated; otherwise, <c>false</c>.
        /// </returns>
        public override Boolean IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
        }

        /// <summary>
        ///   Gets the password.
        /// </summary>
        /// <value>
        ///   The password.
        /// </value>
        public SecureString Password
        {
            get;
            private set;
        }
    }
}