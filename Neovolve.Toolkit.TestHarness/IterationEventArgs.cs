namespace Neovolve.Toolkit.TestHarness
{
    using System;

    /// <summary>
    /// The <see cref="IterationEventArgs"/>
    ///   class is used to provide iteration event argument information when raising an event.
    /// </summary>
    public class IterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IterationEventArgs"/> class.
        /// </summary>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <param name="total">
        /// The total.
        /// </param>
        public IterationEventArgs(Int32 current, Int32 total)
        {
            Current = current;
            Total = total;
        }

        /// <summary>
        ///   Gets the current.
        /// </summary>
        /// <value>
        ///   The current.
        /// </value>
        public Int32 Current
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the total.
        /// </summary>
        /// <value>
        ///   The total.
        /// </value>
        public Int32 Total
        {
            get;
            private set;
        }
    }
}