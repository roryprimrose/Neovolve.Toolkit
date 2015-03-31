namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="IfThenActivityTests"/>
    ///   class is used to test the <see cref="IfThen"/> class.
    /// </summary>
    [TestClass]
    public class IfThenActivityTests
    {
        /// <summary>
        /// Runs test for if then does not executes body when condition is false.
        /// </summary>
        [TestMethod]
        public void IfThenActivityDoesNotExecutesBodyWhenConditionIsFalseTest()
        {
            IfThenEvaluator target = new IfThenEvaluator
                                     {
                                         Condition = false
                                     };

            IDictionary<String, Object> outputParameters = WorkflowInvoker.Invoke(target);

            Boolean actual = (Boolean)outputParameters["BodyExecuted"];

            Assert.IsFalse(actual, "The body was not executed");
        }

        /// <summary>
        /// Runs test for if then executes body when condition is true.
        /// </summary>
        [TestMethod]
        public void IfThenActivityExecutesBodyWhenConditionIsTrueTest()
        {
            IfThenEvaluator target = new IfThenEvaluator
                                     {
                                         Condition = true
                                     };

            IDictionary<String, Object> outputParameters = WorkflowInvoker.Invoke(target);

            Boolean actual = (Boolean)outputParameters["BodyExecuted"];

            Assert.IsTrue(actual, "The body was not executed");
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