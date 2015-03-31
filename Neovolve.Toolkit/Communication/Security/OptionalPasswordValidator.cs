namespace Neovolve.Toolkit.Communication.Security
{
    using System;
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="OptionalPasswordValidator"/>
    ///   class provides a user name password validation implementation that ensures that a user name value has been supplied.
    /// </summary>
    /// <remarks>
    /// This validator allows the password to be optional.
    /// </remarks>
    public class OptionalPasswordValidator : UserNamePasswordValidator
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
            // Check if there is a user name
            if (String.IsNullOrEmpty(userName))
            {
                throw new SecurityTokenException(Resources.NoUserNameProvided);
            }
        }
    }
}