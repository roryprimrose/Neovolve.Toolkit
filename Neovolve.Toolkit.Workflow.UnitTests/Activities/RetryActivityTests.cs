namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="RetryActivityTests"/>
    ///   class is used to test the <see cref="Retry"/> class.
    /// </summary>
    [TestClass]
    public class RetryActivityTests
    {
        /// <summary>
        /// Runs test for retry activity execute completes successfully with available retries and no time delay.
        /// </summary>
        [TestMethod]
        public void RetryActivityExecuteCompletesSuccessfullyWithAvailableRetriesAndNoTimeDelayTest()
        {
            RetryWithFailure activity = ActivityStore.Resolve<RetryWithFailure>();

            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("AttemptsToFail", 4);
            inputParameters.Add("MaxAttemptsAllowed", 5);
            inputParameters.Add("RetryTimeout", TimeSpan.Zero);
            inputParameters.Add("FailureException", new TimeoutException());

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(activity, inputParameters);

            Int32 attempts = (Int32)outputParameters["AttemptsMade"];

            Assert.AreEqual(5, attempts, "Retry did not execute child activity");
        }

        /// <summary>
        /// Runs test for retry activity execute completes successfully with available retries and time delay.
        /// </summary>
        [TestMethod]
        public void RetryActivityExecuteCompletesSuccessfullyWithAvailableRetriesAndTimeDelayTest()
        {
            RetryWithFailure activity = ActivityStore.Resolve<RetryWithFailure>();

            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            TimeSpan retryTimeout = new TimeSpan(0, 0, 0, 2);
            const Int32 AttemptsToFail = 3;

            inputParameters.Add("AttemptsToFail", AttemptsToFail);
            inputParameters.Add("MaxAttemptsAllowed", 5);
            inputParameters.Add("RetryTimeout", retryTimeout);
            inputParameters.Add("FailureException", new TimeoutException());

            Stopwatch timer = Stopwatch.StartNew();

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(activity, inputParameters);

            timer.Stop();

            Int32 attempts = (Int32)outputParameters["AttemptsMade"];

            Assert.AreEqual(AttemptsToFail + 1, attempts, "Retry did not execute child activity");

            Double expectedDelay = retryTimeout.TotalMilliseconds * AttemptsToFail;

            Trace.WriteLine("Expected delay: " + expectedDelay + " milliseconds");
            Trace.WriteLine("Actual delay: " + timer.ElapsedMilliseconds + " milliseconds");

            Assert.IsTrue(timer.ElapsedMilliseconds > expectedDelay, "Retry activity did not delay as expected");
        }

        /// <summary>
        /// Runs test for retry activity execute completes successfully with no child activities.
        /// </summary>
        [TestMethod]
        public void RetryActivityExecuteCompletesSuccessfullyWithNoChildActivitiesTest()
        {
            Retry activity = ActivityStore.Resolve<Retry>();

            ActivityInvoker.Invoke(activity);
        }

        /// <summary>
        /// Runs test for retry activity execute completes successfully with no failures.
        /// </summary>
        [TestMethod]
        public void RetryActivityExecuteCompletesSuccessfullyWithNoFailuresTest()
        {
            RetryWithFailure activity = ActivityStore.Resolve<RetryWithFailure>();

            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("AttemptsToFail", 0);
            inputParameters.Add("MaxAttemptsAllowed", 5);
            inputParameters.Add("RetryTimeout", new TimeSpan(0, 0, 0, 1));
            inputParameters.Add("FailureException", new TimeoutException());

            IDictionary<String, Object> outputParameters = ActivityInvoker.Invoke(activity, inputParameters);

            Int32 attempts = (Int32)outputParameters["AttemptsMade"];

            Assert.AreEqual(1, attempts, "Retry did not execute child activity");
        }

        /// <summary>
        /// Runs test for retry activity execute completes successfully with null child activities.
        /// </summary>
        [TestMethod]
        public void RetryActivityExecuteCompletesSuccessfullyWithNullChildActivitiesTest()
        {
            Retry activity = new Retry
                             {
                                 Body = null
                             };

            ActivityInvoker.Invoke(activity);
        }

        /// <summary>
        /// Runs test for retry activity execute fails with retries available but different exceptions thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void RetryActivityExecuteFailsWithRetriesAvailableButDifferentExceptionsThrownTest()
        {
            RetryWithFailure activity = ActivityStore.Resolve<RetryWithFailure>();

            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("AttemptsToFail", 4);
            inputParameters.Add("MaxAttemptsAllowed", 5);
            inputParameters.Add("RetryTimeout", TimeSpan.Zero);
            inputParameters.Add("FailureException", new SecurityException());

            try
            {
                ActivityInvoker.Invoke(activity, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Trace.WriteLine(ex);

                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Runs test for retry activity throws exception when all attempts fail.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void RetryActivityThrowsExceptionWhenAllAttemptsFailTest()
        {
            RetryWithFailure activity = ActivityStore.Resolve<RetryWithFailure>();

            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("AttemptsToFail", 5);
            inputParameters.Add("MaxAttemptsAllowed", 5);
            inputParameters.Add("RetryTimeout", TimeSpan.Zero);
            inputParameters.Add("FailureException", new TimeoutException());

            try
            {
                ActivityInvoker.Invoke(activity, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Trace.WriteLine(ex);

                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Runs test for retry activity throws exception when no retry attempts allowed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void RetryActivityThrowsExceptionWhenNoRetryAttemptsAllowedTest()
        {
            RetryWithFailure activity = ActivityStore.Resolve<RetryWithFailure>();

            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("AttemptsToFail", 1);
            inputParameters.Add("MaxAttemptsAllowed", 0);
            inputParameters.Add("RetryTimeout", TimeSpan.Zero);
            inputParameters.Add("FailureException", new TimeoutException());

            try
            {
                ActivityInvoker.Invoke(activity, inputParameters);
            }
            catch (ActivityFailureException ex)
            {
                Trace.WriteLine(ex);

                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Runs test for retry activity throws exception with invalid exception type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWorkflowException))]
        public void RetryActivityThrowsExceptionWithInvalidExceptionTypeTest()
        {
            Retry target = new Retry
                           {
                               ExceptionType = typeof(String)
                           };

            try
            {
                ActivityInvoker.Invoke(target);
            }
            catch (ActivityFailureException ex)
            {
                Trace.WriteLine(ex);

                throw ex.InnerException;
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