namespace Neovolve.Toolkit.Communication.Security
{
    using System;
    using System.Diagnostics;
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="PasswordSecurityTokenManager"/>
    ///   class is used to provide the <see cref="PasswordSecurityTokenAuthenticator"/>
    ///   used for processing user name and password combinations for service authentication.
    /// </summary>
    internal class PasswordSecurityTokenManager : ServiceCredentialsSecurityTokenManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordSecurityTokenManager"/> class.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        public PasswordSecurityTokenManager(ServiceCredentials credentials)
            : base(credentials)
        {
        }

        /// <summary>
        /// Creates a security token authenticator based on the <see cref="T:System.IdentityModel.Selectors.SecurityTokenRequirement"/>.
        /// </summary>
        /// <param name="tokenRequirement">
        /// The <see cref="T:System.IdentityModel.Selectors.SecurityTokenRequirement"/>.
        /// </param>
        /// <param name="outOfBandTokenResolver">
        /// When this method returns, contains a <see cref="T:System.IdentityModel.Selectors.SecurityTokenResolver"/>. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.IdentityModel.Selectors.SecurityTokenAuthenticator"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// A security token authenticator cannot be created for the<paramref name=" tokenRequirement"/> that was passed in.
        /// </exception>
        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(
            SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
        {
            if (tokenRequirement == null)
            {
                throw new ArgumentNullException("tokenRequirement");
            }

            if (tokenRequirement.TokenType == SecurityTokenTypes.UserName)
            {
                outOfBandTokenResolver = null;

                // Get the current validator 
                UserNamePasswordValidator validator = ServiceCredentials.UserNameAuthentication.CustomUserNamePasswordValidator;

                // Ensure that a validator exists 
                if (validator == null)
                {
                    Trace.TraceWarning(Resources.NoCustomUserNamePasswordValidatorConfigured);

                    validator = new DefaultPasswordValidator();
                }

                return new PasswordSecurityTokenAuthenticator(validator);
            }

            return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
        }
    }
}