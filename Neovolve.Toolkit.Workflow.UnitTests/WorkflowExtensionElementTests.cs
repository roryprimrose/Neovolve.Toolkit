namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="WorkflowExtensionElementTests"/>
    ///   class is used to test the <see cref="WorkflowExtensionElement"/> class.
    /// </summary>
    [TestClass]
    public class WorkflowExtensionElementTests
    {
        /// <summary>
        /// Runs test for workflow extension element returns correct behavior type.
        /// </summary>
        [TestMethod]
        public void WorkflowExtensionElementReturnsCorrectBehaviorTypeTest()
        {
            WorkflowExtensionElement target = new WorkflowExtensionElement();

            Type actual = target.BehaviorType;

            Assert.AreEqual(typeof(WorkflowExtensionBehavior), actual, "BehaviorType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for workflow extension element stores extension type.
        /// </summary>
        [TestMethod]
        public void WorkflowExtensionElementStoresExtensionTypeTest()
        {
            String expected = typeof(String).AssemblyQualifiedName;
            WorkflowExtensionElement target = new WorkflowExtensionElement
                                              {
                                                  ExtensionType = expected
                                              };

            String actual = target.ExtensionType;

            Assert.AreEqual(expected, actual, "ExtensionType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for workflow extension element stores is singleton.
        /// </summary>
        [TestMethod]
        public void WorkflowExtensionElementStoresIsSingletonTest()
        {
            WorkflowExtensionElement target = new WorkflowExtensionElement
                                              {
                                                  IsSingleton = true
                                              };

            Boolean actual = target.IsSingleton;

            Assert.IsTrue(actual, "IsSingleton returned an incorrect value");
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