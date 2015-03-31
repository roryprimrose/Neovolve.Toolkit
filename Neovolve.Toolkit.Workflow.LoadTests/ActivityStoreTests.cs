namespace Neovolve.Toolkit.Workflow.LoadTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="ActivityStoreTests"/>
    ///   class is used to test the <see cref="ActivityStore"/> class.
    /// </summary>
    [TestClass]
    public class ActivityStoreTests
    {
        /// <summary>
        /// Runs test for activity store resolve.
        /// </summary>
        [TestMethod]
        public void ActivityStoreResolveTest()
        {
            ActivityStore.Resolve<SystemFailureEvaluator>();
        }

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