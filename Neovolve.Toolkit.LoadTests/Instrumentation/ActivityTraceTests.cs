namespace Neovolve.Toolkit.LoadTests.Instrumentation
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="ActivityTraceTests"/>
    ///   class is used to test the <see cref="ActivityTrace"/> class.
    /// </summary>
    [TestClass]
    public class ActivityTraceTests
    {
        /// <summary>
        ///   Defines the test message used to write to <see cref = "ActivityTrace" /> events.
        /// </summary>
        private const String TestMessage = "Test Message";

        /// <summary>
        ///   Defines the resolver to use for the tests.
        /// </summary>
        private static readonly TestTraceSourceResolver _resolver = new TestTraceSourceResolver();

        /// <summary>
        /// Runs test for run recursive activity.
        /// </summary>
        [TestMethod]
        public void RunRecursiveActivityTest()
        {
            RunRecursiveActivity(20, 1);
        }

        #region Static Helper Methods

        /// <summary>
        /// Runs the recursive activity.
        /// </summary>
        /// <param name="maxDepth">
        /// The max depth.
        /// </param>
        /// <param name="currentDepth">
        /// The current depth.
        /// </param>
        private static void RunRecursiveActivity(Int32 maxDepth, Int32 currentDepth)
        {
            ActivityTrace activity = new ActivityTrace(String.Empty, _resolver);

            activity.StartActivity(TestMessage);
            activity.Write(RecordType.Verbose, TestMessage);
            activity.Write(RecordType.Information, TestMessage);
            activity.Write(RecordType.Warning, TestMessage);

            if (currentDepth < maxDepth)
            {
                RunRecursiveActivity(maxDepth, currentDepth + 1);
            }

            activity.Write(RecordType.Error, TestMessage);
            activity.Write(RecordType.Critical, TestMessage);
            activity.StopActivity(TestMessage);
        }

        #endregion

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///   The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}