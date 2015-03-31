namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.UnitTests.Activities;
    using WorkflowTestHelper.Persistence;

    /// <summary>
    /// The <see cref="ActivityInvokerTests"/>
    ///   class is used to test the <see cref="ActivityInvoker"/> class.
    /// </summary>
    [TestClass]
    public class ActivityInvokerTests
    {
        /// <summary>
        /// Runs test for activity invoker can invoke activity asynchronously with input parameters.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeActivityAsynchronouslyWithInputParametersTest()
        {
            SimpleActivity target = new SimpleActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            String expected = Guid.NewGuid().ToString();

            inputParameters.Add("InputValue", expected);

            ActivityInvoker.InvokeAsync(target, inputParameters);
        }

        /// <summary>
        /// Runs test for activity invoker can invoke activity asynchronously with no input data.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeActivityAsynchronouslyWithNoInputDataTest()
        {
            GetWorkflowInstanceId target = new GetWorkflowInstanceId();

            ActivityInvoker.InvokeAsync(target);
        }

        /// <summary>
        /// Runs test for activity invoker can invoke activity asynchronously with input data.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeActivityAsynchronouslyWithNullInputDataTest()
        {
            GetWorkflowInstanceId target = new GetWorkflowInstanceId();

            ActivityInvoker.InvokeAsync(target, null);
        }

        /// <summary>
        /// Runs test for activity invoker can invoke activity with input parameters.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeActivityWithInputParametersTest()
        {
            SimpleActivity target = new SimpleActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            String expected = Guid.NewGuid().ToString();

            inputParameters.Add("InputValue", expected);

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(target, inputParameters);

            Assert.AreEqual(expected, outputParameters["OutputValue"], "Invoke returned an incorrect value");
        }

        /// <summary>
        /// Runs test for activity invoker can invoke activity with no parameters.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeActivityWithNoParametersTest()
        {
            GetWorkflowInstanceId target = new GetWorkflowInstanceId();

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(target);

            Assert.AreNotEqual(Guid.Empty, outputParameters["Result"], "Invoke failed to return expected value");
        }

        /// <summary>
        /// Runs test for activity invoker can invoke activity with null input parameters.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeActivityWithNullInputParametersTest()
        {
            GetWorkflowInstanceId target = new GetWorkflowInstanceId();

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(target, null);

            Assert.AreNotEqual(Guid.Empty, outputParameters["Result"], "Invoke failed to return expected value");
        }

        /// <summary>
        /// Runs test for activity invoker can invoke application with activity initialize.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeApplicationWithActivityInitializeTest()
        {
            SimpleActivity activity = new SimpleActivity();
            Boolean invoked = false;
            WorkflowApplication target = new WorkflowApplication(activity);

            ActivityInvoker.Invoke(target, x => invoked = true);

            Assert.IsTrue(invoked, "Invoke did not invoke action");
        }

        /// <summary>
        /// Runs test for activity invoker can invoke application with instance store and resume context.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeApplicationWithInstanceStoreAndResumeContextWithDataTest()
        {
            MemoryStore store = new MemoryStore();
            PersistAndResume activity = new PersistAndResume();
            WorkflowApplication firstApplication = new WorkflowApplication(activity);

            ActivityInvoker.Invoke(firstApplication, store);

            ResumeBookmarkContext<String> resume = new ResumeBookmarkContext<String>(firstApplication.Id, "BookmarkToResume", "Some data");
            WorkflowApplication secondApplication = new WorkflowApplication(activity);

            ActivityInvoker.Invoke(secondApplication, store, resume);
        }

        /// <summary>
        /// Runs test for activity invoker can invoke application with instance store and resume context with no data.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeApplicationWithInstanceStoreAndResumeContextWithNoDataTest()
        {
            MemoryStore store = new MemoryStore();
            PersistAndResumeNoValue activity = new PersistAndResumeNoValue();
            WorkflowApplication firstApplication = new WorkflowApplication(activity);

            ActivityInvoker.Invoke(firstApplication, store);

            ResumeBookmarkContext resume = new ResumeBookmarkContext(firstApplication.Id, "BookmarkToResume");
            WorkflowApplication secondApplication = new WorkflowApplication(activity);

            ActivityInvoker.Invoke(secondApplication, store, resume);
        }

        /// <summary>
        /// Runs test for activity invoker can invoke application with instance store.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeApplicationWithInstanceStoreTest()
        {
            MemoryStore store = new MemoryStore();
            PersistAndResume activity = new PersistAndResume();
            WorkflowApplication target = new WorkflowApplication(activity);

            ActivityInvoker.Invoke(target, store);
        }

        /// <summary>
        /// Runs test for activity invoker can invoke application with no parameters.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerCanInvokeApplicationWithNoParametersTest()
        {
            GetWorkflowInstanceId target = new GetWorkflowInstanceId();
            WorkflowApplication application = new WorkflowApplication(target);

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(application);

            Assert.AreNotEqual(Guid.Empty, outputParameters["Result"], "Invoke failed to return expected value");
        }

        /// <summary>
        /// Runs test for activity invoker returns null output parameters when workflow is aborted.
        /// </summary>
        [TestMethod]
        public void ActivityInvokerReturnsNullOutputParametersWhenWorkflowIsAbortedTest()
        {
            MemoryStore store = new MemoryStore();
            AbortActivity activity = new AbortActivity();
            WorkflowApplication target = new WorkflowApplication(activity);

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(target, store);

            Assert.IsNull(outputParameters, "Invoke returned an incorrect value");
        }

        /// <summary>
        /// Runs test for activity invoker returns throws exception when workflow is aborted with reason.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ActivityInvokerReturnsThrowsExceptionWhenWorkflowIsAbortedWithReasonTest()
        {
            AbortActivity target = new AbortActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            TimeoutException expected = new TimeoutException();

            inputParameters.Add("Reason", expected);

            try
            {
                ActivityInvoker.Invoke(target, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Assert.AreSame(expected, ex.InnerException, "InnerException returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for activity invoker throws exception when workflow throws unhandled exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ActivityInvokerThrowsExceptionWhenWorkflowThrowsUnhandledExceptionTest()
        {
            ExecuteBookmark activity = new ExecuteBookmark
                                       {
                                           BookmarkName = String.Empty
                                       };

            try
            {
                ActivityInvoker.Invoke(activity);
            }
            catch (ActivityFailureException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "Incorrect exception type thrown");

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