namespace Neovolve.Toolkit.IntegrationTests.Communication.Security
{
    using System;
    using System.ServiceModel;
    using System.Threading;
    using Neovolve.Toolkit.Communication;
    using Neovolve.Toolkit.Communication.Security;

    /// <summary>
    /// The <see cref="PasswordService"/>
    ///   class is the service implementation for testing the user name password security implementation.
    /// </summary>
    [ErrorHandler(typeof(DebugErrorHandler))]
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Gets the context password.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public String GetContextPassword()
        {
            if (GetIsCorrectContextIdentity())
            {
                return ((PasswordIdentity)ServiceSecurityContext.Current.PrimaryIdentity).Password.ConvertToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Gets the name of the context user.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public String GetContextUserName()
        {
            return ServiceSecurityContext.Current.PrimaryIdentity.Name;
        }

        /// <summary>
        /// Gets the is correct context identity.
        /// </summary>
        /// <returns>
        /// A <see cref="Boolean"/> value.
        /// </returns>
        public Boolean GetIsCorrectContextIdentity()
        {
            return ServiceSecurityContext.Current.PrimaryIdentity is PasswordIdentity;
        }

        /// <summary>
        /// Gets the is correct thread identity.
        /// </summary>
        /// <returns>
        /// A <see cref="Boolean"/> value.
        /// </returns>
        public Boolean GetIsCorrectThreadIdentity()
        {
            return Thread.CurrentPrincipal.Identity is PasswordIdentity;
        }

        /// <summary>
        /// Gets the thread password.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public String GetThreadPassword()
        {
            if (GetIsCorrectThreadIdentity())
            {
                return ((PasswordIdentity)Thread.CurrentPrincipal.Identity).Password.ConvertToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Gets the name of the thread user.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public String GetThreadUserName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }

        /// <summary>
        /// Sleeps the specified milliseconds.
        /// </summary>
        /// <param name="milliseconds">
        /// The milliseconds.
        /// </param>
        public void Sleep(Int32 milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}