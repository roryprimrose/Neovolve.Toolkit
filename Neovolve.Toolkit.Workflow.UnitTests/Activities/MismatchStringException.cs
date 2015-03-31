namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="MismatchStringException"/>
    ///   class is used to test the <see cref="SystemFailureEvaluator"/>.
    /// </summary>
    public class MismatchStringException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchStringException"/> class.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        public MismatchStringException(String first, String second)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchStringException"/> class.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        public MismatchStringException(String first)
        {
        }
    }
}