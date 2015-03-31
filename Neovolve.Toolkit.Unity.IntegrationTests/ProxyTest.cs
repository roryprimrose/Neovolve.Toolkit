namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="ProxyTest"/>
    ///   class is used to unit test proxy injection.
    /// </summary>
    public class ProxyTest : MarshalByRefObject
    {
        /// <summary>
        /// Renders the message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void RenderMessage(String message)
        {
            Trace.WriteLine(message);
        }
    }
}