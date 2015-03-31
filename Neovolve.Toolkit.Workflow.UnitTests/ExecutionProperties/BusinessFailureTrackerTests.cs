namespace Neovolve.Toolkit.Workflow.UnitTests.ExecutionProperties
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.ExecutionProperties;

    /// <summary>
    /// The <see cref="BusinessFailureTrackerTests"/>
    ///   class is used to test the <see cref="BusinessFailureTracker{T}"/> class.
    /// </summary>
    [TestClass]
    public class BusinessFailureTrackerTests
    {
        /// <summary>
        /// Runs test for name returns unique name based on generic type.
        /// </summary>
        [TestMethod]
        public void NameReturnsUniqueNameBasedOnGenericTypeTest()
        {
            String firstName = BusinessFailureTracker<Int32>.Name;
            String secondName = BusinessFailureTracker<Boolean>.Name;

            Assert.AreNotEqual(firstName, secondName, "Name returned the same values");
        }

        /// <summary>
        /// Runs test for returns failure collection instance.
        /// </summary>
        [TestMethod]
        public void ReturnsFailureCollectionInstanceTest()
        {
            BusinessFailureTracker<Int32> target = new BusinessFailureTracker<Int32>();

            Assert.IsNotNull(target.Failures, "Failures failed to return an instance");
        }

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}