namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;

    /// <summary>
    /// The <see cref="ActivityTraceTests"/>
    ///   class is used to test the <see cref="ActivityTrace"/> class.
    /// </summary>
    [TestClass]
    public class ActivityTraceTests
    {
        /// <summary>
        ///   Stores the test trace source.
        /// </summary>
        private static readonly TraceSource TestSource = new TraceSource("TestSource");

        #region Setup/Teardown

        /// <summary>
        /// Initializes the class for running unit tests.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestSource.Listeners.Add(new TestTraceListener());
        }

        /// <summary>
        /// InitializeContexts the test.
        /// </summary>
        [TestInitialize]
        public void TestInitializeContext()
        {
            TestSource.Switch.Level = SourceLevels.All;

            TestTraceListener.Clear();
            RecordTrace.ThrowOnSourceFailure = true;

            // Clear the cache resolver for each test
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.Stub<ITraceSourceResolver>();
            CacheResolver cacheResolver = new CacheResolver(resolver);
            cacheResolver.Reload();
        }

        #endregion

        /// <summary>
        /// Runs test for can create.
        /// </summary>
        [TestMethod]
        public void CanCreateTest()
        {
            new ActivityTrace();
        }

        /// <summary>
        /// Runs test for can create with source.
        /// </summary>
        [TestMethod]
        public void CanCreateWithSourceTest()
        {
            MockRepository mock = new MockRepository();
            TraceListener listener = mock.PartialMock<TraceListener>();
            TraceSource source = new TraceSource("test");

            source.Switch.Level = SourceLevels.All;
            source.Listeners.Add(listener);

            ActivityTrace target = new ActivityTrace(source);
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                listener.TraceEvent(null, source.Name, TraceEventType.Start, 0, message);
                LastCall.Constraints(Is.Anything(), Is.Equal(source.Name), Is.Equal(TraceEventType.Start), Is.Equal(0), Is.Equal(message));

                listener.TraceEvent(null, source.Name, TraceEventType.Stop, 0, message);
                LastCall.Constraints(Is.Anything(), Is.Equal(source.Name), Is.Equal(TraceEventType.Stop), Is.Equal(0), Is.Equal(message));
            }

            using (mock.Playback())
            {
                target.StartActivity(message);
                target.StopActivity(message);
            }
        }

        /// <summary>
        /// Runs test for current nested activities.
        /// </summary>
        [TestMethod]
        public void CurrentNestedActivitiesTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace parent = new ActivityTrace(String.Empty, resolver);
            ActivityTrace child = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.IsNotNull(ActivityTrace.Current, "Current returned an incorrect value");

            parent.StartActivity(message);

            Assert.AreEqual(parent.ActivityId, ActivityTrace.Current.ActivityId, "ActivityId returned an incorrect value");

            child.StartActivity(message);

            Assert.AreEqual(child.ActivityId, ActivityTrace.Current.ActivityId, "ActivityId returned an incorrect value");

            child.StopActivity(message);

            Assert.AreEqual(parent.ActivityId, ActivityTrace.Current.ActivityId, "ActivityId returned an incorrect value");

            parent.StopActivity(message);

            Assert.IsNotNull(ActivityTrace.Current, "Current returned an incorrect value");
        }

        /// <summary>
        /// Runs test for current no activity returns A disabled activity.
        /// </summary>
        [TestMethod]
        public void CurrentNoActivityReturnsADisabledActivityTest()
        {
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "The logical operation stack is not empty");
            Assert.AreEqual(ActivityTraceState.Disabled, ActivityTrace.Current.State, "State returned an incorrect value");
        }

        /// <summary>
        /// Runs test for current no parent activity.
        /// </summary>
        [TestMethod]
        public void CurrentNoParentActivityTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            ActivityTrace actual = null;

            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "The logical operation stack is empty");

                actual = ActivityTrace.Current;

                Assert.IsNotNull(actual, "Current returned an incorrect value");

                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
                Assert.AreEqual(target.State, actual.State, "State values do not match");

                // Test that we can suspend one and resume the other
                actual.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");
                Assert.AreEqual(target.State, actual.State, "State values do not match");

                target.ResumeActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
                Assert.AreEqual(target.State, actual.State, "State values do not match");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

                if (actual != null)
                {
                    Assert.AreEqual(target.State, actual.State, "State values do not match");
                }
            }
        }

        /// <summary>
        /// Runs test for current returns equivalent instance with existing activity on stack.
        /// </summary>
        [TestMethod]
        public void CurrentReturnsEquivalentInstanceWithExistingActivityOnStackTest()
        {
            ActivityTrace parent = new ActivityTrace(TestSource);

            try
            {
                parent.StartActivity("parent started");

                ActivityTrace target = new ActivityTrace(TestSource);

                target.StartActivity("child started");

                try
                {
                    ActivityTrace current = ActivityTrace.Current;

                    Assert.AreEqual(
                        target.GetHashCode(), current.GetHashCode(), "Current returned an instance that wasn't equal to the last ActivityTrace");
                    Assert.AreNotEqual(
                        parent.GetHashCode(), current.GetHashCode(), "Current returned an instance that wasn't equal to the last ActivityTrace");
                }
                finally
                {
                    target.StopActivity("child stopped");
                }
            }
            finally
            {
                parent.StopActivity("stopping");
            }
        }

        /// <summary>
        /// Runs test for current returns existing activity.
        /// </summary>
        [TestMethod]
        public void CurrentReturnsExistingActivityTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                ActivityTrace actual = ActivityTrace.Current;

                Assert.AreEqual(target.GetHashCode(), actual.GetHashCode(), "Current failed to return the correct item");
                Assert.AreEqual(target, actual, "Current failed to return the correct item");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for current returns instance with empty activity stack.
        /// </summary>
        [TestMethod]
        public void CurrentReturnsInstanceWithEmptyActivityStackTest()
        {
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "Correlation stack is not empty");

            ActivityTrace target = ActivityTrace.Current;

            Assert.IsNotNull(target, "Current failed to return an instance");
            Assert.AreEqual(
                ActivityTraceContext.Empty.GetHashCode(), target.GetHashCode(), "Current returned an instance that didn't have an empty context");
        }

        /// <summary>
        /// Runs test for current returns instance with no available activity.
        /// </summary>
        [TestMethod]
        public void CurrentReturnsInstanceWithNoAvailableActivityTest()
        {
            // Precondition for the test
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "The logical operation stack is not empty");

            Assert.IsNotNull(ActivityTrace.Current, "Current returned an incorrect value");
        }

        /// <summary>
        /// Runs test for current returns new activity when no activity is available in correlation manager.
        /// </summary>
        [TestMethod]
        public void CurrentReturnsNewActivityWhenNoActivityIsAvailableInCorrelationManagerTest()
        {
            Guid operationId = Guid.NewGuid();
            Trace.CorrelationManager.StartLogicalOperation(operationId);

            try
            {
                ActivityTrace activity = ActivityTrace.Current;

                Assert.AreEqual(ActivityTraceState.Disabled, activity.State, "State returned an incorrect value");
            }
            finally
            {
                Trace.CorrelationManager.StopLogicalOperation();
            }
        }

        /// <summary>
        /// Runs test for disabled trace activity is created with disabled state.
        /// </summary>
        [TestMethod]
        public void DisabledTraceActivityIsCreatedWithDisabledStateTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.Stub<ITraceSourceResolver>();

            ActivityTrace target = new ActivityTrace(Guid.NewGuid().ToString(), resolver, false);

            Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
        }

        /// <summary>
        /// Runs test for enabled trace activity is created with stopped state.
        /// </summary>
        [TestMethod]
        public void EnabledTraceActivityIsCreatedWithStoppedStateTest()
        {
            ActivityTrace target = new ActivityTrace(TestTraceSourceResolver.SourceName, new TestTraceSourceResolver(), true);

            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals correctly compares different instances with different internal context.
        /// </summary>
        [TestMethod]
        public void EqualsCorrectlyComparesDifferentInstancesWithDifferentInternalContextTest()
        {
            ActivityTrace first = new ActivityTrace(TestSource);
            ActivityTrace second = new ActivityTrace("second", null, false);

            Assert.IsFalse(first.Equals(second), "Equals failed to correctly compare different instance with different context values");
        }

        /// <summary>
        /// Runs test for equals correctly compares different instances with the same internal context.
        /// </summary>
        [TestMethod]
        public void EqualsCorrectlyComparesDifferentInstancesWithTheSameInternalContextTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            // We need to start the activity to ensure that Current returns the same logic value
            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                Assert.IsTrue(target.Equals(ActivityTrace.Current), "Equals failed to correctly compare different instance with the same context");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for equals returns false for comparing A different type.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsFalseForComparingADifferentTypeTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            Assert.IsFalse(target.Equals(new TraceSourceLoadException()), "Equals failed to correctly compare with an incorrect type");
        }

        /// <summary>
        /// Runs test for equals returns false for comparing A null instance.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsFalseForComparingANullInstanceTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            Assert.IsFalse(target.Equals(null), "Equals failed to correctly compare with a null instance");
        }

        /// <summary>
        /// Runs test for equals returns true for comparing the same instance.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsTrueForComparingTheSameInstanceTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            Assert.IsTrue(target.Equals(target), "Equals failed to correctly compare the same instance");
        }

        /// <summary>
        /// Runs test for failing to resolve trace source throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TraceSourceLoadException))]
        public void FailingToResolveTraceSourceThrowsExceptionTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.Stub<ITraceSourceResolver>();

            ActivityTrace target = new ActivityTrace(Guid.NewGuid().ToString(), resolver);

            // Invoke a method to ensure that the instance is initialized
            target.Flush();
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different instances with different internal context.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentInstancesWithDifferentInternalContextTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                TraceSource nextSource = new TraceSource(Guid.NewGuid().ToString());
                ActivityTrace compare = new ActivityTrace(nextSource);

                Assert.AreNotEqual(compare.GetHashCode(), target.GetHashCode(), "GetHashCode returned an incorrect value");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instances with same internal context.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstancesWithSameInternalContextTest()
        {
            ActivityTrace target = new ActivityTrace(TestSource);

            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                Assert.AreEqual(ActivityTrace.Current.GetHashCode(), target.GetHashCode(), "GetHashCode returned an incorrect value");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for nested trace activity.
        /// </summary>
        [TestMethod]
        public void NestedTraceActivityTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace parent = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();
            try
            {
                Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "An activity is already active");
                Assert.AreEqual(ActivityTraceState.Stopped, parent.State, "State returned an incorrect value");
                Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the stack");

                parent.StartActivity(message);

                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the stack");

                parent.Write(RecordType.Information, message);

                ActivityTrace child = new ActivityTrace(String.Empty, resolver);

                Assert.AreEqual(ActivityTraceState.Stopped, child.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                child.StartActivity(message);

                Assert.AreEqual(ActivityTraceState.Running, child.State, "State returned an incorrect value");
                Assert.AreEqual(ActivityTraceState.Running, parent.State, "State returned an incorrect value");
                Assert.AreEqual(child.ActivityId, Trace.CorrelationManager.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(2, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the stack");

                Assert.AreEqual(2, TestTraceListener.Events.Count, "Invalid number of events have been written");
                Assert.AreEqual(TraceEventType.Transfer, TestTraceListener.Events[0].EventType, "Invalid event type written");
                Assert.AreEqual(TraceEventType.Start, TestTraceListener.Events[1].EventType, "Invalid event type written");

                child.Write(RecordType.Verbose, message);

                TestTraceListener.Clear();

                child.StopActivity(message);

                Assert.AreEqual(2, TestTraceListener.Events.Count, "Invalid number of events have been written");
                Assert.AreEqual(TraceEventType.Stop, TestTraceListener.Events[0].EventType, "Invalid event type written");
                Assert.AreEqual(TraceEventType.Transfer, TestTraceListener.Events[1].EventType, "Invalid event type written");
                Assert.AreEqual(ActivityTraceState.Stopped, child.State, "State returned an incorrect value");
                Assert.AreEqual(ActivityTraceState.Running, parent.State, "State returned an incorrect value");
                Assert.AreEqual(parent.ActivityId, Trace.CorrelationManager.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the stack");

                parent.Write(RecordType.Information, message);

                parent.StopActivity(message);

                Assert.AreEqual(ActivityTraceState.Stopped, parent.State, "State returned an incorrect value");
                Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the stack");
            }
            finally
            {
                // Clear out any of the operations
                while (Trace.CorrelationManager.LogicalOperationStack.Count > 0)
                {
                    Trace.CorrelationManager.StopLogicalOperation();
                }
            }
        }

        /// <summary>
        /// Runs test for started disabled trace activity does not change correlation stack.
        /// </summary>
        [TestMethod]
        public void StartedDisabledTraceActivityDoesNotChangeCorrelationStackTest()
        {
            RecordTrace.ThrowOnSourceFailure = false;

            ActivityTrace activity = new ActivityTrace("ATraceSourceThatDoesn'tExist");

            Assert.AreEqual(ActivityTraceState.Disabled, activity.State, "State returned an incorrect value");

            activity.StartActivity(Guid.NewGuid().ToString());

            activity.StopActivity(Guid.NewGuid().ToString());

            Assert.AreEqual(
                0, Trace.CorrelationManager.LogicalOperationStack.Count, "ActivityTrace was placed onto the stack when it should have been");
        }

        /// <summary>
        /// Runs test for state can progress to running state when trace level excludes activity messages.
        /// </summary>
        [TestMethod]
        public void StateCanProgressToRunningStateWhenTraceLevelExcludesActivityMessagesTest()
        {
            String message = Guid.NewGuid().ToString();

            TestSource.Switch.Level = SourceLevels.Information;

            ActivityTrace target = new ActivityTrace(TestSource);

            target.StartActivity(Guid.NewGuid().ToString());
            target.Write(RecordType.Information, message);
            target.StopActivity(Guid.NewGuid().ToString());

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Information, TestTraceListener.Events[0].EventType, "EventType returned an incorrect value");
            Assert.AreEqual(message, TestTraceListener.Events[0].Message, "Message returned an incorrect value");
        }

        /// <summary>
        /// Runs test for state returns disabled when tracing is not enabled.
        /// </summary>
        [TestMethod]
        public void StateReturnsDisabledWhenTracingIsNotEnabledTest()
        {
            Boolean originalEnabled = RecordTrace.Enabled;

            try
            {
                RecordTrace.Enabled = false;

                ActivityTrace target = new ActivityTrace(new TraceSource("Test"));

                Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
            }
            finally
            {
                RecordTrace.Enabled = originalEnabled;
            }
        }

        /// <summary>
        /// Runs test for state returns stored state when tracing is enabled.
        /// </summary>
        [TestMethod]
        public void StateReturnsStoredStateWhenTracingIsEnabledTest()
        {
            Boolean originalEnabled = RecordTrace.Enabled;

            try
            {
                RecordTrace.Enabled = true;

                ActivityTrace target = new ActivityTrace(new TraceSource("Test"));

                Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");
            }
            finally
            {
                RecordTrace.Enabled = originalEnabled;
            }
        }

        /// <summary>
        /// Runs test for stopping an activity that has A running child activity throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StoppingAnActivityThatHasARunningChildActivityThrowsExceptionTest()
        {
            TraceSource source = new TraceSource("test");
            ActivityTrace parent = new ActivityTrace(source);

            parent.StartActivity(Guid.NewGuid().ToString());

            ActivityTrace child = new ActivityTrace(source);

            child.StartActivity(Guid.NewGuid().ToString());

            try
            {
                parent.StopActivity(Guid.NewGuid().ToString());
            }
            finally
            {
                child.StopActivity(Guid.NewGuid().ToString());
                parent.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write resume object array disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteResumeObjectArrayDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                target.ResumeActivity(Format, args);

                Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                Assert.AreEqual(0, TestTraceListener.Events.Count, "Events were recored when none were expected");
            }
        }

        /// <summary>
        /// Runs test for write resume object array running.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteResumeObjectArrayRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                target.ResumeActivity(Format, args);
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write resume object array stopped.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteResumeObjectArrayStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.ResumeActivity(Format, args);
        }

        /// <summary>
        /// Runs test for write resume string disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteResumeStringDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                target.ResumeActivity(message);

                Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for write resume string object array suspended.
        /// </summary>
        [TestMethod]
        public void WriteResumeStringObjectArraySuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            target.StartActivity(Guid.NewGuid().ToString());

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                target.ResumeActivity(Format, args);

                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Resume, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(
                    String.Format(CultureInfo.InvariantCulture, Format, args), 
                    TestTraceListener.Events[0].Message, 
                    "An incorrect message was recorded");
            }
            finally
            {
                // Close off the activity
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write resume string running.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteResumeStringRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            target.StartActivity(message);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                target.ResumeActivity(message);
            }
            finally
            {
                // Close off the activity
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write resume string stopped.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteResumeStringStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.ResumeActivity(message);
        }

        /// <summary>
        /// Runs test for write resume string suspended.
        /// </summary>
        [TestMethod]
        public void WriteResumeStringSuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            target.StartActivity(message);

            try
            {
                target.SuspendActivity(message);

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                target.ResumeActivity(message);

                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Resume, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(message, TestTraceListener.Events[0].Message, "An incorrect message was recorded");
            }
            finally
            {
                target.StopActivity(message);
            }
        }

        /// <summary>
        /// Runs test for write start object array disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteStartObjectArrayDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                try
                {
                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                    target.StartActivity(Format, args);

                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                    Assert.AreEqual(0, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                }
                finally
                {
                    target.StopActivity(Guid.NewGuid().ToString());
                }
            }
        }

        /// <summary>
        /// Runs test for write start object array running.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStartObjectArrayRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(Format, args);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                target.StartActivity(Format, args);
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write start object array stopped.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteStartObjectArrayStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");
            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.StartActivity(Format, args);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
                Assert.AreNotEqual(Guid.Empty, target.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(target.ActivityId, Trace.CorrelationManager.ActivityId, "CorrelationResolver does not have the current ActivityId");
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the activity stack");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Start, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(
                    String.Format(CultureInfo.InvariantCulture, Format, args), 
                    TestTraceListener.Events[0].Message, 
                    "An incorrect message was recorded");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write start object array suspended.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStartObjectArraySuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(Format, args);

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                target.StartActivity(Format, args);
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write start string disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteStartStringDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                try
                {
                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                    target.StartActivity(message);

                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                    Assert.AreEqual(0, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                }
                finally
                {
                    target.StopActivity(Guid.NewGuid().ToString());
                }
            }
        }

        /// <summary>
        /// Runs test for write start string running.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStartStringRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(message);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                target.StartActivity(message);
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write start string stopped.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteStartStringStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");
            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.StartActivity(message);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
                Assert.AreNotEqual(Guid.Empty, target.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(target.ActivityId, Trace.CorrelationManager.ActivityId, "CorrelationResolver does not have the current ActivityId");
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the activity stack");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Start, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(message, TestTraceListener.Events[0].Message, "An incorrect message was recorded");
            }
            finally
            {
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write start string suspended.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStartStringSuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(message);

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                target.StartActivity(message);
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write stop object array disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteStopObjectArrayDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                target.StartActivity(Format, args);

                try
                {
                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                    target.StopActivity(Guid.NewGuid().ToString());

                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                    Assert.AreEqual(0, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                }
                finally
                {
                    if (target.State == ActivityTraceState.Running)
                    {
                        target.StopActivity(Guid.NewGuid().ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for write stop object array running.
        /// </summary>
        [TestMethod]
        public void WriteStopObjectArrayRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(Format, args);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                target.StopActivity(Format, args);

                Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");
                Assert.AreEqual(Guid.Empty, target.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(target.ActivityId, Trace.CorrelationManager.ActivityId, "CorrelationResolver does not have the current ActivityId");
                Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the activity stack");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Stop, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(
                    String.Format(CultureInfo.InvariantCulture, Format, args), 
                    TestTraceListener.Events[0].Message, 
                    "An incorrect message was recorded");
            }
            finally
            {
                if (target.State == ActivityTraceState.Running)
                {
                    target.StopActivity(Guid.NewGuid().ToString());
                }
            }
        }

        /// <summary>
        /// Runs test for write stop object array stopped.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStopObjectArrayStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");
            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.StopActivity(Format, args);
        }

        /// <summary>
        /// Runs test for write stop object array suspended.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStopObjectArraySuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(Format, args);

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                target.StopActivity(Format, args);
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write stop string disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteStopStringDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                target.StartActivity(message);

                try
                {
                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                    target.StopActivity(message);

                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                    Assert.AreEqual(0, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                }
                finally
                {
                    if (target.State == ActivityTraceState.Running)
                    {
                        target.StopActivity(Guid.NewGuid().ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Runs test for write stop string running.
        /// </summary>
        [TestMethod]
        public void WriteStopStringRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(message);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                target.StopActivity(message);

                Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");
                Assert.AreEqual(Guid.Empty, target.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(target.ActivityId, Trace.CorrelationManager.ActivityId, "CorrelationResolver does not have the current ActivityId");
                Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the activity stack");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Stop, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(message, TestTraceListener.Events[0].Message, "An incorrect message was recorded");
            }
            finally
            {
                if (target.State == ActivityTraceState.Running)
                {
                    target.StopActivity(Guid.NewGuid().ToString());
                }
            }
        }

        /// <summary>
        /// Runs test for write stop string stopped.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStopStringStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");
            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.StopActivity(message);
        }

        /// <summary>
        /// Runs test for write stop string suspended.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteStopStringSuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(message);

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                target.StopActivity(message);
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write suspend object array disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteSuspendObjectArrayDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                target.StartActivity(Guid.NewGuid().ToString());

                try
                {
                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                    target.SuspendActivity(Format, args);

                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                    Assert.AreEqual(0, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                }
                finally
                {
                    target.StopActivity(Guid.NewGuid().ToString());
                }
            }
        }

        /// <summary>
        /// Runs test for write suspend object array running.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteSuspendObjectArrayRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(Format, args);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                target.SuspendActivity(Format, args);

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");
                Assert.AreNotEqual(Guid.Empty, target.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(target.ActivityId, Trace.CorrelationManager.ActivityId, "CorrelationResolver does not have the current ActivityId");
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the activity stack");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Suspend, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(
                    String.Format(CultureInfo.InvariantCulture, Format, args), 
                    TestTraceListener.Events[0].Message, 
                    "An incorrect message was recorded");
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write suspend object array stopped.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteSuspendObjectArrayStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");
            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.SuspendActivity(Format, args);
        }

        /// <summary>
        /// Runs test for write suspend object array suspended.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteSuspendObjectArraySuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            const String Format = "Test {0}";
            Object[] args = new Object[]
                            {
                                123
                            };

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(Format, args);

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                target.SuspendActivity(Format, args);
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write suspend string disabled.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteSuspendStringDisabledTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String SourceName = "Neovolve";
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                ActivityTrace target = new ActivityTrace(String.Empty, resolver, false);

                target.StartActivity(Guid.NewGuid().ToString());

                try
                {
                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");

                    target.SuspendActivity(message);

                    Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
                    Assert.AreEqual(0, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                }
                finally
                {
                    target.StopActivity(Guid.NewGuid().ToString());
                }
            }
        }

        /// <summary>
        /// Runs test for write suspend string running.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void WriteSuspendStringRunningTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(message);

            try
            {
                Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");

                TestTraceListener.Clear();

                target.SuspendActivity(message);

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");
                Assert.AreNotEqual(Guid.Empty, target.ActivityId, "ActivityId returned an incorrect value");
                Assert.AreEqual(target.ActivityId, Trace.CorrelationManager.ActivityId, "CorrelationResolver does not have the current ActivityId");
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count, "Invalid number of operations on the activity stack");
                Assert.AreEqual(1, TestTraceListener.Events.Count, "An incorrect number of trace events have been recorded");
                Assert.AreEqual(TraceEventType.Suspend, TestTraceListener.Events[0].EventType, "An incorrect event type was recorded");
                Assert.AreEqual(message, TestTraceListener.Events[0].Message, "An incorrect message was recorded");
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for write suspend string stopped.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteSuspendStringStoppedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");
            Assert.AreEqual(ActivityTraceState.Stopped, target.State, "State returned an incorrect value");

            target.SuspendActivity(message);
        }

        /// <summary>
        /// Runs test for write suspend string suspended.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WriteSuspendStringSuspendedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);
            String message = Guid.NewGuid().ToString();

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId, "There is already an activity running");

            target.StartActivity(message);

            try
            {
                target.SuspendActivity(Guid.NewGuid().ToString());

                Assert.AreEqual(ActivityTraceState.Suspended, target.State, "State returned an incorrect value");

                target.SuspendActivity(message);
            }
            finally
            {
                target.ResumeActivity(Guid.NewGuid().ToString());
                target.StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for writing trace message with incorrect correlation manager activity id throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WritingTraceMessageWithIncorrectCorrelationManagerActivityIdThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();
            ActivityTrace target = new ActivityTrace(String.Empty, resolver);

            target.StartActivity(Guid.NewGuid().ToString());

            Guid newActivityId = Guid.NewGuid();

            Trace.CorrelationManager.ActivityId = newActivityId;

            try
            {
                target.Write(RecordType.Information, Guid.NewGuid().ToString());
            }
            finally
            {
                Trace.CorrelationManager.ActivityId = target.ActivityId;

                target.StopActivity(Guid.NewGuid().ToString());
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