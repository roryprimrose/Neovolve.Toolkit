namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;
    using WorkflowTestHelper.Persistence;

    /// <summary>
    /// The <see cref="BusinessFailureScopeTests"/>
    ///   class is used to test the <see cref="BusinessFailureScope{T}"/> class.
    /// </summary>
    [TestClass]
    public class BusinessFailureScopeTests
    {
        /// <summary>
        /// Runs test for business failure scope catches failures from deep nested evaluators.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureScopeCatchesFailuresFromDeepNestedEvaluatorsTest()
        {
            BusinessFailureScopeWithDeepEvaluators target = new BusinessFailureScopeWithDeepEvaluators();

            try
            {
                WorkflowInvoker.Invoke(target);
            }
            catch (BusinessFailureException<Int32> ex)
            {
                Trace.WriteLine(ex);

                Assert.AreEqual(2, ex.Failures.Count(), "Incorrect number of failures found");

                throw;
            }
        }

        /// <summary>
        /// Runs test for business failure scope does not link to child activities with mismatching type.
        /// </summary>
        [TestMethod]
        public void BusinessFailureScopeDoesNotLinkToChildActivitiesWithMismatchingTypeTest()
        {
            BusinessFailureScopeLink target = new BusinessFailureScopeLink();

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(target);
            Boolean activityLinked = (Boolean)outputParameters["MismatchingTypeChildInScope"];

            Assert.IsFalse(activityLinked, "Child activity was linked to the scope when it should not have been");
        }

        /// <summary>
        /// Runs test for business failure scope does not throw exception when no failures added to scope.
        /// </summary>
        [TestMethod]
        public void BusinessFailureScopeDoesNotThrowExceptionWhenNoFailuresAddedToScopeTest()
        {
            BusinessFailureScopeWithSingleEvaluator target = new BusinessFailureScopeWithSingleEvaluator();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("Condition", false);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for business failure scope does not throw exception with no child activities.
        /// </summary>
        [TestMethod]
        public void BusinessFailureScopeDoesNotThrowExceptionWithNoChildActivitiesTest()
        {
            BusinessFailureScope<Int32> target = new BusinessFailureScope<Int32>();

            WorkflowInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure scope links to child activities with matching type.
        /// </summary>
        [TestMethod]
        public void BusinessFailureScopeLinksToChildActivitiesWithMatchingTypeTest()
        {
            BusinessFailureScopeLink target = new BusinessFailureScopeLink();

            IDictionary<String, Object> outputParameters = WorkflowInvoker.Invoke(target);
            Boolean activityLinked = (Boolean)outputParameters["MatchingTypeChildInScope"];

            Assert.IsTrue(activityLinked, "Child activity was not linked to the scope as expected");
        }

        /// <summary>
        /// Runs test for business failure scope throws exception for multiple child evaluator that fails.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureScopeThrowsExceptionForMultipleChildEvaluatorThatFailsTest()
        {
            BusinessFailureScopeWithMultipleFailures target = new BusinessFailureScopeWithMultipleFailures();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            BusinessFailure<Int32> firstFailure = new BusinessFailure<Int32>(Environment.TickCount, Guid.NewGuid().ToString());
            BusinessFailure<Int32> secondFailure = new BusinessFailure<Int32>(Environment.TickCount + 1244, Guid.NewGuid().ToString());

            inputParameters.Add("FirstFailure", firstFailure);
            inputParameters.Add("SecondFailure", secondFailure);

            try
            {
                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (BusinessFailureException<Int32> ex)
            {
                Assert.AreSame(firstFailure, ex.Failures.First(), "Exception did not contain the expected failure");
                Assert.AreSame(secondFailure, ex.Failures.Skip(1).First(), "Exception did not contain the expected failure");

                throw;
            }
        }

        /// <summary>
        /// Runs test for business failure scope throws exception for single child evaluator that fails.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureScopeThrowsExceptionForSingleChildEvaluatorThatFailsTest()
        {
            BusinessFailureScopeWithSingleEvaluator target = new BusinessFailureScopeWithSingleEvaluator();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            BusinessFailure<Int32> failure = new BusinessFailure<Int32>(Environment.TickCount, Guid.NewGuid().ToString());

            inputParameters.Add("Failure", failure);

            try
            {
                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (BusinessFailureException<Int32> ex)
            {
                Assert.AreSame(failure, ex.Failures.First(), "Exception did not contain the expected failure");

                throw;
            }
        }

        /// <summary>
        /// Runs test for business failure scope throws failures stored before persist action.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void BusinessFailureScopeThrowsFailuresStoredBeforePersistActionTest()
        {
            BusinessFailure<Int32> firstFailure = new BusinessFailure<Int32>(Environment.TickCount, Guid.NewGuid().ToString());
            BusinessFailure<Int32> secondFailure = new BusinessFailure<Int32>(Environment.TickCount + 1244, Guid.NewGuid().ToString());
            MemoryStore memoryStore = new MemoryStore();

            Guid workflowId;
            {
                BusinessFailureScopeWithMultipleFailuresAndPersistence target = new BusinessFailureScopeWithMultipleFailuresAndPersistence();
                Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();

                inputParameters.Add("FirstFailure", firstFailure);
                inputParameters.Add("SecondFailure", secondFailure);

                WorkflowApplication firstApplication = new WorkflowApplication(target, inputParameters);

                ActivityInvoker.Invoke(firstApplication, memoryStore);

                workflowId = firstApplication.Id;
            }

            try
            {
                {
                    BusinessFailureScopeWithMultipleFailuresAndPersistence secondTarget = new BusinessFailureScopeWithMultipleFailuresAndPersistence();
                    WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                    ResumeBookmarkContext bookmark = new ResumeBookmarkContext(workflowId, "TestBookmark");

                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);
                }
            }
            catch (ActivityFailureException ex)
            {
                Trace.WriteLine(ex);

                if (ex.InnerException != null)
                {
                    Trace.WriteLine(ex.InnerException);
                }

                BusinessFailureException<Int32> failureException = ex.InnerException as BusinessFailureException<Int32>;
                Assert.IsNotNull(failureException, "Incorrect workflow exception was thrown");
                Assert.AreEqual(2, failureException.Failures.Count(), "Incorrect number of failures thrown");

                BusinessFailure<Int32> firstThrownFailure = failureException.Failures.First();
                BusinessFailure<Int32> secondThrownFailure = failureException.Failures.Skip(1).First();

                Assert.AreEqual(firstFailure.Code, firstThrownFailure.Code, "First failure Code returned an incorrect value");
                Assert.AreEqual(firstFailure.Description, firstThrownFailure.Description, "First failure Description returned an incorrect value");
                Assert.AreEqual(secondFailure.Code, secondThrownFailure.Code, "Second failure Code returned an incorrect value");
                Assert.AreEqual(secondFailure.Description, secondThrownFailure.Description, "Second failure Description returned an incorrect value");

                throw;
            }
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