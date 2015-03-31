namespace Neovolve.Toolkit.UnitTests.Reflection
{
    using System;
    using Neovolve.Toolkit.Reflection;

    /// <summary>
    /// The <see cref="ITester"/>
    ///   interface is used for testing the <see cref="MethodResolver"/> class.
    /// </summary>
    internal interface ITester : IFormattable, IDisposable
    {
        /// <summary>
        /// Does something.
        /// </summary>
        void DoSomething();
    }
}