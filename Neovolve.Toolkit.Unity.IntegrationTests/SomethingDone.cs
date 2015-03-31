namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;

    /// <summary>
    /// The <see cref="SomethingDone"/>
    ///   class is used to test dependency injection configuration.
    /// </summary>
    public class SomethingDone : IDoSomething
    {
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public String Execute()
        {
            return "Some random value";
        }
    }
}