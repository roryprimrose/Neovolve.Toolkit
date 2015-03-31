namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using System.Reflection;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ProxyHandlerWrapper&lt;T&gt;"/>
    ///   class is used to provide a wrapper implementation for <see cref="ProxyHandler{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type being proxied.
    /// </typeparam>
    internal class ProxyHandlerWrapper<T> : ProxyHandler<T> where T : class
    {
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
        protected override Object ExecuteMethod(MethodBase method, Object[] parameters)
        {
            return null;
        }
    }
}