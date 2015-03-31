namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="GetWorkflowInstanceIdTests"/>
    ///   class is used to test the <see cref="GetWorkflowInstanceId"/> class.
    /// </summary>
    [TestClass]
    public class GetWorkflowInstanceIdTests
    {
        /// <summary>
        /// Runs test for get workflow instance id returns workflow instance id.
        /// </summary>
        [TestMethod]
        public void GetWorkflowInstanceIdReturnsWorkflowInstanceIdTest()
        {
            GetWorkflowInstanceId target = new GetWorkflowInstanceId();
            WorkflowApplication application = new WorkflowApplication(target);

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(application, null, null, null);

            Assert.AreEqual(application.Id, outputParameters["Result"], "GetWorkflowInstanceId returned an incorrect value");
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