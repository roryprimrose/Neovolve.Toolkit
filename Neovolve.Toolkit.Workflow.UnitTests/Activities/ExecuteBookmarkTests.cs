namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;
    using WorkflowTestHelper.Persistence;

    /// <summary>
    /// The <see cref="ExecuteBookmarkTests"/>
    ///   class is used to test the <see cref="ExecuteBookmark"/> class.
    /// </summary>
    [TestClass]
    public class ExecuteBookmarkTests
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
                PersistAndResumeNoValue firstTarget = new PersistAndResumeNoValue();
                WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                ActivityInvoker.Invoke(firstApplication, memoryStore);

                workflowId = firstApplication.Id;
            }
            {
                // Run resume workflow
                PersistAndResumeNoValue secondTarget = new PersistAndResumeNoValue();
                WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);
                ResumeBookmarkContext bookmark = new ResumeBookmarkContext(workflowId, BookmarkName);

                ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);
            }
        }

        /// <summary>
        /// Runs test for execute bookmark throws exception with empty bookmark name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void ExecuteBookmarkThrowsExceptionWithEmptyBookmarkNameTest()
        {
            ExecuteBookmark target = new ExecuteBookmark();
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
            ExecuteBookmark target = new ExecuteBookmark();
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
            ExecuteBookmark target = new ExecuteBookmark();
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