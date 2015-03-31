namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ProxyStub"/>
    ///   class is used to provide a stub to test the <see cref="DefaultProxyHandler{T}"/> class.
    /// </summary>
    public class ProxyStub : MarshalByRefObject
    {
        /// <summary>
        /// Does something.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public virtual String DoSomething(String value)
        {
            return value + " updated";
        }
    }
}