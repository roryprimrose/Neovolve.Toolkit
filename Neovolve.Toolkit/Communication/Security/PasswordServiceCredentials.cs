namespace Neovolve.Toolkit.Communication.Security
{
    using System.Diagnostics;
    using System.IdentityModel.Policy;
    using System.IdentityModel.Selectors;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    using System.Threading;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="PasswordServiceCredentials"/>
    ///   class provides a username password security implementation for WCF services. It will generate a
    ///   <see cref="PasswordPrincipal"/> containing a <see cref="PasswordIdentity"/>
    ///   that exposes the password of the client credentials.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="PasswordServiceCredentials"/> adds a custom <see cref="IAuthorizationPolicy"/> to the service
    ///     that creates a <see cref="PasswordPrincipal"/> and <see cref="PasswordIdentity"/> for the service security context.
    ///     The new principal is available via <see cref="ServiceSecurityContext.PrimaryIdentity">ServiceSecurityContext.PrimaryIdentity</see>. 
    ///     The <see cref="PasswordPrincipal"/> is also available via <see cref="Thread.CurrentPrincipal">Thread.CurrentPrincipal</see>
    ///     if UserNamePasswordValidationMode in configuration is set to custom (see example below).
    ///   </para>
    /// <note>
    /// The <see cref="PasswordPrincipal"/> instance is created using <c>null</c> for the roles of the principal.
    ///   </note>
    /// <para>
    /// The <see cref="PasswordServiceCredentials"/> is configured with the following:
    ///     <list type="bullet">
    /// <item>
    /// <description>
    ///           Set type to the <see cref="PasswordServiceCredentials"/> class in the serviceCredentials element. 
    ///         </description>
    /// </item>
    /// <item>
    /// <description>
    ///           Set userNamePasswordValidationMode to Custom in the userNameAuthentication element to use custom authentication rather
    ///           than Windows authentication. 
    ///         </description>
    /// </item>
    /// <item>
    /// <description>
    ///           Set principalPermissionMode to Custom in the serviceAuthorization element to assign <see cref="PasswordPrincipal"/> to 
    ///           <see cref="Thread.CurrentPrincipal">Thread.CurrentPrincipal</see>.
    ///         </description>
    /// </item>
    /// <item>
    /// <description>
    ///           Set the customUserNamePasswordValidatorType in the userNameAuthentication element as required.
    ///         </description>
    /// </item>
    /// </list>
    /// </para>
    /// <note>
    /// The <see cref="DefaultPasswordValidator"/> is used if no customUserNamePasswordValidatorType is configured.
    ///   </note>
    /// <para>
    /// WCF enforces protection of the password in transmission using transport security. 
    ///     The security mode should be set to TransportWithMessageCredentials with a message client credential type as UserName.
    ///   </para>
    /// </remarks>
    /// <example>
    /// The following example shows the web.config xml to use this class.
    ///   <code lang="xml" title="Configuration">
    /// <![CDATA[<?xml version="1.0" encoding="utf-8" ?> 
    /// <configuration> 
    ///   <system.serviceModel> 
    ///     <bindings> 
    ///       <netTcpBinding> 
    ///         <binding name="netTcpBindingConfig"> 
    ///           <security mode="TransportWithMessageCredential"> 
    ///             <message clientCredentialType="UserName"/> 
    ///           </security> 
    ///         </binding> 
    ///       </netTcpBinding> 
    ///     </bindings> 
    ///     <services> 
    ///       <service behaviorConfiguration="Neovolve.Toolkit.Communication.SecurityTest.Service1Behavior" 
    ///         name="Neovolve.Toolkit.Communication.SecurityHost.Service1"> 
    ///         <endpoint address="net.tcp://localhost:8792/PasswordSecurityTest" 
    ///           binding="netTcpBinding" bindingConfiguration="netTcpBindingConfig" 
    ///           contract="Neovolve.Toolkit.Communication.SecurityHost.IService1"/> 
    ///       </service> 
    ///     </services> 
    ///     <behaviors> 
    ///       <serviceBehaviors> 
    ///         <behavior name="Neovolve.Toolkit.Communication.SecurityTest.Service1Behavior"> 
    ///           <serviceDebug includeExceptionDetailInFaults="true"/> 
    ///           <serviceCredentials type="Neovolve.Toolkit.Communication.Security.PasswordServiceCredentials, Neovolve.Toolkit.Communication.Security"> 
    ///             <serviceCertificate findValue="localhost" x509FindType="FindBySubjectName"/> 
    ///             <userNameAuthentication userNamePasswordValidationMode="Custom"
    ///               customUserNamePasswordValidatorType="Neovolve.Toolkit.Communication.Security.OptionalPasswordValidator, Neovolve.Toolkit.Communication.Security"/> 
    ///           </serviceCredentials> 
    ///           <serviceAuthorization principalPermissionMode="Custom"/> 
    ///         </behavior> 
    ///       </serviceBehaviors> 
    ///     </behaviors> 
    ///   </system.serviceModel> 
    /// </configuration>]]>
    ///   </code>
    /// </example>
    /// <seealso cref="ServiceSecurityContext"/>
    public class PasswordServiceCredentials : ServiceCredentials
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PasswordServiceCredentials" /> class.
        /// </summary>
        public PasswordServiceCredentials()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordServiceCredentials"/> class.
        /// </summary>
        /// <param name="clone">
        /// The clone.
        /// </param>
        private PasswordServiceCredentials(ServiceCredentials clone)
            : base(clone)
        {
        }

        /// <summary>
        /// Creates a token manager for this service.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Security.ServiceCredentialsSecurityTokenManager"/> instance.
        /// </returns>
        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            // Check if the current validation mode is for custom username password validation 
            if (UserNameAuthentication.UserNamePasswordValidationMode == UserNamePasswordValidationMode.Custom)
            {
                return new PasswordSecurityTokenManager(this);
            }

            Trace.TraceWarning(Resources.CustomUserNamePasswordValidationNotEnabled);

            return base.CreateSecurityTokenManager();
        }

        /// <summary>
        /// Copies the essential members of the current instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Description.ServiceCredentials"/> instance.
        /// </returns>
        protected override ServiceCredentials CloneCore()
        {
            return new PasswordServiceCredentials(this);
        }
    }
}