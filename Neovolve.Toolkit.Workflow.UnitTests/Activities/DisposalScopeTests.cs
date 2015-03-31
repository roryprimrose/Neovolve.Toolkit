namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="DisposalScopeTests"/>
    ///   class is used to test the <see cref="DisposalScope{T}"/> class.
    /// </summary>
    [TestClass]
    public class DisposalScopeTests
    {
        /// <summary>
        /// Runs test for disposable scope disallows persistence.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DisposableScopeDisallowsPersistenceTest()
        {
            DisposalScopeActivity target = new DisposalScopeActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            inputParameters.Add("DisposableInstance", disposable);
            inputParameters.Add("DestroyInScope", false);
            inputParameters.Add("PersistInScope", true);
            inputParameters.Add("ThrowException", false);

            try
            {
                WorkflowInvoker.Invoke(target, inputParameters);
            }
            finally
            {
                disposable.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for disposable scope disposes instance on exception thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void DisposableScopeDisposesInstanceOnExceptionThrownTest()
        {
            DisposalScopeActivity target = new DisposalScopeActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            inputParameters.Add("DisposableInstance", disposable);
            inputParameters.Add("DestroyInScope", false);
            inputParameters.Add("PersistInScope", false);
            inputParameters.Add("ThrowException", true);

            try
            {
                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (TimeoutException ex)
            {
                Trace.WriteLine(ex);

                throw;
            }
            finally
            {
                disposable.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for disposable scope disposes instance on successful execution.
        /// </summary>
        [TestMethod]
        public void DisposableScopeDisposesInstanceOnSuccessfulExecutionTest()
        {
            DisposalScopeActivity target = new DisposalScopeActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            inputParameters.Add("DisposableInstance", disposable);
            inputParameters.Add("DestroyInScope", false);
            inputParameters.Add("PersistInScope", false);
            inputParameters.Add("ThrowException", false);

            IDictionary<String, Object> outputParameters = WorkflowInvoker.Invoke(target, inputParameters);

            Boolean actual = (Boolean)outputParameters["BodyExecuted"];

            Assert.IsTrue(actual, "The body activity was not executed");

            disposable.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for disposable scope disposes instance released from variable.
        /// </summary>
        [TestMethod]
        public void DisposableScopeDisposesInstanceReleasedFromVariableTest()
        {
            DisposalScopeActivity target = new DisposalScopeActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            inputParameters.Add("DisposableInstance", disposable);
            inputParameters.Add("DestroyInScope", true);
            inputParameters.Add("PersistInScope", false);
            inputParameters.Add("ThrowException", false);

            WorkflowInvoker.Invoke(target, inputParameters);

            disposable.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for disposable scope executes body with null instance.
        /// </summary>
        [TestMethod]
        public void DisposableScopeExecutesBodyWithNullInstanceTest()
        {
            DisposalScopeActivity target = new DisposalScopeActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("DisposableInstance", null);
            inputParameters.Add("DestroyInScope", false);
            inputParameters.Add("PersistInScope", false);
            inputParameters.Add("ThrowException", false);

            IDictionary<String, Object> outputParameters = WorkflowInvoker.Invoke(target, inputParameters);

            Boolean actual = (Boolean)outputParameters["BodyExecuted"];

            Assert.IsTrue(actual, "The body activity was not executed");
        }

        /// <summary>
        /// Runs test for disposable scope ignores object disposed exception.
        /// </summary>
        [TestMethod]
        public void DisposableScopeIgnoresObjectDisposedExceptionTest()
        {
            DisposalScopeActivity target = new DisposalScopeActivity();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            disposable.Stub(x => x.Dispose()).Throw(new ObjectDisposedException("Object has already been disposed."));

            inputParameters.Add("DisposableInstance", disposable);
            inputParameters.Add("DestroyInScope", false);
            inputParameters.Add("PersistInScope", false);
            inputParameters.Add("ThrowException", false);

            WorkflowInvoker.Invoke(target, inputParameters);

            disposable.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for disposal scope can use specific type in child activities.
        /// </summary>
        [TestMethod]
        public void DisposalScopeCanUseSpecificTypeInChildActivitiesTest()
        {
            DisposalScopeSpecificType target = new DisposalScopeSpecificType();
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            ITestInstance disposable = MockRepository.GenerateStub<ITestInstance>();

            inputParameters.Add("DisposableInstance", disposable);

            WorkflowInvoker.Invoke(target, inputParameters);

            disposable.AssertWasCalled(x => x.DoSomething());
            disposable.AssertWasCalled(x => x.DoSomethingElse());
            disposable.AssertWasCalled(x => x.Dispose());
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