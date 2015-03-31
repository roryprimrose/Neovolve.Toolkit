namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="RecordTraceTests"/>
    ///   class is used to test the <see cref="RecordTrace"/> class.
    /// </summary>
    [TestClass]
    public class RecordTraceTests
    {
        /// <summary>
        ///   Stores the test trace source.
        /// </summary>
        private static TraceSource _source;

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
            _source = new TraceSource("TestSource");

            _source.Switch.Level = SourceLevels.All;
            _source.Listeners.Add(new TestTraceListener());
        }

        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            TestTraceListener.Events.Clear();
            TestTraceListener.FlushCalled = false;
        }

        #endregion

        /// <summary>
        /// Runs test for can create from configuration and calling method.
        /// </summary>
        [TestMethod]
        public void CanInitializeFromConfigurationAndCallingMethodTest()
        {
            const String TraceSourceName = "Neovolve";
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            Collection<String> sourceNames = new Collection<String>
                                             {
                                                 TraceSourceName
                                             };
            TraceSource source = new TraceSource(TraceSourceName);

            resolver.Stub(x => x.ResolveNames()).Return(sourceNames);
            resolver.Stub(x => x.Resolve(TraceSourceName, StringComparison.OrdinalIgnoreCase)).Return(source);

            RecordTrace target = new RecordTrace(null, resolver);

            // Ensure that the instance is initialized
            target.Flush();
        }

        /// <summary>
        /// Runs test for can create from trace source name and resolver.
        /// </summary>
        [TestMethod]
        public void CanInitializeFromTraceSourceNameAndResolverTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();

            resolver.Stub(x => x.Resolve(sourceName, StringComparison.OrdinalIgnoreCase)).Return(_source);

            RecordTrace target = new RecordTrace(sourceName, resolver);

            // Ensure that the instance is initialized
            target.Flush();
        }

        /// <summary>
        /// Runs test for can create from trace source name resolver and throw setting.
        /// </summary>
        [TestMethod]
        public void CanInitializeFromTraceSourceNameResolverAndThrowSettingTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();

            using (mock.Record())
            {
                resolver.Resolve(sourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(_source);
            }

            using (mock.Playback())
            {
                RecordTrace target = new RecordTrace(sourceName, resolver, false);

                // Ensure that the instance is initialized
                target.Flush();
            }
        }

        /// <summary>
        /// Runs test for can create from trace source name.
        /// </summary>
        [TestMethod]
        public void CanInitializeFromTraceSourceNameTest()
        {
            new RecordTrace("SpecificNameSource");
        }

        /// <summary>
        /// A test for Flush.
        /// </summary>
        [TestMethod]
        public void FlushHasNoAffectWithNullSourceTest()
        {
            RecordTrace target = new RecordTrace("UnknownSource", null, false);

            Assert.IsFalse(TestTraceListener.FlushCalled, "FlushCalled returned an incorrect value");

            target.Flush();

            Assert.IsFalse(TestTraceListener.FlushCalled, "FlushCalled returned an incorrect value");
        }

        /// <summary>
        /// A test for Flush.
        /// </summary>
        [TestMethod]
        public void FlushTest()
        {
            RecordTrace target = new RecordTrace(_source);

            Assert.IsFalse(TestTraceListener.FlushCalled, "FlushCalled returned an incorrect value");

            target.Flush();

            Assert.IsTrue(TestTraceListener.FlushCalled, "FlushCalled returned an incorrect value");
        }

        /// <summary>
        /// Runs test for create throws exception when source cannot be resolved and throw setting is set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TraceSourceLoadException))]
        public void InitializeThrowsExceptionWhenSourceCannotBeResolvedAndThrowSettingIsSetTest()
        {
            String sourceName = Guid.NewGuid().ToString();
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();

            using (mock.Record())
            {
                resolver.Resolve(sourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(null);

                ITraceSourceResolver childResolver = resolver.ChildResolver;

                Debug.WriteLine(childResolver == null);

                LastCall.Return(null);

                resolver.ResolveNames();
                LastCall.Return(
                    new Collection<String>
                    {
                        sourceName
                    });
            }

            using (mock.Playback())
            {
                RecordTrace target = new RecordTrace(sourceName, resolver, true);

                // Ensure that the instance is initialized
                target.Flush();
            }
        }

        /// <summary>
        /// Runs test for no records are written when disabled.
        /// </summary>
        [TestMethod]
        public void NoRecordsAreWrittenWhenDisabledTest()
        {
            RecordTrace.Enabled = false;

            try
            {
                RecordTrace target = new RecordTrace(_source);

                target.Write(RecordType.Information, Guid.NewGuid().ToString());

                Assert.AreEqual(0, TestTraceListener.Events.Count, "Trace records were written when none were expected");

                target.Write(RecordType.Verbose, "Some value: {0}", 123);

                Assert.AreEqual(0, TestTraceListener.Events.Count, "Trace records were written when none were expected");

                target.Write(RecordType.Error, new TraceSourceLoadException());

                Assert.AreEqual(0, TestTraceListener.Events.Count, "Trace records were written when none were expected");
            }
            finally
            {
                RecordTrace.Enabled = true;
            }
        }

        /// <summary>
        /// Runs test for no records are written with disabled source.
        /// </summary>
        [TestMethod]
        public void NoRecordsAreWrittenWithDisabledSourceTest()
        {
            const String TraceSourceName = "DisabledSource";
            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            TraceSource source = new TraceSource(TraceSourceName)
                                 {
                                     Switch =
                                         {
                                             Level = SourceLevels.Off
                                         }
                                 };

            resolver.Stub(x => x.Resolve(TraceSourceName, StringComparison.OrdinalIgnoreCase)).Return(source);

            RecordTrace target = new RecordTrace(TraceSourceName, resolver);

            target.Write(RecordType.Information, Guid.NewGuid().ToString());

            Assert.AreEqual(0, TestTraceListener.Events.Count, "Trace records were written when none were expected");
        }

        /// <summary>
        /// Runs test for no records are written with null source.
        /// </summary>
        [TestMethod]
        public void NoRecordsAreWrittenWithNullSourceTest()
        {
            RecordTrace target = new RecordTrace("UnknownSource", null, false);

            target.Write(RecordType.Information, Guid.NewGuid().ToString());

            Assert.AreEqual(0, TestTraceListener.Events.Count, "Trace records were written when none were expected");
        }

        /// <summary>
        /// Runs test for trace activity caches trace source instances.
        /// </summary>
        [TestMethod]
        public void RecordTraceCachesTraceSourceInstancesTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            String traceSourceName = Guid.NewGuid().ToString();

            using (mock.Record())
            {
                resolver.Resolve(traceSourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(new TraceSource(traceSourceName));
            }

            using (mock.Playback())
            {
                RecordTrace firstTarget = new RecordTrace(traceSourceName, resolver, false);

                // Ensure that the instance is initialized
                firstTarget.Flush();

                RecordTrace secondTarget = new RecordTrace(traceSourceName, resolver, false);

                // Ensure that the instance is initialized
                secondTarget.Flush();
            }
        }

        /// <summary>
        /// A test for RecordTrace Constructor.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void RecordTraceLoadsSpecificTraceSourceFromConfigurationWithoutResolverTest()
        {
            const String SourceName = "SpecificNameSource";
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            TraceSource actual = new TraceSource(SourceName);

            using (mock.Record())
            {
                Expect.Call(resolver.ChildResolver).Return(null).Repeat.Any();

                resolver.Resolve(SourceName, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(actual);
            }

            TraceSourceResolverFactory.ResolverType = typeof(ConfigurationResolverMockWrapper);
            ConfigurationResolverMockWrapper.MockInstance = resolver;

            using (mock.Playback())
            {
                try
                {
                    RecordTrace target = new RecordTrace(SourceName);

                    // Invoke a trace message to ensure that the trace gets initialised
                    target.Write(RecordType.Information, Guid.NewGuid().ToString());
                }
                finally
                {
                    TraceSourceResolverFactory.ResolverType = null;
                    ConfigurationResolverMockWrapper.MockInstance = null;
                }
            }
        }

        /// <summary>
        /// Runs test for trace activity resolves trace source from configuration without resolver.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void RecordTraceResolvesTraceSourceFromConfigurationWithoutResolverTest()
        {
            const String Expected = "Neovolve.Toolkit.UnitTests.dll";
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            TraceSource actual = new TraceSource(Expected);
            Collection<String> names = new Collection<String>
                                       {
                                           Expected
                                       };

            using (mock.Record())
            {
                Expect.Call(resolver.ChildResolver).Return(null).Repeat.Any();

                resolver.ResolveNames();
                LastCall.Return(names);

                resolver.Resolve(Expected, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(actual);
            }

            TraceSourceResolverFactory.ResolverType = typeof(ConfigurationResolverMockWrapper);
            ConfigurationResolverMockWrapper.MockInstance = resolver;

            using (mock.Playback())
            {
                try
                {
                    RecordTrace target = new RecordTrace();

                    // Invoke a trace message to ensure that the trace gets initialised
                    target.Write(RecordType.Information, Guid.NewGuid().ToString());
                }
                finally
                {
                    TraceSourceResolverFactory.ResolverType = null;
                    ConfigurationResolverMockWrapper.MockInstance = null;
                }
            }
        }

        /// <summary>
        /// Runs test for write critical with exception correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteCriticalWithExceptionCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            Exception exception = new TraceSourceLoadException();

            target.Write(RecordType.Critical, exception);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Critical, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(exception.ToString(), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write critical with format string correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteCriticalWithFormatStringCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            const String Format = "Test message: {0}, {1}, {2}";
            Object[] args = new Object[]
                            {
                                "test", 123, true
                            };

            target.Write(RecordType.Critical, Format, args);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Critical, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(
                String.Format(CultureInfo.CurrentCulture, Format, args), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write critical with message correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteCriticalWithMessageCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            String message = Guid.NewGuid().ToString();

            target.Write(RecordType.Critical, message);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Critical, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(message, TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write critical with throws exception with null exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteCriticalWithThrowsExceptionWithNullExceptionTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Critical, (Exception)null);
        }

        /// <summary>
        /// Runs test for write Error with exception correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteErrorWithExceptionCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            Exception exception = new TraceSourceLoadException();

            target.Write(RecordType.Error, exception);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Error, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(exception.ToString(), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Error with format string correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteErrorWithFormatStringCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            const String Format = "Test message: {0}, {1}, {2}";
            Object[] args = new Object[]
                            {
                                "test", 123, true
                            };

            target.Write(RecordType.Error, Format, args);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Error, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(
                String.Format(CultureInfo.CurrentCulture, Format, args), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Error with message correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteErrorWithMessageCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            String message = Guid.NewGuid().ToString();

            target.Write(RecordType.Error, message);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Error, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(message, TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write error with throws exception with null exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteErrorWithThrowsExceptionWithNullExceptionTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Error, (Exception)null);
        }

        /// <summary>
        /// Runs test for write handles format masks without values.
        /// </summary>
        [TestMethod]
        public void WriteHandlesFormatMasksWithoutValuesTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Information, "Test {0}");

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Information, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual("Test {null}", TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Information with format string correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteInformationWithFormatStringCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            const String Format = "Test message: {0}, {1}, {2}";
            Object[] args = new Object[]
                            {
                                "test", 123, true
                            };

            target.Write(RecordType.Information, Format, args);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Information, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(
                String.Format(CultureInfo.CurrentCulture, Format, args), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Information with message correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteInformationWithMessageCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            String message = Guid.NewGuid().ToString();

            target.Write(RecordType.Information, message);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Information, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(message, TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write throws exception with empty message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteThrowsExceptionWithEmptyMessageTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Critical, String.Empty);
        }

        /// <summary>
        /// Runs test for write throws exception with invalid record type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WriteThrowsExceptionWithInvalidRecordTypeTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write((RecordType)Int32.MaxValue, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Runs test for write throws exception with null message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteThrowsExceptionWithNullMessageTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Critical, (String)null);
        }

        /// <summary>
        /// Runs test for write throws exception with white space message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteThrowsExceptionWithWhiteSpaceMessageTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Critical, "  ");
        }

        /// <summary>
        /// Runs test for write Verbose with format string correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteVerboseWithFormatStringCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            const String Format = "Test message: {0}, {1}, {2}";
            Object[] args = new Object[]
                            {
                                "test", 123, true
                            };

            target.Write(RecordType.Verbose, Format, args);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Verbose, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(
                String.Format(CultureInfo.CurrentCulture, Format, args), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Verbose with message correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteVerboseWithMessageCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            String message = Guid.NewGuid().ToString();

            target.Write(RecordType.Verbose, message);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Verbose, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(message, TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Warning with exception correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteWarningWithExceptionCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            Exception exception = new TraceSourceLoadException();

            target.Write(RecordType.Warning, exception);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Warning, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(exception.ToString(), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Warning with format string correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteWarningWithFormatStringCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            const String Format = "Test message: {0}, {1}, {2}";
            Object[] args = new Object[]
                            {
                                "test", 123, true
                            };

            target.Write(RecordType.Warning, Format, args);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Warning, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(
                String.Format(CultureInfo.CurrentCulture, Format, args), TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write Warning with message correctly writes trace record.
        /// </summary>
        [TestMethod]
        public void WriteWarningWithMessageCorrectlyWritesTraceRecordTest()
        {
            RecordTrace target = new RecordTrace(_source);
            String message = Guid.NewGuid().ToString();

            target.Write(RecordType.Warning, message);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Warning, TestTraceListener.Events[0].EventType, "Incorrect event type written");
            Assert.AreEqual(message, TestTraceListener.Events[0].Message, "Incorrect event message written");
        }

        /// <summary>
        /// Runs test for write warning with throws exception with null exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteWarningWithThrowsExceptionWithNullExceptionTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Warning, (Exception)null);
        }

        /// <summary>
        /// Runs test for write writes additional parameters without format masks.
        /// </summary>
        [TestMethod]
        public void WriteWritesAdditionalParametersWithoutFormatMasksTest()
        {
            RecordTrace target = new RecordTrace(_source);

            target.Write(RecordType.Information, "Test", 123123);

            Assert.AreEqual(1, TestTraceListener.Events.Count, "Incorrect number of events written");
            Assert.AreEqual(TraceEventType.Information, TestTraceListener.Events[0].EventType, "Incorrect event type written");

            const String ExpectedMessage = @"Test

Additional Parameters:
1. 123123";

            Assert.AreEqual(ExpectedMessage, TestTraceListener.Events[0].Message, "Incorrect event message written");
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