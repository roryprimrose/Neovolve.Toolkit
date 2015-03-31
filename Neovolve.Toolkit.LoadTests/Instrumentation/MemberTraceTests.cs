namespace Neovolve.Toolkit.LoadTests.Instrumentation
{
    using System;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="MemberTraceTests"/> 
    ///   class is used to run tests against the <see cref="MemberTrace"/> class.
    /// </summary>
    [TestClass]
    public class MemberTraceTests
    {
        /// <summary>
        ///   Defines the test message used to write to <see cref = "MemberTrace" /> events.
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

        /// <summary>
        /// Runs test for run recursive activity with defined method.
        /// </summary>
        [TestMethod]
        public void RunRecursiveActivityWithDefinedMethodTest()
        {
            RunRecursiveActivityWithDefinedMethod(20, 1);
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
            using (MemberTrace activity = new MemberTrace(String.Empty, null, _resolver))
            {
                activity.Write(RecordType.Verbose, TestMessage);
                activity.Write(RecordType.Information, TestMessage);
                activity.Write(RecordType.Warning, TestMessage);

                if (currentDepth < maxDepth)
                {
                    RunRecursiveActivity(maxDepth, currentDepth + 1);
                }

                activity.Write(RecordType.Error, TestMessage);
                activity.Write(RecordType.Critical, TestMessage);
            }
        }

        /// <summary>
        /// Runs the recursive activity.
        /// </summary>
        /// <param name="maxDepth">
        /// The max depth.
        /// </param>
        /// <param name="currentDepth">
        /// The current depth.
        /// </param>
        private static void RunRecursiveActivityWithDefinedMethod(Int32 maxDepth, Int32 currentDepth)
        {
            using (MemberTrace activity = new MemberTrace(String.Empty, MethodBase.GetCurrentMethod(), _resolver))
            {
                activity.Write(RecordType.Verbose, TestMessage);
                activity.Write(RecordType.Information, TestMessage);
                activity.Write(RecordType.Warning, TestMessage);

                if (currentDepth < maxDepth)
                {
                    RunRecursiveActivityWithDefinedMethod(maxDepth, currentDepth + 1);
                }

                activity.Write(RecordType.Error, TestMessage);
                activity.Write(RecordType.Critical, TestMessage);
            }
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