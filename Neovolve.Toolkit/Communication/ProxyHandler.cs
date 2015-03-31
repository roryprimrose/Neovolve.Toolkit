namespace Neovolve.Toolkit.Communication
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using System.Security;
    using Neovolve.Toolkit.Reflection;

    /// <summary>
    /// The <see cref="ProxyHandler{T}"/>
    ///   class is used to provide the base logic for managing the execution of methods on a proxy.
    /// </summary>
    /// <typeparam name="T">
    /// The type of proxy.
    /// </typeparam>
    public abstract class ProxyHandler<T> : RealProxy where T : class
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProxyHandler{T}" /> class.
        /// </summary>
        protected ProxyHandler()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// Initializes the proxy using the specified action.
        /// </summary>
        /// <typeparam name="TInitialize">
        /// The type of object to initialize.
        /// </typeparam>
        /// <param name="action">
        /// The action used to initialize the proxy.
        /// </param>
        public virtual void Initialize<TInitialize>(Action<TInitialize> action)
        {
            InitializeAction = action;
        }

        /// <summary>
        /// Invokes the method that is specified in the provided <see cref="T:System.Runtime.Remoting.Messaging.IMessage"/> on the remote object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">
        /// A <see cref="T:System.Runtime.Remoting.Messaging.IMessage"/> that contains a <see cref="T:System.Collections.IDictionary"/> of information about the method call.
        /// </param>
        /// <returns>
        /// The message returned by the invoked method, containing the return value and any out or ref parameters.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "Catching the generic exception is required to correctly throw it from the proxy.")]
        [SecurityCritical]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public override IMessage Invoke(IMessage msg)
        {
            Contract.Assume(msg != null);

            ReturnMessage responseMessage;
            Object response = null;
            Exception caughtException = null;

            try
            {
                String methodName = (String)msg.Properties["__MethodName"];

                if (String.IsNullOrWhiteSpace(methodName))
                {
                    throw new InvalidOperationException("No method name was found in the message.");
                }

                Type[] parameterTypes = (Type[])msg.Properties["__MethodSignature"];

                if (parameterTypes != null)
                {
                    // Strip out any null types
                    if (parameterTypes.Any(x => x == null))
                    {
                        throw new InvalidOperationException("A null parameter type was found in the message.");
                    }
                }

                MethodBase method = MethodResolver.Resolve(typeof(T), methodName, parameterTypes);

                Debug.Assert(method != null, "Method was not found on the proxy");

                Object[] parameters = (Object[])msg.Properties["__Args"];

                // Invoke the action
                response = ExecuteMethod(method, parameters);
            }
            catch (Exception ex)
            {
                // Store the caught exception
                caughtException = ex;
            }

            IMethodCallMessage message = msg as IMethodCallMessage;

            // Check if there is an exception
            if (caughtException == null)
            {
                // Return the response from the service
                responseMessage = new ReturnMessage(response, null, 0, null, message);
            }
            else
            {
                // Return the exception thrown by the service
                responseMessage = new ReturnMessage(caughtException, message);
            }

            // Return the response message
            return responseMessage;
        }

        /// <summary>
        /// Executes the method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        protected abstract Object ExecuteMethod(MethodBase method, Object[] parameters);

        /// <summary>
        ///   Gets or sets the initialize action.
        /// </summary>
        /// <value>
        ///   The initialize action.
        /// </value>
        protected MulticastDelegate InitializeAction
        {
            get;
            set;
        }
    }
}