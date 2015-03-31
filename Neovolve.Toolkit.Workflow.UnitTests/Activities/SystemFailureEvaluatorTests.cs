namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="SystemFailureEvaluatorTests"/>
    ///   class is used to test the <see cref="SystemFailureEvaluator"/> class.
    /// </summary>
    [TestClass]
    public class SystemFailureEvaluatorTests
    {
        /// <summary>
        /// Runs test for system failure evaluator completes when provided condition is false.
        /// </summary>
        [TestMethod]
        public void SystemFailureEvaluatorCompletesWhenProvidedConditionIsFalseTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("Condition", false);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when exception type does not have message constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenExceptionTypeDoesNotHaveMessageConstructorTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            String expected = Guid.NewGuid().ToString();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("ExceptionType", typeof(NoMessageException));
            inputParameters.Add("Message", expected);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when exception type does not have param name constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenExceptionTypeDoesNotHaveParamNameConstructorTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            String expected = Guid.NewGuid().ToString();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("ExceptionType", typeof(NoMessageException));
            inputParameters.Add("ParameterName", expected);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when exception type does not have param name message constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenExceptionTypeDoesNotHaveParamNameMessageConstructorTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            String expected = Guid.NewGuid().ToString();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("ExceptionType", typeof(NoMessageException));
            inputParameters.Add("Message", expected);
            inputParameters.Add("ParameterName", expected);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when exception type has unsupported message string constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenExceptionTypeHasUnsupportedMessageStringConstructorTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            String expected = Guid.NewGuid().ToString();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("ExceptionType", typeof(MismatchStringException));
            inputParameters.Add("Message", expected);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when exception type has unsupported param name string constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenExceptionTypeHasUnsupportedParamNameStringConstructorTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            String expected = Guid.NewGuid().ToString();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("ExceptionType", typeof(MismatchStringException));
            inputParameters.Add("ParameterName", expected);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when exception type has unsupported string string constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenExceptionTypeHasUnsupportedStringStringConstructorTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            String expected = Guid.NewGuid().ToString();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("ExceptionType", typeof(MismatchStringException));
            inputParameters.Add("ParameterName", expected);
            inputParameters.Add("Message", expected);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when provided condition is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SystemFailureException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenProvidedConditionIsNullTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();

            WorkflowInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception when provided condition is true.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SystemFailureException))]
        public void SystemFailureEvaluatorThrowsExceptionWhenProvidedConditionIsTrueTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("Condition", true);

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception with specified message and parameter name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SystemFailureEvaluatorThrowsExceptionWithSpecifiedMessageAndParameterNameTest()
        {
            String expectedParamName = "ParamName: " + Guid.NewGuid();
            String expectedMessage = "Message: " + Guid.NewGuid();

            try
            {
                SystemFailureEvaluator target = new SystemFailureEvaluator();
                IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

                inputParameters.Add("ExceptionType", typeof(ArgumentException));
                inputParameters.Add("ParameterName", expectedParamName);
                inputParameters.Add("Message", expectedMessage);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedMessage), "Message returned an incorrect value");
                Assert.AreEqual(expectedParamName, ex.ParamName, "ParamName returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception with specified message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void SystemFailureEvaluatorThrowsExceptionWithSpecifiedMessageTest()
        {
            String expected = Guid.NewGuid().ToString();

            try
            {
                SystemFailureEvaluator target = new SystemFailureEvaluator();
                IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

                inputParameters.Add("ExceptionType", typeof(InvalidCastException));
                inputParameters.Add("Message", expected);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expected, ex.Message, "Message returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception with specified parameter name and message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SystemFailureEvaluatorThrowsExceptionWithSpecifiedParameterNameAndMessageTest()
        {
            String expectedParamName = "ParamName: " + Guid.NewGuid();
            String expectedMessage = "Message: " + Guid.NewGuid();

            try
            {
                SystemFailureEvaluator target = new SystemFailureEvaluator();
                IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

                inputParameters.Add("ExceptionType", typeof(ArgumentOutOfRangeException));
                inputParameters.Add("ParameterName", expectedParamName);
                inputParameters.Add("Message", expectedMessage);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedMessage), "Message returned an incorrect value");
                Assert.AreEqual(expectedParamName, ex.ParamName, "ParamName returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for system failure evaluator throws exception with specified parameter name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SystemFailureEvaluatorThrowsExceptionWithSpecifiedParameterNameTest()
        {
            String expected = Guid.NewGuid().ToString();

            try
            {
                SystemFailureEvaluator target = new SystemFailureEvaluator();
                IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

                inputParameters.Add("ExceptionType", typeof(ArgumentOutOfRangeException));
                inputParameters.Add("ParameterName", expected);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual(expected, ex.ParamName, "ParamName returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for system failure evaluator throws specified exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void SystemFailureEvaluatorThrowsSpecifiedExceptionTest()
        {
            try
            {
                SystemFailureEvaluator target = new SystemFailureEvaluator();
                IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

                inputParameters.Add("ExceptionType", typeof(InvalidCastException));

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidCastException), "SystemFailureEvaluator threw an incorrect exception");

                throw;
            }
        }

        /// <summary>
        /// Runs test for system failure evaluator throws system failure exception by default.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SystemFailureException))]
        public void SystemFailureEvaluatorThrowsSystemFailureExceptionByDefaultTest()
        {
            SystemFailureEvaluator target = new SystemFailureEvaluator();

            try
            {
                WorkflowInvoker.Invoke(target);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SystemFailureException), "SystemFailureEvaluator threw an incorrect exception");

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