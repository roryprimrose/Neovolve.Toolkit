namespace Neovolve.Toolkit.Communication.Security
{
    using System;
    using System.Security.Principal;

    /// <summary>
    /// The <see cref="PasswordPrincipal"/>
    ///   class provides information about the roles available to the <see cref="PasswordIdentity"/> that it exposes.
    /// </summary>
    public class PasswordPrincipal : GenericPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordPrincipal"/> class.
        /// </summary>
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <param name="roles">
        /// The roles.
        /// </param>
        public PasswordPrincipal(IIdentity identity, String[] roles)
            : base(identity, roles)
        {
        }
    }
}