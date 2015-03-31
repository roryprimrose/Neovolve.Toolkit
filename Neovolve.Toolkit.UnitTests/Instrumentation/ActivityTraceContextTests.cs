namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="ActivityTraceContextTests"/>
    ///   class is used to test the <see cref="ActivityTraceContext"/> class.
    /// </summary>
    [TestClass]
    public class ActivityTraceContextTests
    {
        /// <summary>
        /// Runs test for context defaults to disabled state.
        /// </summary>
        [TestMethod]
        public void ContextDefaultsToDisabledStateTest()
        {
            ActivityTraceContext target = new ActivityTraceContext();

            Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
        }

        /// <summary>
        /// Runs test for empty correct equals empty.
        /// </summary>
        [TestMethod]
        public void EmptyCorrectEqualsEmptyTest()
        {
            Assert.IsTrue(ActivityTraceContext.Empty.Equals(ActivityTraceContext.Empty), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals returns false for different context with different values.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsFalseForDifferentContextWithDifferentValuesTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.ActivityId = Guid.NewGuid();
            targetA.PreviousActivityId = Guid.NewGuid();
            targetA.State = ActivityTraceState.Running;
            targetA.Source = new TraceSource("test");

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.State = targetA.State;
            targetB.Source = targetA.Source;

            Assert.IsFalse(targetA.Equals(targetB), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals returns false for invalid type.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsFalseForInvalidTypeTest()
        {
            ActivityTraceContext target = new ActivityTraceContext();

            Assert.IsFalse(target.Equals(true), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals returns true for different context with same values.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsTrueForDifferentContextWithSameValuesTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.ActivityId = Guid.NewGuid();
            targetA.PreviousActivityId = Guid.NewGuid();
            targetA.State = ActivityTraceState.Running;
            targetA.Source = new TraceSource("test");

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.ActivityId = targetA.ActivityId;
            targetB.PreviousActivityId = targetA.PreviousActivityId;
            targetB.State = targetA.State;
            targetB.Source = targetA.Source;

            Assert.IsTrue(targetA.Equals(targetB), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals returns true for same context instance.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsTrueForSameContextInstanceTest()
        {
            ActivityTraceContext target = new ActivityTraceContext();

            Assert.IsTrue(target.Equals(target), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals with null returns false.
        /// </summary>
        [TestMethod]
        public void EqualsWithNullReturnsFalseTest()
        {
            ActivityTraceContext target = new ActivityTraceContext();

            Assert.IsFalse(target.Equals(null), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different instance with different activity id.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentInstanceWithDifferentActivityIdTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.ActivityId = Guid.NewGuid();

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.ActivityId = Guid.NewGuid();

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different instance with different previous activity id.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentInstanceWithDifferentPreviousActivityIdTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.PreviousActivityId = Guid.NewGuid();

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.PreviousActivityId = Guid.NewGuid();

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different instance with different source.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentInstanceWithDifferentSourceTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.Source = new TraceSource("test");

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.Source = new TraceSource("test");

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different instance with different state.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentInstanceWithDifferentStateTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.State = ActivityTraceState.Stopped;
            targetA.Source = new TraceSource("test");

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.State = ActivityTraceState.Suspended;

            // The same source instance must be provided to test State in GetHashCode as State returns Disabled if Source is null
            // The Source instance must be the same in order to ensure that this test only tests the GetHashCode outcome for the State property alone
            targetB.Source = targetA.Source;

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with null source.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithNullSourceTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.Source = null;

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.Source = targetA.Source;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with same activity id.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithSameActivityIdTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.ActivityId = Guid.NewGuid();

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.ActivityId = targetA.ActivityId;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with same previous activity id.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithSamePreviousActivityIdTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.PreviousActivityId = Guid.NewGuid();

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.PreviousActivityId = targetA.PreviousActivityId;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with same source.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithSameSourceTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.Source = new TraceSource("test");

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.Source = targetA.Source;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with same state.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithSameStateTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.State = ActivityTraceState.Stopped;

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.State = targetA.State;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with same values and null source.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithSameValuesAndNullSourceTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.ActivityId = Guid.NewGuid();
            targetA.PreviousActivityId = Guid.NewGuid();
            targetA.State = ActivityTraceState.Running;
            targetA.Source = null;

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.ActivityId = targetA.ActivityId;
            targetB.PreviousActivityId = targetA.PreviousActivityId;
            targetB.State = targetA.State;
            targetB.Source = targetA.Source;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for different instance with same values.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForDifferentInstanceWithSameValuesTest()
        {
            ActivityTraceContext targetA = new ActivityTraceContext();

            targetA.ActivityId = Guid.NewGuid();
            targetA.PreviousActivityId = Guid.NewGuid();
            targetA.State = ActivityTraceState.Running;
            targetA.Source = new TraceSource("test");

            ActivityTraceContext targetB = new ActivityTraceContext();

            targetB.ActivityId = targetA.ActivityId;
            targetB.PreviousActivityId = targetA.PreviousActivityId;
            targetB.State = targetA.State;
            targetB.Source = targetA.Source;

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for two empty context values.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForTwoEmptyContextValuesTest()
        {
            Assert.AreEqual(
                ActivityTraceContext.Empty.GetHashCode(), ActivityTraceContext.Empty.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for state returns disabled with null source.
        /// </summary>
        [TestMethod]
        public void StateReturnsDisabledWithNullSourceTest()
        {
            ActivityTraceContext target = new ActivityTraceContext();

            target.State = ActivityTraceState.Suspended;

            Assert.AreEqual(ActivityTraceState.Disabled, target.State, "State returned an incorrect value");
        }

        /// <summary>
        /// Runs test for state returns stored value with source instance.
        /// </summary>
        [TestMethod]
        public void StateReturnsStoredValueWithSourceInstanceTest()
        {
            ActivityTraceContext target = new ActivityTraceContext();

            target.State = ActivityTraceState.Running;
            target.Source = new TraceSource("test");

            Assert.AreEqual(ActivityTraceState.Running, target.State, "State returned an incorrect value");
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