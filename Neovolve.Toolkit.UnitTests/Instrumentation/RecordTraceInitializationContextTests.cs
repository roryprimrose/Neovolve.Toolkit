namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="RecordTraceInitializationContextTests"/>
    ///   class is used to test the <see cref="RecordTraceInitializationContext"/> class.
    /// </summary>
    [TestClass]
    public class RecordTraceInitializationContextTests
    {
        /// <summary>
        /// Runs test for can create with source.
        /// </summary>
        [TestMethod]
        public void CanCreateWithSourceTest()
        {
            TraceSource source = new TraceSource(Guid.NewGuid().ToString());

            RecordTraceInitializationContext target = new RecordTraceInitializationContext(source);

            Assert.AreSame(source, target.Source, "Source returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can create with trace source name resolver and throw indicator.
        /// </summary>
        [TestMethod]
        public void CanCreateWithTraceSourceNameResolverAndThrowIndicatorTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            ITraceSourceResolver resolver = new TestTraceSourceResolver();
            const Boolean ThrowFlag = true;

            RecordTraceInitializationContext target = new RecordTraceInitializationContext(sourceName, resolver, ThrowFlag);

            Assert.AreEqual(sourceName, target.TraceSourceName, "TraceSourceName returned an incorrect value");
            Assert.AreSame(resolver, target.Resolver, "Resolver returned an incorrect value");
            Assert.AreEqual(ThrowFlag, target.ThrowOnSourceLoadFailure, "ThrowOnSourceLoadFailure returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equality returns false for different context values.
        /// </summary>
        [TestMethod]
        public void EqualityReturnsFalseForDifferentContextValuesTest()
        {
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);

            Assert.IsFalse(targetA == targetB, "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equality returns true for same context values.
        /// </summary>
        [TestMethod]
        public void EqualityReturnsTrueForSameContextValuesTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(sourceName, null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(sourceName, null, true);

            Assert.IsTrue(targetA == targetB, "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equality test with incorrect type returns false.
        /// </summary>
        [TestMethod]
        public void EqualityTestWithIncorrectTypeReturnsFalseTest()
        {
            RecordTraceInitializationContext target = new RecordTraceInitializationContext(null);

            Assert.IsFalse(target.Equals(true), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equality test with null returns false.
        /// </summary>
        [TestMethod]
        public void EqualityTestWithNullReturnsFalseTest()
        {
            RecordTraceInitializationContext target = new RecordTraceInitializationContext(null);

            Assert.IsFalse(target.Equals(null), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals returns false for different context values.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsFalseForDifferentContextValuesTest()
        {
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);

            Assert.IsFalse(targetA.Equals(targetB), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for equals returns true for same context values.
        /// </summary>
        [TestMethod]
        public void EqualsReturnsTrueForSameContextValuesTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(sourceName, null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(sourceName, null, true);

            Assert.IsTrue(targetA.Equals(targetB), "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different trace source name.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentResolverTest()
        {
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(null, new TestTraceSourceResolver(), false);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(null, new TestTraceSourceResolver(), false);

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different source.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentSourceTest()
        {
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(new TraceSource(Guid.NewGuid().ToString()));
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(new TraceSource(Guid.NewGuid().ToString()));

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns different value for different trace source name.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsDifferentValueForDifferentTraceSourceNameTest()
        {
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);

            Assert.AreNotEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for same resolver.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForSameResolverTest()
        {
            ITraceSourceResolver resolver = new TestTraceSourceResolver();
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(null, resolver, false);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(null, resolver, false);

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for same source.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForSameSourceTest()
        {
            TraceSource source = new TraceSource(Guid.NewGuid().ToString());
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(source);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(source);

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get hash code returns same value for same trace source name.
        /// </summary>
        [TestMethod]
        public void GetHashCodeReturnsSameValueForSameTraceSourceNameTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(sourceName, null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(sourceName, null, true);

            Assert.AreEqual(targetA.GetHashCode(), targetB.GetHashCode(), "GetHashCode returned an incorrect value");
        }

        /// <summary>
        /// Runs test for inequality returns false for different context values.
        /// </summary>
        [TestMethod]
        public void InequalityReturnsFalseForDifferentContextValuesTest()
        {
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(Guid.NewGuid().ToString(), null, true);

            Assert.IsTrue(targetA != targetB, "Equals returned an incorrect value");
        }

        /// <summary>
        /// Runs test for inequality returns true for same context values.
        /// </summary>
        [TestMethod]
        public void InequalityReturnsTrueForSameContextValuesTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            RecordTraceInitializationContext targetA = new RecordTraceInitializationContext(sourceName, null, true);
            RecordTraceInitializationContext targetB = new RecordTraceInitializationContext(sourceName, null, true);

            Assert.IsFalse(targetA != targetB, "Equals returned an incorrect value");
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