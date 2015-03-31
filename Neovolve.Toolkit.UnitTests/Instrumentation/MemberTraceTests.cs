namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;

    /// <summary>
    /// The <see cref="MemberTraceTests"/>
    ///   class contains unit tests for the <see cref="MemberTrace"/> class.
    /// </summary>
    [TestClass]
    public class MemberTraceTests
    {
        /// <summary>
        ///   Stores the test trace source.
        /// </summary>
        private static readonly TraceSource _testSource = new TraceSource("TestSource");

        #region Setup/Teardown

        /// <summary>
        /// InitializeContexts the test.
        /// </summary>
        [TestInitialize]
        public void TestInitializeContext()
        {
            TestTraceListener.Clear();
            RecordTrace.ThrowOnSourceFailure = true;
        }

        #endregion

        /// <summary>
        /// Runs test for calling state on disposed member trace throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingStateOnDisposedMemberTraceThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                ActivityTraceState state = trace.State;

                Debug.WriteLine(state);
            }
        }

        /// <summary>
        /// Runs test for calling write critical with exception forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteCriticalWithExceptionForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            InvalidOperationException exception = new InvalidOperationException();

            using (mock.Record())
            {
                writer.Write(RecordType.Critical, exception);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Critical, exception);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write critical with exception when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteCriticalWithExceptionWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Critical, new InvalidOperationException());
            }
        }

        /// <summary>
        /// Runs test for calling write critical with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteCriticalWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.Write(RecordType.Critical, Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Critical, Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write critical with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteCriticalWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Critical, "{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Writes the critical string test.
        /// </summary>
        [TestMethod]
        public void CallingWriteCriticalWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.Write(RecordType.Critical, message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Critical, message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write critical with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteCriticalWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Critical, Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write error with exception forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteErrorWithExceptionForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            InvalidOperationException exception = new InvalidOperationException();

            using (mock.Record())
            {
                writer.Write(RecordType.Error, exception);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Error, exception);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write error with exception when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteErrorWithExceptionWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Error, new InvalidOperationException());
            }
        }

        /// <summary>
        /// Runs test for calling write error with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteErrorWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.Write(RecordType.Error, Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Error, Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write error with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteErrorWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Error, "{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write error with message forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteErrorWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.Write(RecordType.Error, message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Error, message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write error with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteErrorWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Error, Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write information with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteInformationWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.Write(RecordType.Information, Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Information, Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write information with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteInformationWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Information, "{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write information with message forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteInformationWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.Write(RecordType.Information, message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Information, message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write information with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteInformationWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Information, Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write resume with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteResumeWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.ResumeActivity(Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.ResumeActivity(Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write resume with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteResumeWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.ResumeActivity("{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write resume with message forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteResumeWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.ResumeActivity(message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.ResumeActivity(message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write resume with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteResumeWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.ResumeActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write start with string format not supported.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallingWriteStartWithStringFormatNotSupportedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                ((IActivityWriter)trace).StartActivity("{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write start with string not supported.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallingWriteStartWithStringNotSupportedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                ((IActivityWriter)trace).StartActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write stop with string format not supported.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallingWriteStopWithStringFormatNotSupportedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                ((IActivityWriter)trace).StopActivity("{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write stop with string not supported.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CallingWriteStopWithStringNotSupportedTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                ((IActivityWriter)trace).StopActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write suspend with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteSuspendWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.SuspendActivity(Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.SuspendActivity(Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write suspend with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteSuspendWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.SuspendActivity("{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write suspend with message forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteSuspendWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.SuspendActivity(message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.SuspendActivity(message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write suspend with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteSuspendWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.SuspendActivity(Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write verbose with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteVerboseWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.Write(RecordType.Verbose, Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Verbose, Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write verbose with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteVerboseWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Verbose, "{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write verbose with message forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteVerboseWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.Write(RecordType.Verbose, message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Verbose, message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write verbose with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteVerboseWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Verbose, Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write warning with exception forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteWarningWithExceptionForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            InvalidOperationException exception = new InvalidOperationException();

            using (mock.Record())
            {
                writer.Write(RecordType.Warning, exception);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Warning, exception);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write warning with exception when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteWarningWithExceptionWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Warning, new InvalidOperationException());
            }
        }

        /// <summary>
        /// Runs test for calling write warning with format string forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteWarningWithFormatStringForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            const String Format = "Test {0} message format";
            const String Args = "testedValue";

            using (mock.Record())
            {
                writer.Write(RecordType.Warning, Format, Args);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Warning, Format, Args);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write warning with format string when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteWarningWithFormatStringWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Warning, "{0}", Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for calling write warning with message forwards call to writer.
        /// </summary>
        [TestMethod]
        public void CallingWriteWarningWithMessageForwardsCallToWriterTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();
            String message = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                writer.Write(RecordType.Warning, message);
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Write(RecordType.Warning, message);
                }
            }
        }

        /// <summary>
        /// Runs test for calling write warning with message when disposed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CallingWriteWarningWithMessageWhenDisposedThrowsExceptionTest()
        {
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                trace.Dispose();

                trace.Write(RecordType.Warning, Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Runs test for can build start method without thread identity.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void CanBuildStartMethodWithoutThreadIdentityTest()
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            IPrincipal mockedPrincipal = MockRepository.GenerateStub<IPrincipal>();

            mockedPrincipal.Stub(x => x.Identity).Return(null);

            Thread.CurrentPrincipal = mockedPrincipal;

            try
            {
                TraceSource source = new TraceSource(Guid.NewGuid().ToString());

                using (new MemberTrace(source))
                {
                }
            }
            finally
            {
                Thread.CurrentPrincipal = principal;
            }
        }

        /// <summary>
        /// Runs test for can build start method without thread principal.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void CanBuildStartMethodWithoutThreadPrincipalTest()
        {
            IPrincipal principal = Thread.CurrentPrincipal;

            Thread.CurrentPrincipal = null;

            try
            {
                TraceSource source = new TraceSource(Guid.NewGuid().ToString(), SourceLevels.All);

                using (new MemberTrace(source))
                {
                }
            }
            finally
            {
                Thread.CurrentPrincipal = principal;
            }
        }

        /// <summary>
        /// Runs test for creating member trace with I activity writer writes boundary messages.
        /// </summary>
        [TestMethod]
        public void CreatingMemberTraceWithIActivityWriterWritesBoundaryMessagesTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.StrictMultiMock<IActivityWriter>(typeof(IDisposable));

            using (mock.Record())
            {
                ActivityTraceState state = writer.State;
                Debug.WriteLine("Current state is: " + state);
                LastCall.Return(ActivityTraceState.Stopped).Repeat.Any();

                writer.StartActivity(String.Empty);
                LastCall.IgnoreArguments();

                writer.StopActivity(String.Empty);
                LastCall.IgnoreArguments();

                ((IDisposable)writer).Dispose();
            }

            using (mock.Playback())
            {
                using (new MemberTrace(writer))
                {
                }
            }
        }

        /// <summary>
        /// Runs test for creating when unable to resolve source name throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TraceSourceLoadException))]
        public void CreatingWhenUnableToResolveSourceNameThrowsExceptionTest()
        {
            using (new MemberTrace("SomeTraceSourceThatDoesn'tExist"))
            {
            }
        }

        /// <summary>
        /// Runs test for creating with default constructor writes boundary messages.
        /// </summary>
        [TestMethod]
        public void CreatingWithDefaultConstructorWritesBoundaryMessagesTest()
        {
            TraceSourceResolverFactory.ResolverType = typeof(TestTraceSourceResolver);

            using (new MemberTrace())
            {
                Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
                Assert.AreEqual(TraceEventType.Start, TestTraceListener.Events[0].EventType, "EventType returned an incorrect value");
            }

            Assert.AreEqual(2, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Stop, TestTraceListener.Events[1].EventType, "EventType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating with method to trace and null writer throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingWithMethodToTraceAndNullWriterThrowsExceptionTest()
        {
            MethodBase current = MethodBase.GetCurrentMethod();

            using (new MemberTrace((IActivityWriter)null, current))
            {
            }
        }

        /// <summary>
        /// Runs test for creating with null I activity writer throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingWithNullIActivityWriterThrowsExceptionTest()
        {
            using (new MemberTrace((IActivityWriter)null))
            {
            }
        }

        /// <summary>
        /// Runs test for creating with source name and current member writes boundary messages.
        /// </summary>
        [TestMethod]
        public void CreatingWithSourceNameAndCurrentMemberWritesBoundaryMessagesTest()
        {
            TraceSourceResolverFactory.ResolverType = typeof(TestTraceSourceResolver);

            using (new MemberTrace(TestTraceSourceResolver.SourceName, MethodBase.GetCurrentMethod()))
            {
                Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
                Assert.AreEqual(TraceEventType.Start, TestTraceListener.Events[0].EventType, "EventType returned an incorrect value");
            }

            Assert.AreEqual(2, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Stop, TestTraceListener.Events[1].EventType, "EventType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating with source name writes boundary messages.
        /// </summary>
        [TestMethod]
        public void CreatingWithSourceNameWritesBoundaryMessagesTest()
        {
            TraceSourceResolverFactory.ResolverType = typeof(TestTraceSourceResolver);

            using (new MemberTrace(TestTraceSourceResolver.SourceName))
            {
                Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
                Assert.AreEqual(TraceEventType.Start, TestTraceListener.Events[0].EventType, "EventType returned an incorrect value");
            }

            Assert.AreEqual(2, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Stop, TestTraceListener.Events[1].EventType, "EventType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating with source writes boundary messages.
        /// </summary>
        [TestMethod]
        public void CreatingWithSourceWritesBoundaryMessagesTest()
        {
            MockRepository mock = new MockRepository();
            TraceListener listener = mock.PartialMock<TraceListener>();
            TraceSource source = new TraceSource("test");
            String message = Guid.NewGuid().ToString();

            source.Switch.Level = SourceLevels.All;
            source.Listeners.Add(listener);

            using (mock.Record())
            {
                listener.TraceEvent(null, source.Name, TraceEventType.Start, 0, message);
                LastCall.Constraints(Is.Anything(), Is.Equal(source.Name), Is.Equal(TraceEventType.Start), Is.Equal(0), Is.NotNull());

                listener.TraceEvent(null, source.Name, TraceEventType.Stop, 0, message);
                LastCall.Constraints(Is.Anything(), Is.Equal(source.Name), Is.Equal(TraceEventType.Stop), Is.Equal(0), Is.NotNull());
            }

            using (mock.Playback())
            {
                using (new MemberTrace(source))
                {
                }
            }
        }

        /// <summary>
        /// Runs test for disabled member trace does not trace messages.
        /// </summary>
        [TestMethod]
        public void DisabledMemberTraceDoesNotTraceBoundaryMessagesTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.StrictMock<IActivityWriter>();

            using (mock.Record())
            {
                ActivityTraceState state = writer.State;
                Debug.WriteLine(state);

                LastCall.Return(ActivityTraceState.Disabled);
                LastCall.Repeat.Twice();
            }

            using (mock.Playback())
            {
                using (new MemberTrace(writer))
                {
                }
            }
        }

        /// <summary>
        /// Runs test for flush invokes writer flush.
        /// </summary>
        [TestMethod]
        public void FlushInvokesWriterFlushTest()
        {
            MockRepository mock = new MockRepository();
            IActivityWriter writer = mock.DynamicMock<IActivityWriter>();

            using (mock.Record())
            {
                writer.Flush();
            }

            using (mock.Playback())
            {
                using (MemberTrace trace = new MemberTrace(writer))
                {
                    trace.Flush();
                }
            }
        }

        /// <summary>
        /// Runs test for flush on disposed member trace throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void FlushOnDisposedMemberTraceThrowsExceptionTest()
        {
            using (MemberTrace trace = new MemberTrace(_testSource))
            {
                trace.Dispose();
                trace.Flush();
            }
        }

        /// <summary>
        /// Runs test for multiple dispose does not throw exception.
        /// </summary>
        [TestMethod]
        public void MultipleDisposeDoesNotThrowExceptionTest()
        {
            using (MemberTrace trace = new MemberTrace(_testSource))
            {
                trace.Dispose();
                trace.Dispose();
            }
        }

        /// <summary>
        /// Runs test for state progression.
        /// </summary>
        [TestMethod]
        public void StateProgressionTest()
        {
            RecordTrace.ThrowOnSourceFailure = false;
            TestTraceSourceResolver resolver = new TestTraceSourceResolver();

            using (MemberTrace traceDisabled = new MemberTrace(Guid.NewGuid().ToString(), null, resolver))
            {
                Assert.AreEqual(ActivityTraceState.Disabled, traceDisabled.State, "MemberTrace is not disabled");
            }

            using (MemberTrace trace = new MemberTrace(TestTraceSourceResolver.SourceName, null, resolver))
            {
                Assert.AreEqual(ActivityTraceState.Running, trace.State, "MemberTrace is not running");

                trace.SuspendActivity("We are suspending this trace");

                Assert.AreEqual(ActivityTraceState.Suspended, trace.State, "MemberTrace is not suspended");

                trace.ResumeActivity("We are resuming this trace");

                Assert.AreEqual(ActivityTraceState.Running, trace.State, "MemberTrace is not running");
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