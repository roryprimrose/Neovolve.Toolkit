namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="SystemFailureEvaluator"/>
    ///   class is used to evaluate a condition to determine whether a system failure has occurred.
    /// </summary>
    public sealed class SystemFailureEvaluator : CodeActivity
    {
        /// <summary>
        ///   Defines the message parameter name.
        /// </summary>
        private const String MessageParameterName = "message";

        /// <summary>
        ///   Defines the paramName parameter name.
        /// </summary>
        private const String ParamNameParameterName = "paramName";

        /// <summary>
        /// Creates and validates a description of the activity’s arguments, variables, child activities, and activity delegates.
        /// </summary>
        /// <param name="metadata">
        /// The activity’s metadata that encapsulates the activity’s arguments, variables, child activities, and activity delegates.
        /// </param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            RuntimeArgument conditionArgument = new RuntimeArgument("Condition", typeof(Nullable<Boolean>), ArgumentDirection.In);
            RuntimeArgument exceptionTypeArgument = new RuntimeArgument("ExceptionType", typeof(Type), ArgumentDirection.In);
            RuntimeArgument messageArgument = new RuntimeArgument("Message", typeof(String), ArgumentDirection.In);
            RuntimeArgument parameterNameArgument = new RuntimeArgument("ParameterName", typeof(String), ArgumentDirection.In);

            metadata.Bind(Condition, conditionArgument);
            metadata.Bind(ExceptionType, exceptionTypeArgument);
            metadata.Bind(Message, messageArgument);
            metadata.Bind(ParameterName, parameterNameArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        conditionArgument, 
                                                        exceptionTypeArgument, 
                                                        messageArgument, 
                                                        parameterNameArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);
        }

        /// <summary>
        /// Performs the execution of the activity.
        /// </summary>
        /// <param name="context">
        /// The execution context under which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(CodeActivityContext context)
        {
            Contract.Assume(context != null);

            Nullable<Boolean> condition = context.GetValue(Condition);

            if (condition != null && condition.Value == false)
            {
                return;
            }

            String parameterName = context.GetValue(ParameterName);
            String message = context.GetValue(Message);
            Type exceptionType = context.GetValue(ExceptionType);

            if (exceptionType == null)
            {
                exceptionType = typeof(SystemFailureException);
            }

            Exception exceptionToThrow = DetermineExceptionToThrow(exceptionType, parameterName, message);

            throw exceptionToThrow;
        }

        /// <summary>
        /// Creates the parameter name message exception.
        /// </summary>
        /// <param name="exceptionType">
        /// Type of the exception.
        /// </param>
        /// <param name="parameterName">
        /// Name of the parameter.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// A <see cref="Exception"/> instance.
        /// </returns>
        private static Exception CreateParameterNameMessageException(Type exceptionType, String parameterName, String message)
        {
            Type[] parameterTypes = new[]
                                    {
                                        typeof(String), typeof(String)
                                    };
            ConstructorInfo matchingConstructor = exceptionType.GetConstructor(parameterTypes);

            if (matchingConstructor == null)
            {
                String noConstructorFoundMessage = String.Format(
                    CultureInfo.InvariantCulture, Resources.SystemFailureEvaluator_ParamNameMessageConstructorNotFound, exceptionType.FullName);

                throw new MissingMemberException(noConstructorFoundMessage);
            }

            ParameterInfo[] parameters = matchingConstructor.GetParameters();

            if (parameters[0].Name == ParamNameParameterName && parameters[1].Name == MessageParameterName)
            {
                Object[] parameterValues = new Object[]
                                           {
                                               parameterName, message
                                           };

                return (Exception)matchingConstructor.Invoke(parameterValues);
            }

            if (parameters[0].Name == MessageParameterName && parameters[1].Name == ParamNameParameterName)
            {
                Object[] parameterValues = new Object[]
                                           {
                                               message, parameterName
                                           };

                return (Exception)matchingConstructor.Invoke(parameterValues);
            }

            String invalidConstructorFoundMessage = String.Format(
                CultureInfo.InvariantCulture, Resources.SystemFailureEvaluator_ParamNameMessageConstructorNotFound, exceptionType.FullName);

            throw new MissingMemberException(invalidConstructorFoundMessage);
        }

        /// <summary>
        /// Creates the string exception.
        /// </summary>
        /// <param name="exceptionType">
        /// Type of the exception.
        /// </param>
        /// <param name="parameterName">
        /// Name of the parameter.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// A <see cref="Exception"/> instance.
        /// </returns>
        private static Exception CreateStringException(Type exceptionType, String parameterName, String message)
        {
            Type[] parameterTypes = new[]
                                    {
                                        typeof(String)
                                    };
            ConstructorInfo matchingConstructor = exceptionType.GetConstructor(parameterTypes);

            if (matchingConstructor == null)
            {
                String parameterFailureMessage;

                if (String.IsNullOrWhiteSpace(parameterName))
                {
                    parameterFailureMessage = Resources.SystemFailureEvaluator_MessageConstructorNotFound;
                }
                else
                {
                    parameterFailureMessage = Resources.SystemFailureEvaluator_ParamNameConstructorNotFound;
                }

                String noConstructorFoundMessage = String.Format(CultureInfo.InvariantCulture, parameterFailureMessage, exceptionType.FullName);

                throw new MissingMemberException(noConstructorFoundMessage);
            }

            ParameterInfo[] parameters = matchingConstructor.GetParameters();

            if (parameters[0].Name == ParamNameParameterName)
            {
                Object[] parameterValues = new Object[]
                                           {
                                               parameterName
                                           };

                return (Exception)matchingConstructor.Invoke(parameterValues);
            }

            if (parameters[0].Name == MessageParameterName)
            {
                Object[] parameterValues = new Object[]
                                           {
                                               message
                                           };

                return (Exception)matchingConstructor.Invoke(parameterValues);
            }

            String failureMessage;

            if (String.IsNullOrWhiteSpace(parameterName))
            {
                failureMessage = Resources.SystemFailureEvaluator_MessageConstructorNotFound;
            }
            else
            {
                failureMessage = Resources.SystemFailureEvaluator_ParamNameConstructorNotFound;
            }

            String invalidConstructorFoundMessage = String.Format(CultureInfo.InvariantCulture, failureMessage, exceptionType.FullName);

            throw new MissingMemberException(invalidConstructorFoundMessage);
        }

        /// <summary>
        /// Determines the exception to throw.
        /// </summary>
        /// <param name="exceptionType">
        /// Type of the exception.
        /// </param>
        /// <param name="parameterName">
        /// Name of the parameter.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// A <see cref="Exception"/> instance.
        /// </returns>
        private static Exception DetermineExceptionToThrow(Type exceptionType, String parameterName, String message)
        {
            Contract.Requires<ArgumentNullException>(exceptionType != null);

            Boolean parameterNameProvided = String.IsNullOrWhiteSpace(parameterName) == false;
            Boolean messageProvided = String.IsNullOrWhiteSpace(message) == false;

            if (parameterNameProvided && messageProvided)
            {
                // Look for a constructor with paramName and message
                return CreateParameterNameMessageException(exceptionType, parameterName, message);
            }

            if (parameterNameProvided)
            {
                // Look for a constructor with paramName
                return CreateStringException(exceptionType, parameterName, null);
            }

            if (messageProvided)
            {
                // Look for a constructor with message
                return CreateStringException(exceptionType, null, message);
            }

            return (Exception)Activator.CreateInstance(exceptionType);
        }

        /// <summary>
        ///   Gets or sets the condition.
        /// </summary>
        /// <value>
        ///   The condition.
        /// </value>
        [Category("Input")]
        [Description("Defines the optional condition used to determine whether an exception is raised")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "Workflow forces a generic type to be wrapped in a InArgument.")]
        [DefaultValue((String)null)]
        public InArgument<Nullable<Boolean>> Condition
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the type of the exception.
        /// </summary>
        /// <value>
        ///   The type of the exception.
        /// </value>
        [Category("Input")]
        [Description("The type of exception to throw. Default is ArgumentNullException.")]
        [DefaultValue((String)null)]
        public InArgument<Type> ExceptionType
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the message.
        /// </summary>
        /// <value>
        ///   The message.
        /// </value>
        [Category("Input")]
        [Description("Defines the message for the failure")]
        [DefaultValue((String)null)]
        public InArgument<String> Message
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        ///   The name of the parameter.
        /// </value>
        [Category("Input")]
        [Description("Defines the parameter name being checked")]
        [DefaultValue((String)null)]
        public InArgument<String> ParameterName
        {
            get;
            set;
        }
    }
}