namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;

    /// <summary>
    /// The <see cref="TestProxyTarget"/>
    ///   class is used to support testing proxy injection.
    /// </summary>
    public class TestProxyTarget : MarshalByRefObject
    {
        /// <summary>
        /// Does something.
        /// </summary>
        /// <param name="withThisValue">
        /// The with this value.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public String DoSomething(String withThisValue)
        {
            return Guid.NewGuid().ToString();
        }
    }
}