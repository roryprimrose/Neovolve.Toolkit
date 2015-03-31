namespace Neovolve.Toolkit.Communication
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    /// <summary>
    /// The <see cref="DefaultProxyHandler{T}"/>
    ///   class is used to provide a default handler for invoking methods on a type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of proxy to invoke.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// The <see cref="DefaultProxyHandler{T}"/> works by invoking methods on the proxy type that is either provided
    ///     or created by the default constructor. 
    ///   </para>
    /// <note>
    /// The proxy type must be a class that inherits from <see cref="MarshalByRefObject"/>. 
    ///   </note>
    /// </remarks>
    public class DefaultProxyHandler<T> : ProxyHandler<T> where T : class
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DefaultProxyHandler{T}" /> class.
        /// </summary>
        /// <remarks>
        ///   The default constructor creates an instance of the proxy type and invokes the methods on that instance.
        /// </remarks>
        public DefaultProxyHandler()
        {
            Target = Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProxyHandler{T}"/> class.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        public DefaultProxyHandler(T target)
        {
            Target = target;
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
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override Object ExecuteMethod(MethodBase method, Object[] parameters)
        {
            Contract.Assume(method != null);

            return method.Invoke(Target, parameters);
        }

        /// <summary>
        ///   Gets or sets the target.
        /// </summary>
        /// <value>
        ///   The target.
        /// </value>
        public T Target
        {
            get;
            set;
        }
    }
}