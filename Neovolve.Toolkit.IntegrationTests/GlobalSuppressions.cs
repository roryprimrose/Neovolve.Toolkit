// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.IPasswordService.#GetContextPassword()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.IPasswordService.#GetContextUserName()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.IPasswordService.#GetIsCorrectContextIdentity()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.IPasswordService.#GetIsCorrectThreadIdentity()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.IPasswordService.#GetThreadPassword()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.IPasswordService.#GetThreadUserName()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.ErrorHandlerAttributeTests.#ApplyDispatchBehaviorTest()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.PasswordServiceCredentialsTests.#RunPasswordServiceCredentialsTest(System.IdentityModel.Selectors.UserNamePasswordValidator,System.ServiceModel.Description.PrincipalPermissionMode,System.String,System.ServiceModel.Security.UserNamePasswordValidationMode)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Scope = "type", Target = "Neovolve.Toolkit.IntegrationTests.Communication.IUnknownService")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Neovolve.Toolkit.IntegrationTests.Communication.KnownFault.#ctor(System.String)", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.KnownErrorHandler.#ProvideFault(System.Exception,System.ServiceModel.Channels.MessageVersion,System.ServiceModel.Channels.Message&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Neovolve.Toolkit.IntegrationTests.Communication.UnknownFault.#ctor(System.String)", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.UnknownErrorHandler.#ProvideFault(System.Exception,System.ServiceModel.Channels.MessageVersion,System.ServiceModel.Channels.Message&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Neovolve.Toolkit.IntegrationTests.Communication.Security.PasswordServiceCredentialsTests.#CreateServiceHost(System.ServiceModel.Channels.Binding,System.IdentityModel.Selectors.UserNamePasswordValidator,System.ServiceModel.Security.UserNamePasswordValidationMode,System.ServiceModel.Description.PrincipalPermissionMode,System.Uri)")]
