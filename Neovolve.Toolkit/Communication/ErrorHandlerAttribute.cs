namespace Neovolve.Toolkit.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The <see cref="ErrorHandlerAttribute"/>
    ///   class is used to decorate WCF service implementations with a 
    ///   <see cref="IServiceBehavior"/> that identifies <see cref="IErrorHandler"/> references to invoke
    ///   when the service encounters errors.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IErrorHandler"/> implementations are often set up for services using configuration.
    ///     This may create risks in cases where configuration is not correctly defined or configuration values are missing.
    ///     The result of this may be that exception shielding and error handling are not functioning as intended.
    ///   </para>
    /// <para>
    /// Lack of exception shielding may be a security risk because exception information crosses the service boundary
    ///     that was not intended. The details of the exception may contain sensitive information that the consumers of
    ///     the services should not be able to read.
    ///   </para>
    /// <para>
    /// Lack of error handling may not be a security risk, but will produce an unexpected behaviour for clients as 
    ///     they consume the service. According to the contract, the client may expect to see a particular fault exception
    ///     being raised, but may instead receive a different fault.
    ///   </para>
    /// <note>
    /// The constructor overloads that accept a <c>params</c> array are not CLS compliant. Usage of attributes that use array arguments
    ///     breaks CLS compliance. These methods have been appropriately marked as not being CLS compliant.
    ///   </note>
    /// <para>
    /// The constructor of the attribute instance either takes a single type or string, or a <c>params</c> array of strings or types.
    ///     This allows for a single error handler to be assigned in a CLS compliant way, or multiple error handlers to be assigned
    ///     in a non-CLS compliant way.
    ///   </para>
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use this attribute on a service implementation.
    ///   <code lang="C#">
    /// <![CDATA[
    ///   // In this case, the ErrorHandler attribute is used to assign single error handler
    ///   [ErrorHandler(typeof(KnownErrorHandler))]
    ///   public class TestService : ITestService
    ///   {
    ///   } 
    /// 
    ///   // In this case, the ErrorHandler attribute is used to assign multiple error handlers
    ///   // Note: Passing multiple handlers is not CLS compliant
    ///   [CLSCompliant(false)]
    ///   [ErrorHandler(typeof(KnownErrorHandler), 
    ///       typeof(UnknownErrorHandler), 
    ///       typeof(AnotherErrorHandler), 
    ///       typeof(YetAnotherErrorHandler))]
    ///   public class AnotherTestService : ITestService
    ///   {
    ///   } 
    /// ]]>
    ///   </code>
    /// </example>
    /// <seealso cref="ErrorHandlerElement"/>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", 
        Justification = "The type is stored in a list and cannot be exposed via a property.")]
    [AttributeUsage(AttributeTargets.Class)]
    [CLSCompliant(false)]
    public sealed class ErrorHandlerAttribute : Attribute, IServiceBehavior
    {
        /// <overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class.
        ///   </summary>
        /// </overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler type.
        /// </summary>
        /// <param name="errorHandler">
        /// Type of the error handler.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='AttributeConstructorCLSCompliant']/*"/>
        /// </remarks>
        public ErrorHandlerAttribute(Type errorHandler)
        {
            Contract.Requires<ArgumentNullException>(errorHandler != null);
            Contract.Requires<InvalidCastException>(typeof(IErrorHandler).IsAssignableFrom(errorHandler));

            Type[] errorHandlerTypes = new[]
                                       {
                                           errorHandler
                                       };

            Initialize(errorHandlerTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler types.
        /// </summary>
        /// <param name="firstErrorHandler">
        /// The first error handler.
        /// </param>
        /// <param name="secondErrorHandler">
        /// The second error handler.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='AttributeConstructorCLSCompliant']/*"/>
        /// </remarks>
        public ErrorHandlerAttribute(Type firstErrorHandler, Type secondErrorHandler)
        {
            Contract.Requires<ArgumentNullException>(firstErrorHandler != null);
            Contract.Requires<ArgumentNullException>(secondErrorHandler != null);
            Contract.Requires<InvalidCastException>(typeof(IErrorHandler).IsAssignableFrom(firstErrorHandler));
            Contract.Requires<InvalidCastException>(typeof(IErrorHandler).IsAssignableFrom(secondErrorHandler));

            Type[] errorHandlerTypes = new[]
                                       {
                                           firstErrorHandler, secondErrorHandler
                                       };

            Initialize(errorHandlerTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler types.
        /// </summary>
        /// <param name="firstErrorHandler">
        /// The first error handler.
        /// </param>
        /// <param name="secondErrorHandler">
        /// The second error handler.
        /// </param>
        /// <param name="thirdErrorHandler">
        /// The third error handler.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='AttributeConstructorCLSCompliant']/*"/>
        /// </remarks>
        public ErrorHandlerAttribute(Type firstErrorHandler, Type secondErrorHandler, Type thirdErrorHandler)
        {
            Contract.Requires<ArgumentNullException>(firstErrorHandler != null);
            Contract.Requires<ArgumentNullException>(secondErrorHandler != null);
            Contract.Requires<ArgumentNullException>(thirdErrorHandler != null);
            Contract.Requires<InvalidCastException>(typeof(IErrorHandler).IsAssignableFrom(firstErrorHandler));
            Contract.Requires<InvalidCastException>(typeof(IErrorHandler).IsAssignableFrom(secondErrorHandler));
            Contract.Requires<InvalidCastException>(typeof(IErrorHandler).IsAssignableFrom(thirdErrorHandler));

            Type[] errorHandlerTypes = new[]
                                       {
                                           firstErrorHandler, secondErrorHandler, thirdErrorHandler
                                       };

            Initialize(errorHandlerTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler types.
        /// </summary>
        /// <param name="errorHandlerTypes">
        /// The error handler types.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='MethodNotCLSCompliant']/*"/>
        /// </remarks>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public ErrorHandlerAttribute(params Type[] errorHandlerTypes)
        {
            Contract.Requires<ArgumentNullException>(errorHandlerTypes != null);
            Contract.Requires<ArgumentOutOfRangeException>(errorHandlerTypes.Length > 0);
            Contract.Requires<ArgumentNullException>(Contract.ForAll(errorHandlerTypes, x => x != null));
            Contract.Requires<InvalidCastException>(Contract.ForAll(errorHandlerTypes, x => typeof(IErrorHandler).IsAssignableFrom(x)));

            Initialize(errorHandlerTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler type name.
        /// </summary>
        /// <param name="errorHandlerTypeName">
        /// Name of the error handler type.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='AttributeConstructorCLSCompliant']/*"/>
        /// </remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='TypeLoadException']/*"/>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='InvalidCastExceptionErrorHandlerType']/*"/>
        public ErrorHandlerAttribute(String errorHandlerTypeName)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(errorHandlerTypeName) == false);

            String[] errorHandlerTypeNames = new[]
                                             {
                                                 errorHandlerTypeName
                                             };

            Contract.Assume(Contract.ForAll(errorHandlerTypeNames, x => String.IsNullOrWhiteSpace(x) == false));

            Initialize(errorHandlerTypeNames);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler type names.
        /// </summary>
        /// <param name="firstErrorHandlerTypeName">
        /// Name of the first error handler type.
        /// </param>
        /// <param name="secondErrorHandlerTypeName">
        /// Name of the second error handler type.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='AttributeConstructorCLSCompliant']/*"/>
        /// </remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='TypeLoadException']/*"/>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='InvalidCastExceptionErrorHandlerType']/*"/>
        public ErrorHandlerAttribute(String firstErrorHandlerTypeName, String secondErrorHandlerTypeName)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(firstErrorHandlerTypeName) == false);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(secondErrorHandlerTypeName) == false);
            Contract.Requires<ArgumentException>(firstErrorHandlerTypeName != secondErrorHandlerTypeName);

            String[] errorHandlerTypeNames = new[]
                                             {
                                                 firstErrorHandlerTypeName, secondErrorHandlerTypeName
                                             };

            Contract.Assume(Contract.ForAll(errorHandlerTypeNames, x => String.IsNullOrWhiteSpace(x) == false));

            Initialize(errorHandlerTypeNames);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler type names.
        /// </summary>
        /// <param name="firstErrorHandlerTypeName">
        /// Name of the first error handler type.
        /// </param>
        /// <param name="secondErrorHandlerTypeName">
        /// Name of the second error handler type.
        /// </param>
        /// <param name="thirdErrorHandlerTypeName">
        /// Name of the third error handler type.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='AttributeConstructorCLSCompliant']/*"/>
        /// </remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='TypeLoadException']/*"/>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='InvalidCastExceptionErrorHandlerType']/*"/>
        public ErrorHandlerAttribute(String firstErrorHandlerTypeName, String secondErrorHandlerTypeName, String thirdErrorHandlerTypeName)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(firstErrorHandlerTypeName) == false);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(secondErrorHandlerTypeName) == false);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(thirdErrorHandlerTypeName) == false);
            Contract.Requires<ArgumentException>(firstErrorHandlerTypeName != secondErrorHandlerTypeName);
            Contract.Requires<ArgumentException>(secondErrorHandlerTypeName != thirdErrorHandlerTypeName);

            String[] errorHandlerTypeNames = new[]
                                             {
                                                 firstErrorHandlerTypeName, secondErrorHandlerTypeName, thirdErrorHandlerTypeName
                                             };

            Contract.Assume(Contract.ForAll(errorHandlerTypeNames, x => String.IsNullOrWhiteSpace(x) == false));

            Initialize(errorHandlerTypeNames);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerAttribute"/> class using the provided error handler type names.
        /// </summary>
        /// <param name="errorHandlerTypeNames">
        /// The error handler type names.
        /// </param>
        /// <remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Remarks/Remark[@name='MethodNotCLSCompliant']/*"/>
        /// </remarks>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='InvalidCastExceptionErrorHandlerType']/*"/>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public ErrorHandlerAttribute(params String[] errorHandlerTypeNames)
        {
            Contract.Requires<ArgumentNullException>(errorHandlerTypeNames != null);
            Contract.Requires<ArgumentOutOfRangeException>(errorHandlerTypeNames.Length > 0);
            Contract.Requires<ArgumentNullException>(Contract.ForAll(errorHandlerTypeNames, x => String.IsNullOrWhiteSpace(x) == false));

            Initialize(errorHandlerTypeNames);
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description of the service.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host of the service.
        /// </param>
        /// <param name="endpoints">
        /// The service endpoints.
        /// </param>
        /// <param name="bindingParameters">
        /// Custom objects to which binding elements have access.
        /// </param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
            // Nothing to do here
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host that is currently being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="serviceHostBase"/> value is <c>null</c>.
        /// </exception>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceHostBase == null)
            {
                throw new ArgumentNullException("serviceHostBase");
            }

            // Loop through each channel dispatcher
            for (Int32 dispatcherIndex = 0; dispatcherIndex < serviceHostBase.ChannelDispatchers.Count; dispatcherIndex++)
            {
                // Get the dispatcher for this index and cast to the type we are after
                ChannelDispatcher dispatcher = (ChannelDispatcher)serviceHostBase.ChannelDispatchers[dispatcherIndex];

                // Loop through each error handler
                for (Int32 typeIndex = 0; typeIndex < ErrorHandlerTypes.Count; typeIndex++)
                {
                    Type errorHandlerType = ErrorHandlerTypes[typeIndex];

                    if (errorHandlerType == null)
                    {
                        continue;
                    }

                    // Create a new error handler instance
                    IErrorHandler handler = (IErrorHandler)Activator.CreateInstance(errorHandlerType);

                    // Add the handler to the dispatcher
                    dispatcher.ErrorHandlers.Add(handler);
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The service host that is currently being constructed.
        /// </param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Nothing to do here
        }

        /// <summary>
        /// Initializes the specified error handler types.
        /// </summary>
        /// <param name="errorHandlerTypes">
        /// The error handler types.
        /// </param>
        private void Initialize(Type[] errorHandlerTypes)
        {
            Contract.Requires<ArgumentNullException>(errorHandlerTypes != null);
            Contract.Requires<ArgumentOutOfRangeException>(errorHandlerTypes.Length > 0);

            List<String> typeNames = new List<String>(errorHandlerTypes.Length);

            // Loop through each item supplied
            for (Int32 index = 0; index < errorHandlerTypes.Length; index++)
            {
                Type errorHandlerType = errorHandlerTypes[index];

                Contract.Assume(errorHandlerType != null);
                Contract.Assume(typeof(IErrorHandler).IsAssignableFrom(errorHandlerType));

                String assemblyQualifiedName = errorHandlerType.AssemblyQualifiedName;

                if (typeNames.Contains(assemblyQualifiedName) == false)
                {
                    typeNames.Add(assemblyQualifiedName);
                }
            }

            // Store the types
            ErrorHandlerTypes = new ReadOnlyCollection<Type>(errorHandlerTypes);
        }

        /// <summary>
        /// Initializes the specified error handler type names.
        /// </summary>
        /// <param name="errorHandlerTypeNames">
        /// The error handler type names.
        /// </param>
        private void Initialize(String[] errorHandlerTypeNames)
        {
            Contract.Requires<ArgumentNullException>(errorHandlerTypeNames != null);
            Contract.Requires<ArgumentOutOfRangeException>(errorHandlerTypeNames.Length > 0);

            IEnumerable<String> providedTypeNames = from x in errorHandlerTypeNames
                                                    where String.IsNullOrWhiteSpace(x) == false
                                                    select x;
            List<Type> loadedTypes = new List<Type>();

            // Loop through each type
            foreach (String typeName in providedTypeNames)
            {
                Type handlerType = Type.GetType(typeName, true, true);

                if (typeof(IErrorHandler).IsAssignableFrom(handlerType) == false)
                {
                    throw new InvalidCastException();
                }

                loadedTypes.Add(handlerType);
            }

            Type[] types = loadedTypes.ToArray();

            Contract.Assume(types.Length > 0);

            Initialize(types);
        }

        /// <summary>
        ///   Gets the error handler types.
        /// </summary>
        /// <value>
        ///   The error handler types.
        /// </value>
        public ReadOnlyCollection<Type> ErrorHandlerTypes
        {
            get;
            private set;
        }
    }
}