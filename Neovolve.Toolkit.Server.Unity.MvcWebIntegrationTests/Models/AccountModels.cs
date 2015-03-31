namespace Neovolve.Toolkit.Server.Unity.MvcWebIntegrationTests.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Web.Security;

    #region Models

    /// <summary>
    /// The change password model.
    /// </summary>
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        /// <summary>
        ///   Gets or sets ConfirmPassword.
        /// </summary>
        /// <value>
        ///   The confirm password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public String ConfirmPassword
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets NewPassword.
        /// </summary>
        /// <value>
        ///   The new password.
        /// </value>
        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public String NewPassword
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets OldPassword.
        /// </summary>
        /// <value>
        ///   The old password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public String OldPassword
        {
            get;
            set;
        }
    }

    /// <summary>
    /// The log on model.
    /// </summary>
    public class LogOnModel
    {
        /// <summary>
        ///   Gets or sets Password.
        /// </summary>
        /// <value>
        ///   The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public String Password
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether RememberMe.
        /// </summary>
        /// <value>
        ///   The remember me.
        /// </value>
        [DisplayName("Remember me?")]
        public Boolean RememberMe
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets UserName.
        /// </summary>
        /// <value>
        ///   The user name.
        /// </value>
        [Required]
        [DisplayName("User name")]
        public String UserName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// The register model.
    /// </summary>
    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class RegisterModel
    {
        /// <summary>
        ///   Gets or sets ConfirmPassword.
        /// </summary>
        /// <value>
        ///   The confirm password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public String ConfirmPassword
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets Email.
        /// </summary>
        /// <value>
        ///   The email.
        /// </value>
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public String Email
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets Password.
        /// </summary>
        /// <value>
        ///   The password.
        /// </value>
        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public String Password
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets UserName.
        /// </summary>
        /// <value>
        ///   The user name.
        /// </value>
        [Required]
        [DisplayName("User name")]
        public String UserName
        {
            get;
            set;
        }
    }

    #endregion

    #region Services

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    /// <summary>
    /// The i membership service.
    /// </summary>
    public interface IMembershipService
    {
        /// <summary>
        /// The change password.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="oldPassword">
        /// The old password.
        /// </param>
        /// <param name="newPassword">
        /// The new password.
        /// </param>
        /// <returns>
        /// The change password.
        /// </returns>
        Boolean ChangePassword(String userName, String oldPassword, String newPassword);

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// </returns>
        MembershipCreateStatus CreateUser(String userName, String password, String email);

        /// <summary>
        /// The validate user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The validate user.
        /// </returns>
        Boolean ValidateUser(String userName, String password);

        /// <summary>
        ///   Gets MinPasswordLength.
        /// </summary>
        /// <value>
        ///   The min password length.
        /// </value>
        Int32 MinPasswordLength
        {
            get;
        }
    }

    /// <summary>
    /// The account membership service.
    /// </summary>
    public class AccountMembershipService : IMembershipService
    {
        /// <summary>
        ///   The _provider.
        /// </summary>
        private readonly MembershipProvider _provider;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AccountMembershipService" /> class.
        /// </summary>
        public AccountMembershipService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountMembershipService"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        /// <summary>
        /// The change password.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="oldPassword">
        /// The old password.
        /// </param>
        /// <param name="newPassword">
        /// The new password.
        /// </param>
        /// <returns>
        /// The change password.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public Boolean ChangePassword(String userName, String oldPassword, String newPassword)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (String.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            }

            if (String.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Value cannot be null or empty.", "newPassword");
            }

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public MembershipCreateStatus CreateUser(String userName, String password, String email)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be null or empty.", "password");
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Value cannot be null or empty.", "email");
            }

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        /// <summary>
        /// The validate user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The validate user.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public Boolean ValidateUser(String userName, String password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be null or empty.", "password");
            }

            return _provider.ValidateUser(userName, password);
        }

        /// <summary>
        ///   Gets MinPasswordLength.
        /// </summary>
        /// <value>
        ///   The min password length.
        /// </value>
        public Int32 MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }
    }

    /// <summary>
    /// The i forms authentication service.
    /// </summary>
    public interface IFormsAuthenticationService
    {
        /// <summary>
        /// The sign in.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="createPersistentCookie">
        /// The create persistent cookie.
        /// </param>
        void SignIn(String userName, Boolean createPersistentCookie);

        /// <summary>
        /// The sign out.
        /// </summary>
        void SignOut();
    }

    /// <summary>
    /// The forms authentication service.
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        /// <summary>
        /// The sign in.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="createPersistentCookie">
        /// The create persistent cookie.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public void SignIn(String userName, Boolean createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        /// <summary>
        /// The sign out.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    #endregion

    #region Validation

    /// <summary>
    /// The account validation.
    /// </summary>
    public static class AccountValidation
    {
        /// <summary>
        /// The error code to string.
        /// </summary>
        /// <param name="createStatus">
        /// The create status.
        /// </param>
        /// <returns>
        /// The error code to string.
        /// </returns>
        public static String ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    /// <summary>
    /// The properties must match attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        /// <summary>
        ///   The _default error message.
        /// </summary>
        private const String _defaultErrorMessage = "'{0}' and '{1}' do not match.";

        /// <summary>
        ///   The _type id.
        /// </summary>
        private readonly Object _typeId = new Object();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesMustMatchAttribute"/> class.
        /// </summary>
        /// <param name="originalProperty">
        /// The original property.
        /// </param>
        /// <param name="confirmProperty">
        /// The confirm property.
        /// </param>
        public PropertiesMustMatchAttribute(String originalProperty, String confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        /// <summary>
        /// The format error message.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The format error message.
        /// </returns>
        public override String FormatErrorMessage(String name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, OriginalProperty, ConfirmProperty);
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The is valid.
        /// </returns>
        public override Boolean IsValid(Object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            Object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            Object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }

        /// <summary>
        ///   Gets ConfirmProperty.
        /// </summary>
        /// <value>
        ///   The confirm property.
        /// </value>
        public String ConfirmProperty
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets OriginalProperty.
        /// </summary>
        /// <value>
        ///   The original property.
        /// </value>
        public String OriginalProperty
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets TypeId.
        /// </summary>
        /// <value>
        ///   The type id.
        /// </value>
        public override Object TypeId
        {
            get
            {
                return _typeId;
            }
        }
    }

    /// <summary>
    /// The validate password length attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        /// <summary>
        ///   The _default error message.
        /// </summary>
        private const String _defaultErrorMessage = "'{0}' must be at least {1} characters long.";

        /// <summary>
        ///   The _min characters.
        /// </summary>
        private readonly Int32 _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ValidatePasswordLengthAttribute" /> class.
        /// </summary>
        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        /// <summary>
        /// The format error message.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The format error message.
        /// </returns>
        public override String FormatErrorMessage(String name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _minCharacters);
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The is valid.
        /// </returns>
        public override Boolean IsValid(Object value)
        {
            String valueAsString = value as String;
            return valueAsString != null && valueAsString.Length >= _minCharacters;
        }
    }

    #endregion
}