namespace Neovolve.Toolkit.TestHarness
{
    using System;

    /// <summary>
    /// The <see cref="IRepeatableAction"/>
    ///   interface defines an actions that can be invoked for testing code.
    /// </summary>
    internal interface IRepeatableAction
    {
        /// <summary>
        ///   Occurs when an iteration is completed.
        /// </summary>
        event EventHandler<IterationEventArgs> IterationCompleted;

        /// <summary>
        /// Invokes the specified iterations.
        /// </summary>
        /// <param name="iterations">
        /// The iterations.
        /// </param>
        void Invoke(Int32 iterations);

        /// <summary>
        /// Invokes the recursive.
        /// </summary>
        /// <param name="iterations">
        /// The iterations.
        /// </param>
        /// <param name="recursionDepth">
        /// The recursion depth.
        /// </param>
        void InvokeRecursive(Int32 iterations, Int32 recursionDepth);

        /// <summary>
        ///   Gets a value indicating whether this instance is recursive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recursive; otherwise, <c>false</c>.
        /// </value>
        Boolean IsRecursive
        {
            get;
        }
    }
}