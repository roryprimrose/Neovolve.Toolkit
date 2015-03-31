namespace Neovolve.Toolkit.Communication.Security
{
    using System;
    using System.IdentityModel.Tokens;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="DefaultPasswordValidator"/>
    ///   class provides a user name password validation implementation that ensures that a user name and password value have been supplied.
    /// </summary>
    public class DefaultPasswordValidator : OptionalPasswordValidator
    {
        /// <summary>
        /// Validates the specified user name and password.
        /// </summary>
        /// <param name="userName">
        /// The user name to validate.
        /// </param>
        /// <param name="password">
        /// The password to validate.
        /// </param>
        public override void Validate(String userName, String password)
        {
            base.Validate(userName, password);

            // Check if there is a password
            if (String.IsNullOrEmpty(password))
            {
                throw new SecurityTokenException(Resources.NoPasswordProvided);
            }
        }
    }
}