namespace Neovolve.Toolkit.Communication.Security
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Claims;
    using System.IdentityModel.Policy;
    using System.Security;
    using System.Security.Principal;

    /// <summary>
    /// The <see cref="PasswordAuthorizationPolicy"/>
    ///   class is used to set up the security context of a WCF service call
    ///   to allow the <see cref="PasswordIdentity"/> to be made available.
    /// </summary>
    internal class PasswordAuthorizationPolicy : IAuthorizationPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordAuthorizationPolicy"/> class.
        /// </summary>
        /// <param name="userName">
        /// Name of the user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        public PasswordAuthorizationPolicy(String userName, SecureString password)
        {
            Id = Guid.NewGuid().ToString();
            Issuer = ClaimSet.System;
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Evaluates whether a user meets the requirements for this authorization policy.
        /// </summary>
        /// <param name="evaluationContext">
        /// An <see cref="EvaluationContext"/> that contains the claim set that the authorization policy evaluates.
        /// </param>
        /// <param name="state">
        /// A <see cref="Object"/>, passed by reference that represents the custom state for this authorization policy.
        /// </param>
        /// <returns>
        /// <c>false</c> if the <see cref="M:System.IdentityModel.Policy.IAuthorizationPolicy.Evaluate(System.IdentityModel.Policy.EvaluationContext,System.Object@)"/> method for this authorization policy must be called if additional claims are added by other authorization policies to <paramref name="evaluationContext"/>; otherwise, true to state no additional evaluation is required by this authorization policy.
        /// </returns>
        public Boolean Evaluate(EvaluationContext evaluationContext, ref Object state)
        {
            if (String.IsNullOrEmpty(UserName))
            {
                // There is no user name to match to an identity
                return false;
            }

            if (evaluationContext == null)
            {
                return false;
            }

            // Check if there are properties to reference
            if (evaluationContext.Properties == null)
            {
                return false;
            }

            // Check if the properties of the context has the identities list 
            if (evaluationContext.Properties.Count == 0)
            {
                return false;
            }

            const String IdentitiesKey = "Identities";

            if (evaluationContext.Properties.ContainsKey(IdentitiesKey) == false)
            {
                return false;
            }

            if (evaluationContext.Properties[IdentitiesKey] == null)
            {
                return false;
            }

            // Get the identities list 
            List<IIdentity> identities = evaluationContext.Properties[IdentitiesKey] as List<IIdentity>;

            // Validate that the identities list is valid 
            if (identities == null)
            {
                return false;
            }

            // Get the current identity 
            IIdentity currentIdentity =
                identities.Find(
                    identityMatch =>
                    identityMatch is GenericIdentity && String.Equals(identityMatch.Name, UserName, StringComparison.OrdinalIgnoreCase));

            // Check if an identity was found 
            if (currentIdentity == null)
            {
                return false;
            }

            // Create new identity 
            PasswordIdentity newIdentity = new PasswordIdentity(
                UserName, Password, currentIdentity.IsAuthenticated, currentIdentity.AuthenticationType);

            const String PrimaryIdentityKey = "PrimaryIdentity";

            // Update the list and the context with the new identity 
            identities.Remove(currentIdentity);
            identities.Add(newIdentity);
            evaluationContext.Properties[PrimaryIdentityKey] = newIdentity;

            // Create a new principal for this identity 
            PasswordPrincipal newPrincipal = new PasswordPrincipal(newIdentity, null);
            const String PrincipalKey = "Principal";

            // Store the new principal in the context 
            evaluationContext.Properties[PrincipalKey] = newPrincipal;

            // This policy has successfully been evaluated and doesn't need to be called again 
            return true;
        }

        /// <summary>
        ///   Gets a string that identifies this authorization component.
        /// </summary>
        /// <value>
        ///   The Id of the policy.
        /// </value>
        /// <returns>
        ///   A string that identifies this authorization component.
        /// </returns>
        public String Id
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets a claim set that represents the issuer of the authorization policy.
        /// </summary>
        /// <value>
        ///   The issuer of the policy.
        /// </value>
        /// <returns>
        ///   A <see cref = "T:System.IdentityModel.Claims.ClaimSet" /> that represents the issuer of the authorization policy.
        /// </returns>
        public ClaimSet Issuer
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the password.
        /// </summary>
        /// <value>
        ///   The password.
        /// </value>
        private SecureString Password
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the user.
        /// </summary>
        /// <value>
        ///   The name of the user.
        /// </value>
        private String UserName
        {
            get;
            set;
        }
    }
}