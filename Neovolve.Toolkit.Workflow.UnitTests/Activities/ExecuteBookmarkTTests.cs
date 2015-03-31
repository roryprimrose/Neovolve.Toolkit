namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;
    using WorkflowTestHelper.Persistence;

    /// <summary>
    /// The <see cref="ExecuteBookmarkTTests"/>
    ///   class is used to test the <see cref="ExecuteBookmark{T}"/> class.
    /// </summary>
    [TestClass]
    public class ExecuteBookmarkTTests
    {
        /// <summary>
        /// Runs test for execute bookmark resumes bookmark with no provided value.
        /// </summary>
        [TestMethod]
        public void ExecuteBookmarkResumesBookmarkWithNoProvidedValueTest()
        {
            const String BookmarkName = "BookmarkToResume";
            MemoryStore memoryStore = new MemoryStore();
            Guid workflowId;
            {
                PersistAndResume firstTarget = new PersistAndResume();
                WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                ActivityInvoker.Invoke(firstApplication, memoryStore);

                workflowId = firstApplication.Id;
            }
            {
                // Run resume workflow
                PersistAndResume secondTarget = new PersistAndResume();
                WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, BookmarkName);
                IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                Assert.IsNull(outputParameters["BookmarkOutput"], "Workflow returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for execute bookmark resumes bookmark with provided value.
        /// </summary>
        [TestMethod]
        public void ExecuteBookmarkResumesBookmarkWithProvidedValueTest()
        {
            const String BookmarkName = "BookmarkToResume";
            MemoryStore memoryStore = new MemoryStore();
            Guid workflowId;
            {
                PersistAndResume firstTarget = new PersistAndResume();
                WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                ActivityInvoker.Invoke(firstApplication, memoryStore);

                workflowId = firstApplication.Id;
            }
            {
                // Run resume workflow
                String expected = Guid.NewGuid().ToString();
                PersistAndResume secondTarget = new PersistAndResume();
                WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, BookmarkName, expected);
                IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                Assert.AreEqual(expected, outputParameters["BookmarkOutput"], "Workflow returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for execute bookmark throws exception when resumed with an invalid type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ExecuteBookmarkThrowsExceptionWhenResumedWithAnInvalidTypeTest()
        {
            const String BookmarkName = "BookmarkToResume";
            MemoryStore memoryStore = new MemoryStore();
            Guid workflowId;
            {
                PersistAndResume firstTarget = new PersistAndResume();
                WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                ActivityInvoker.Invoke(firstApplication, memoryStore);

                workflowId = firstApplication.Id;
            }
            {
                // Run resume workflow
                PersistAndResume secondTarget = new PersistAndResume();
                WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                ResumeBookmarkContext<Guid> bookmark = new ResumeBookmarkContext<Guid>(workflowId, BookmarkName, Guid.NewGuid());

                try
                {
                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);
                }
                catch (WorkflowApplicationTerminatedException ex)
                {
                    Assert.IsInstanceOfType(ex.InnerException, typeof(InvalidCastException), "InnerException returned an incorrect value");

                    throw;
                }
            }
        }

        /// <summary>
        /// Runs test for execute bookmark throws exception with empty bookmark name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ExecuteBookmarkThrowsExceptionWithEmptyBookmarkNameTest()
        {
            ExecuteBookmark<String> target = new ExecuteBookmark<String>();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("BookmarkName", String.Empty);

            try
            {
                ActivityInvoker.Invoke(target, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "InnerException returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for execute bookmark throws exception with null bookmark name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ExecuteBookmarkThrowsExceptionWithNullBookmarkNameTest()
        {
            ExecuteBookmark<String> target = new ExecuteBookmark<String>();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("BookmarkName", null);

            try
            {
                ActivityInvoker.Invoke(target, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "InnerException returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for execute bookmark throws exception with white space bookmark name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ExecuteBookmarkThrowsExceptionWithWhiteSpaceBookmarkNameTest()
        {
            ExecuteBookmark<String> target = new ExecuteBookmark<String>();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("BookmarkName", " ");

            try
            {
                ActivityInvoker.Invoke(target, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "InnerException returned an incorrect value");

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