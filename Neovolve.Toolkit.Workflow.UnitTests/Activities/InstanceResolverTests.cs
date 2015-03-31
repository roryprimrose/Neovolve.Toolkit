namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Configuration;
    using System.Diagnostics;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.Extensions;
    using Rhino.Mocks;
    using WorkflowTestHelper.Persistence;

    /// <summary>
    /// The <see cref="InstanceResolverTests"/>
    ///   class is used to test the <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> class.
    /// </summary>
    [TestClass]
    public class InstanceResolverTests
    {
        /// <summary>
        /// Runs test for instance resolver can execute with child without instance resolution.
        /// </summary>
        [TestMethod]
        public void InstanceResolverCanExecuteWithChildWithoutInstanceResolutionTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ITestInstance testInstance = MockRepository.GenerateStub<ITestInstance>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(testInstance);

            InstanceManagerExtension.Container = container;

            try
            {
                Activity target = new ResolverWithoutResolution();

                WorkflowInvoker.Invoke(target);

                container.AssertWasNotCalled(x => x.Resolve<ITestInstance>((String)null));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver T16 can execute with no children.
        /// </summary>
        [TestMethod]
        public void InstanceResolverCanExecuteWithNoChildrenTest()
        {
            InstanceResolver
                <Int32, String, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> target
                    =
                    new InstanceResolver
                        <Int32, String, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object
                            >();

            WorkflowInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for instance resolver t16 does not resolve instance with empty body.
        /// </summary>
        [TestMethod]
        public void InstanceResolverDoesNotResolveInstanceWithEmptyBodyTest()
        {
            DelegateInArgument<InstanceHandler<ITestInstance>> firstArgument = new DelegateInArgument<InstanceHandler<ITestInstance>>
                                                                               {
                                                                                   Name = "handler1"
                                                                               };
            DelegateInArgument<InstanceHandler<String>> secondArgument = new DelegateInArgument<InstanceHandler<String>>
                                                                         {
                                                                             Name = "handler2"
                                                                         };
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            InstanceManagerExtension.Container = container;

            try
            {
                InstanceResolver
                    <ITestInstance, String, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, 
                        Object> target = new InstanceResolver
                            <ITestInstance, String, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, 
                                Object, Object>
                                         {
                                             Body =
                                                 new ActivityAction
                                                 <InstanceHandler<ITestInstance>, InstanceHandler<String>, InstanceHandler<Object>, 
                                                 InstanceHandler<Object>, InstanceHandler<Object>, InstanceHandler<Object>, InstanceHandler<Object>, 
                                                 InstanceHandler<Object>, InstanceHandler<Object>, InstanceHandler<Object>, InstanceHandler<Object>, 
                                                 InstanceHandler<Object>, InstanceHandler<Object>, InstanceHandler<Object>, InstanceHandler<Object>, 
                                                 InstanceHandler<Object>>
                                                 {
                                                     Argument1 = firstArgument, 
                                                     Argument2 = secondArgument
                                                 }
                                         };

                WorkflowInvoker.Invoke(target);

                container.AssertWasNotCalled(x => x.Resolve<ITestInstance>((String)null));
                container.AssertWasNotCalled(x => x.Resolve<String>((String)null));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver does not resolve instance with no children.
        /// </summary>
        [TestMethod]
        public void InstanceResolverDoesNotResolveInstanceWithNoChildrenTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ITestInstance testInstance = MockRepository.GenerateStub<ITestInstance>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(testInstance);

            InstanceManagerExtension.Container = container;

            try
            {
                Activity target = new ResolverWithoutChildren();

                WorkflowInvoker.Invoke(target);

                container.AssertWasNotCalled(x => x.Resolve<ITestInstance>((String)null));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver does not resolve instance with null body.
        /// </summary>
        [TestMethod]
        public void InstanceResolverDoesNotResolveInstanceWithNullBodyTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            InstanceManagerExtension.Container = container;

            try
            {
                InstanceResolver
                    <ITestInstance, String, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, 
                        Object> target = new InstanceResolver
                            <ITestInstance, String, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, 
                                Object, Object>
                                         {
                                             Body = null
                                         };

                WorkflowInvoker.Invoke(target);

                container.AssertWasNotCalled(x => x.Resolve<ITestInstance>((String)null));
                container.AssertWasNotCalled(x => x.Resolve<String>((String)null));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver does not teardown null resolved values.
        /// </summary>
        [TestMethod]
        public void InstanceResolverDoesNotTeardownNullResolvedValuesTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(null).Repeat.Once();

            InstanceManagerExtension.Container = container;

            try
            {
                Activity target = new ResolverWithMultipleHandlerReferences();

                WorkflowInvoker.Invoke(target);
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }

            container.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
            container.AssertWasNotCalled(x => x.Teardown(null), opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// Runs test for instance resolver only resolves instance handles null as the resolution value.
        /// </summary>
        [TestMethod]
        public void InstanceResolverHandlesNullAsTheResolutionValueTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(null).Repeat.Once();

            InstanceManagerExtension.Container = container;

            try
            {
                ResolverWithMultipleHandlerReferences target = new ResolverWithMultipleHandlerReferences();

                WorkflowInvoker.Invoke(target);
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }

            container.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
        }

        /// <summary>
        /// Runs test for instance resolver only resolves instance once when handler is referenced multiple times.
        /// </summary>
        [TestMethod]
        public void InstanceResolverOnlyResolvesInstanceOnceWhenHandlerIsReferencedMultipleTimesTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ITestInstance testInstance = MockRepository.GenerateStub<ITestInstance>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(testInstance).Repeat.Once();

            InstanceManagerExtension.Container = container;

            try
            {
                ResolverWithMultipleHandlerReferences target = new ResolverWithMultipleHandlerReferences();

                WorkflowInvoker.Invoke(target);
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }

            container.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
            container.AssertWasCalled(x => x.Teardown(testInstance));
            testInstance.AssertWasCalled(x => x.DoSomething());
            testInstance.AssertWasCalled(x => x.DoSomethingElse());
        }

        /// <summary>
        /// Runs test for instance resolver resolves instance with resolving children after unhandled exception is thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void InstanceResolverResolvesInstanceWithResolvingChildrenAfterUnhandledExceptionIsThrownTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ITestInstance testInstance = MockRepository.GenerateStub<ITestInstance>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(testInstance);

            InstanceManagerExtension.Container = container;

            try
            {
                Activity target = new ResolverWithThrow();

                try
                {
                    WorkflowInvoker.Invoke(target);
                }
                finally
                {
                    container.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
                }
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down multiple resolved instance when persisted then creates and tears down again after bookmark resumes.
        /// </summary>
        [TestMethod]
        public void InstanceResolverTearsDownMultipleResolvedInstanceWhenPersistedThenCreatesAndTearsDownAgainAfterBookmarkResumesTest()
        {
            try
            {
                MemoryStore memoryStore = new MemoryStore();
                Guid workflowId;
                {
                    // Run initial workflow execution
                    // Use code blocks to ensure there is no pollution between first and second workflow invocations
                    IUnityContainer firstContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance firstInstance = MockRepository.GenerateStub<ITestInstance>();
                    String firstValue = Guid.NewGuid().ToString();

                    firstContainer.Stub(x => x.Resolve<String>((String)null)).Return(firstValue);
                    firstContainer.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(firstInstance);

                    InstanceManagerExtension.Container = firstContainer;

                    ResolverMultipleBeforeAndAfterPersist firstTarget = new ResolverMultipleBeforeAndAfterPersist();
                    WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                    ActivityInvoker.Invoke(firstApplication, memoryStore);

                    workflowId = firstApplication.Id;

                    firstContainer.AssertWasCalled(x => x.Resolve<String>((String)null));
                    firstContainer.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
                    firstInstance.AssertWasCalled(x => x.TestInput(firstValue));
                    firstContainer.AssertWasCalled(x => x.Teardown(firstInstance));
                    firstContainer.AssertWasCalled(x => x.Teardown(firstValue));
                }
                {
                    // Run resume workflow
                    IUnityContainer secondContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance secondInstance = MockRepository.GenerateStub<ITestInstance>();
                    String secondValue = Guid.NewGuid().ToString();

                    secondContainer.Stub(x => x.Resolve<String>("NamedString")).Return(secondValue);
                    secondContainer.Stub(x => x.Resolve<ITestInstance>("NamedInstance")).Return(secondInstance);

                    InstanceManagerExtension.Container = secondContainer;

                    ResolverMultipleBeforeAndAfterPersist secondTarget = new ResolverMultipleBeforeAndAfterPersist();
                    WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                    ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, "TestBookmark", "ResumeData");
                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                    secondContainer.AssertWasCalled(x => x.Resolve<String>("NamedString"));
                    secondContainer.AssertWasCalled(x => x.Resolve<ITestInstance>("NamedInstance"));
                    secondInstance.AssertWasCalled(x => x.TestInput(secondValue));
                    secondContainer.AssertWasCalled(x => x.Teardown(secondInstance));
                    secondContainer.AssertWasCalled(x => x.Teardown(secondValue));
                }
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down resolved instance on abort.
        /// </summary>
        [TestMethod]
        public void InstanceResolverTearsDownResolvedInstanceOnAbortTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ITestInstance testInstance = MockRepository.GenerateStub<ITestInstance>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(testInstance);

            InstanceManagerExtension.Container = container;

            try
            {
                Activity target = new ResolverWithAbort();

                try
                {
                    WorkflowInvoker.Invoke(target);

                    Assert.Fail("WorkflowApplicationAbortedException was expected");
                }
                catch (WorkflowApplicationAbortedException)
                {
                    // Ignored, this exception is expected
                }

                container.AssertWasCalled(x => x.Teardown(testInstance));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down resolved instance.
        /// </summary>
        [TestMethod]
        public void InstanceResolverTearsDownResolvedInstanceTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            ITestInstance testInstance = MockRepository.GenerateStub<ITestInstance>();

            container.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(testInstance);

            InstanceManagerExtension.Container = container;

            try
            {
                ResolverWithoutPersist target = new ResolverWithoutPersist();

                WorkflowInvoker.Invoke(target);

                container.AssertWasCalled(x => x.Teardown(testInstance));
                testInstance.AssertWasCalled(x => x.DoSomething());
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down resolved instance when persisted then creates and tears down again after bookmark resumes.
        /// </summary>
        [TestMethod]
        public void InstanceResolverTearsDownResolvedInstanceWhenPersistedThenCreatesAndTearsDownAgainAfterBookmarkResumesTest()
        {
            try
            {
                MemoryStore memoryStore = new MemoryStore();
                Guid workflowId;
                {
                    // Run initial workflow execution
                    // Use code blocks to ensure there is no pollution between first and second workflow invocations
                    IUnityContainer firstContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance firstInstance = MockRepository.GenerateStub<ITestInstance>();

                    firstContainer.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(firstInstance);
                    firstInstance.Stub(x => x.DoSomething()).WhenCalled(a => Trace.WriteLine("FirstInstance invoked"));

                    InstanceManagerExtension.Container = firstContainer;

                    ResolverBeforeAndAfterPersist firstTarget = new ResolverBeforeAndAfterPersist();
                    WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                    ActivityInvoker.Invoke(firstApplication, memoryStore);

                    workflowId = firstApplication.Id;

                    firstContainer.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
                    firstInstance.AssertWasCalled(x => x.DoSomething());
                    firstInstance.AssertWasNotCalled(x => x.DoSomethingElse());
                    firstContainer.AssertWasCalled(x => x.Teardown(firstInstance));
                }
                {
                    // Run resume workflow
                    IUnityContainer secondContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance secondInstance = MockRepository.GenerateStub<ITestInstance>();

                    secondContainer.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(secondInstance);
                    secondInstance.Stub(x => x.DoSomethingElse()).WhenCalled(a => Trace.WriteLine("SecondInstance invoked"));

                    InstanceManagerExtension.Container = secondContainer;

                    ResolverBeforeAndAfterPersist secondTarget = new ResolverBeforeAndAfterPersist();
                    WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                    ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, "TestBookmark", "ResumeData");
                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                    secondContainer.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
                    secondInstance.AssertWasNotCalled(x => x.DoSomething());
                    secondInstance.AssertWasCalled(x => x.DoSomethingElse());
                    secondContainer.AssertWasCalled(x => x.Teardown(secondInstance));
                }
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down resolved instance when persisted then creates and tears down again after bookmark resumes with same activity instance.
        /// </summary>
        [TestMethod]
        public void InstanceResolverTearsDownResolvedInstanceWhenPersistedThenCreatesAndTearsDownAgainAfterBookmarkResumesWithSameActivityInstanceTest
            ()
        {
            try
            {
                MemoryStore memoryStore = new MemoryStore();
                Guid workflowId;
                {
                    // Run initial workflow execution
                    // Use code blocks to ensure there is no pollution between first and second workflow invocations
                    IUnityContainer firstContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance firstInstance = MockRepository.GenerateStub<ITestInstance>();

                    firstContainer.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(firstInstance);
                    firstInstance.Stub(x => x.DoSomething()).WhenCalled(a => Trace.WriteLine("FirstInstance invoked"));

                    InstanceManagerExtension.Container = firstContainer;

                    ResolverSurroundingPersist firstTarget = new ResolverSurroundingPersist();
                    WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                    ActivityInvoker.Invoke(firstApplication, memoryStore);

                    workflowId = firstApplication.Id;

                    firstContainer.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
                    firstInstance.AssertWasCalled(x => x.DoSomething());
                    firstInstance.AssertWasNotCalled(x => x.DoSomethingElse());
                    firstContainer.AssertWasCalled(x => x.Teardown(firstInstance));
                }
                {
                    // Run resume workflow
                    IUnityContainer secondContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance secondInstance = MockRepository.GenerateStub<ITestInstance>();

                    secondContainer.Stub(x => x.Resolve<ITestInstance>((String)null)).Return(secondInstance);
                    secondInstance.Stub(x => x.DoSomethingElse()).WhenCalled(a => Trace.WriteLine("SecondInstance invoked"));

                    InstanceManagerExtension.Container = secondContainer;

                    ResolverSurroundingPersist secondTarget = new ResolverSurroundingPersist();
                    WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                    ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, "TestBookmark", "ResumeData");
                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                    secondContainer.AssertWasCalled(x => x.Resolve<ITestInstance>((String)null));
                    secondInstance.AssertWasNotCalled(x => x.DoSomething());
                    secondInstance.AssertWasCalled(x => x.DoSomethingElse());
                    secondContainer.AssertWasCalled(x => x.Teardown(secondInstance));
                }
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down resolved named instance when persisted then creates and tears down again after bookmark resumes.
        /// </summary>
        [TestMethod]
        public void InstanceResolverTearsDownResolvedNamedInstanceWhenPersistedThenCreatesAndTearsDownAgainAfterBookmarkResumesTest()
        {
            try
            {
                const String NamedResolve = "NamedResolve";
                MemoryStore memoryStore = new MemoryStore();
                Guid workflowId;
                {
                    // Run initial workflow execution
                    // Use code blocks to ensure there is no pollution between first and second workflow invocations
                    IUnityContainer firstContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance firstInstance = MockRepository.GenerateStub<ITestInstance>();

                    firstContainer.Stub(x => x.Resolve<ITestInstance>(NamedResolve)).Return(firstInstance);
                    firstInstance.Stub(x => x.DoSomething()).WhenCalled(a => Trace.WriteLine("FirstInstance invoked"));

                    InstanceManagerExtension.Container = firstContainer;

                    ResolverWithNameBeforeAndAfterPersist firstTarget = new ResolverWithNameBeforeAndAfterPersist();
                    WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                    ActivityInvoker.Invoke(firstApplication, memoryStore);

                    workflowId = firstApplication.Id;

                    firstContainer.AssertWasCalled(x => x.Resolve<ITestInstance>(NamedResolve));
                    firstInstance.AssertWasCalled(x => x.DoSomething());
                    firstInstance.AssertWasNotCalled(x => x.DoSomethingElse());
                    firstContainer.AssertWasCalled(x => x.Teardown(firstInstance));
                }
                {
                    // Run resume workflow
                    IUnityContainer secondContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance secondInstance = MockRepository.GenerateStub<ITestInstance>();

                    secondContainer.Stub(x => x.Resolve<ITestInstance>(NamedResolve)).Return(secondInstance);
                    secondInstance.Stub(x => x.DoSomethingElse()).WhenCalled(a => Trace.WriteLine("SecondInstance invoked"));

                    InstanceManagerExtension.Container = secondContainer;

                    ResolverWithNameBeforeAndAfterPersist secondTarget = new ResolverWithNameBeforeAndAfterPersist();
                    WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                    ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, "TestBookmark", "ResumeData");
                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                    secondContainer.AssertWasCalled(x => x.Resolve<ITestInstance>(NamedResolve));
                    secondInstance.AssertWasNotCalled(x => x.DoSomething());
                    secondInstance.AssertWasCalled(x => x.DoSomethingElse());
                    secondContainer.AssertWasCalled(x => x.Teardown(secondInstance));
                }
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver tears down resolved named instance when persisted then creates and tears down again after bookmark resumes with same activity instance.
        /// </summary>
        [TestMethod]
        public void
            InstanceResolverTearsDownResolvedNamedInstanceWhenPersistedThenCreatesAndTearsDownAgainAfterBookmarkResumesWithSameActivityInstanceTest()
        {
            try
            {
                const String ResolveName = "ResolveName";
                MemoryStore memoryStore = new MemoryStore();
                Guid workflowId;
                {
                    // Run initial workflow execution
                    // Use code blocks to ensure there is no pollution between first and second workflow invocations
                    IUnityContainer firstContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance firstInstance = MockRepository.GenerateStub<ITestInstance>();

                    firstContainer.Stub(x => x.Resolve<ITestInstance>(ResolveName)).Return(firstInstance);
                    firstInstance.Stub(x => x.DoSomething()).WhenCalled(a => Trace.WriteLine("FirstInstance invoked"));

                    InstanceManagerExtension.Container = firstContainer;

                    ResolverWithNameSurroundingPersist firstTarget = new ResolverWithNameSurroundingPersist();
                    WorkflowApplication firstApplication = new WorkflowApplication(firstTarget);

                    ActivityInvoker.Invoke(firstApplication, memoryStore);

                    workflowId = firstApplication.Id;

                    firstContainer.AssertWasCalled(x => x.Resolve<ITestInstance>(ResolveName));
                    firstInstance.AssertWasCalled(x => x.DoSomething());
                    firstInstance.AssertWasNotCalled(x => x.DoSomethingElse());
                    firstContainer.AssertWasCalled(x => x.Teardown(firstInstance));
                }
                {
                    // Run resume workflow
                    IUnityContainer secondContainer = MockRepository.GenerateStub<IUnityContainer>();
                    ITestInstance secondInstance = MockRepository.GenerateStub<ITestInstance>();

                    secondContainer.Stub(x => x.Resolve<ITestInstance>(ResolveName)).Return(secondInstance);
                    secondInstance.Stub(x => x.DoSomethingElse()).WhenCalled(a => Trace.WriteLine("SecondInstance invoked"));

                    InstanceManagerExtension.Container = secondContainer;

                    ResolverWithNameSurroundingPersist secondTarget = new ResolverWithNameSurroundingPersist();
                    WorkflowApplication secondApplication = new WorkflowApplication(secondTarget);

                    ResumeBookmarkContext<String> bookmark = new ResumeBookmarkContext<String>(workflowId, "TestBookmark", "ResumeData");
                    ActivityInvoker.Invoke(secondApplication, memoryStore, bookmark);

                    secondContainer.AssertWasCalled(x => x.Resolve<ITestInstance>(ResolveName));
                    secondInstance.AssertWasNotCalled(x => x.DoSomething());
                    secondInstance.AssertWasCalled(x => x.DoSomethingElse());
                    secondContainer.AssertWasCalled(x => x.Teardown(secondInstance));
                }
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance resolver throws exception when resolving default container with no configuration.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void InstanceResolverThrowsExceptionWhenResolvingDefaultContainerWithNoConfigurationTest()
        {
            Activity target = new ResolverWithMultipleHandlerReferences();

            WorkflowInvoker.Invoke(target);
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