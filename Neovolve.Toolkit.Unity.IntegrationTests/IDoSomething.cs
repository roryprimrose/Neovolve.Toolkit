namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;

    /// <summary>
    /// The <see cref="IDoSomething"/>
    ///   interface is used to test dependency injection configuration.
    /// </summary>
    public interface IDoSomething
    {
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        String Execute();
    }
}