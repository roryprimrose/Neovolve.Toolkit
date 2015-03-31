namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;

    /// <summary>
    /// The <see cref="ITestInstance"/>
    ///   interface is used to help test workflows.
    /// </summary>
    public interface ITestInstance : IDisposable
    {
        /// <summary>
        /// Does something.
        /// </summary>
        void DoSomething();

        /// <summary>
        /// Does something else.
        /// </summary>
        void DoSomethingElse();

        /// <summary>
        /// Tests the input.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data to test.
        /// </typeparam>
        /// <param name="value">
        /// The value.
        /// </param>
        void TestInput<T>(T value);
    }
}