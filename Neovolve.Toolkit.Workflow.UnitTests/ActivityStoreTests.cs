namespace Neovolve.Toolkit.Workflow.UnitTests
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
        /// Runs test for activity store resolve returns instance.
        /// </summary>
        [TestMethod]
        public void ActivityStoreResolveReturnsInstanceTest()
        {
            SystemFailureEvaluator activity = ActivityStore.Resolve<SystemFailureEvaluator>();

            Assert.IsNotNull(activity, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for activity store resolve returns the same instance.
        /// </summary>
        [TestMethod]
        public void ActivityStoreResolveReturnsTheSameInstanceTest()
        {
            SystemFailureEvaluator firstActivity = ActivityStore.Resolve<SystemFailureEvaluator>();
            SystemFailureEvaluator secondActivity = ActivityStore.Resolve<SystemFailureEvaluator>();

            Assert.AreSame(firstActivity, secondActivity, "Resolve returned an incorrect value");
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