namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;
    using Rhino.Mocks;

    /// <summary>
    /// This is a test class for CallingMethodResolverTests and is intended
    ///   to contain all CallingMethodResolverTests Unit Tests.
    /// </summary>
    [TestClass]
    public class CallingMethodResolverTests
    {
        /// <summary>
        /// Runs test for can create with resolver.
        /// </summary>
        [TestMethod]
        public void CanCreateWithResolverTest()
        {
            ITraceSourceResolver childResolver = new TestTraceSourceResolver();
            CallingMethodResolver target = new CallingMethodResolver(childResolver);

            Assert.AreEqual(childResolver, target.ChildResolver, "ChildResolver returned an incorrect value");
        }

        /// <summary>
        /// Runs test for create with null resolver throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullResolverThrowsExceptionTest()
        {
            new CallingMethodResolver(null);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameAssemblyNameNoExtensionTest()
        {
            RunDetectTraceSourceNameTest("Neovolve.Toolkit.UnitTests", false);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameAssemblyNameTest()
        {
            RunDetectTraceSourceNameTest("Neovolve.Toolkit.UnitTests.dll", false);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameFullNamespaceTest()
        {
            RunDetectTraceSourceNameTest(GetType().FullName, true);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameInstrumentationNamespaceTest()
        {
            RunDetectTraceSourceNameTest("Neovolve.Toolkit.UnitTests.Instrumentation", true);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameNeovolveNamespaceTest()
        {
            RunDetectTraceSourceNameTest("Neovolve", false);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameToolkitNamespaceTest()
        {
            RunDetectTraceSourceNameTest("Neovolve.Toolkit", false);
        }

        /// <summary>
        /// Gets the trace source name for method test.
        /// </summary>
        [TestMethod]
        [Description("White box tests")]
        public void DetectTraceSourceNameUnitTestsNamespaceTest()
        {
            RunDetectTraceSourceNameTest("Neovolve.Toolkit.UnitTests", false);
        }

        /// <summary>
        /// Runs test for evaluate matches trace source name with different case.
        /// </summary>
        [TestMethod]
        public void EvaluateMatchesTraceSourceNameWithDifferentCaseTest()
        {
            ITraceSourceResolver childResolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            const String Expected = "NEOVOLVE";
            Collection<String> testNames = new Collection<String>
                                           {
                                               Guid.NewGuid().ToString(), 
                                               Expected
                                           };

            childResolver.Stub(x => x.ResolveNames()).Return(testNames);

            CallingMethodResolver target = new CallingMethodResolver(childResolver);

            CallingMethodResult actual = target.Evaluate();

            Assert.IsNotNull(actual, "Evaluate failed to return an instance");
            Assert.AreEqual("Neovolve", actual.TraceSourceName, "TraceSourceName returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate matches trace source name with same case.
        /// </summary>
        [TestMethod]
        public void EvaluateMatchesTraceSourceNameWithSameCaseTest()
        {
            ITraceSourceResolver childResolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            const String Expected = "Neovolve";
            Collection<String> testNames = new Collection<String>
                                           {
                                               Guid.NewGuid().ToString(), 
                                               Expected
                                           };

            childResolver.Stub(x => x.ResolveNames()).Return(testNames);

            CallingMethodResolver target = new CallingMethodResolver(childResolver);

            CallingMethodResult actual = target.Evaluate();

            Assert.IsNotNull(actual, "Evaluate failed to return an instance");
            Assert.AreEqual(Expected, actual.TraceSourceName, "TraceSourceName returned an incorrect value");
        }

        /// <summary>
        /// Runs test for evaluate returns correct method information.
        /// </summary>
        [TestMethod]
        public void EvaluateReturnsCorrectMethodInformationTest()
        {
            const String SourceName = "Neovolve";
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            CallingMethodResolver target = new CallingMethodResolver(resolver);

            using (mock.Record())
            {
                resolver.ResolveNames();

                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });
            }

            using (mock.Playback())
            {
                CallingMethodResult actual = target.Evaluate();

                Assert.AreEqual(MethodBase.GetCurrentMethod(), actual.CallingMethod, "CallingMethod returned an incorrect value");
                Assert.AreEqual(MethodBase.GetCurrentMethod().DeclaringType, actual.CallingType, "CallingMethod returned an incorrect value");
                Assert.AreEqual(SourceName, actual.TraceSourceName, "TraceSourceName returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for evaluate returns stored value on subsequent call.
        /// </summary>
        [TestMethod]
        public void EvaluateReturnsStoredValueOnSubsequentCallTest()
        {
            const String SourceName = "Neovolve";
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            CallingMethodResolver target = new CallingMethodResolver(resolver);

            using (mock.Record())
            {
                resolver.ResolveNames();

                LastCall.Return(
                    new Collection<String>
                    {
                        SourceName
                    });
            }

            using (mock.Playback())
            {
                target.Evaluate();

                // Make the call again, only one call to ResolveNames should be made
                target.Evaluate();
            }
        }

        /// <summary>
        /// Runs test for evaluate returns unnamed trace source for no trace source name match.
        /// </summary>
        [TestMethod]
        public void EvaluateReturnsUnnamedTraceSourceForNoTraceSourceNameMatchTest()
        {
            ITraceSourceResolver childResolver = MockRepository.GenerateStub<ITraceSourceResolver>();
            Collection<String> testNames = new Collection<String>
                                           {
                                               Guid.NewGuid().ToString(), 
                                               Guid.NewGuid().ToString()
                                           };

            childResolver.Stub(x => x.ResolveNames()).Return(testNames);

            CallingMethodResolver target = new CallingMethodResolver(childResolver);

            CallingMethodResult actual = target.Evaluate();

            Assert.IsTrue(String.IsNullOrWhiteSpace(actual.TraceSourceName), "TraceSourceName returned an incorrect value");
        }

        /// <summary>
        /// Runs test for find returns calling method resolver from resolver chain.
        /// </summary>
        [TestMethod]
        public void FindReturnsCallingMethodResolverFromResolverChainTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            CallingMethodResolver target = new CallingMethodResolver(resolver);

            ITraceSourceResolver actual = CallingMethodResolver.Find(target);

            Assert.AreEqual(target, actual, "Find returned an incorrect value");
        }

        /// <summary>
        /// Runs test for find returns correct resolver with resolver chain that contains calling method resolver.
        /// </summary>
        [TestMethod]
        public void FindReturnsCorrectResolverWithResolverChainThatContainsCallingMethodResolverTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            CallingMethodResolver middleResolver = new CallingMethodResolver(resolver);
            TestTraceSourceResolver target = new TestTraceSourceResolver(middleResolver);

            ITraceSourceResolver actual = CallingMethodResolver.Find(target);

            Assert.AreEqual(middleResolver, actual, "Find returned an incorrect value");
        }

        /// <summary>
        /// Runs test for find returns new resolver with resolver chain that does not contain calling method resolver.
        /// </summary>
        [TestMethod]
        public void FindReturnsNewResolverWithResolverChainThatDoesNotContainCallingMethodResolverTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            TestTraceSourceResolver target = new TestTraceSourceResolver(resolver);

            using (mock.Record())
            {
                ITraceSourceResolver nullResolver = resolver.ChildResolver;
                Assert.IsNull(nullResolver, "ChildResolver returned an incorrect value");
                LastCall.Return(null);
            }

            using (mock.Playback())
            {
                CallingMethodResolver actual = CallingMethodResolver.Find(target);

                Assert.IsNotNull(actual, "Find returned an incorrect value");
                Assert.AreEqual(target, actual.ChildResolver, "ChildResolver returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for find with null resolver throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindWithNullResolverThrowsExceptionTest()
        {
            CallingMethodResolver.Find(null);
        }

        /// <summary>
        /// Runs test for reload calls reload on child resolver.
        /// </summary>
        [TestMethod]
        public void ReloadCallsReloadOnChildResolverTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver childResolver = mock.StrictMock<ITraceSourceResolver>();

            using (mock.Record())
            {
                childResolver.Reload();
            }

            using (mock.Playback())
            {
                CallingMethodResolver target = new CallingMethodResolver(childResolver);

                target.Reload();
            }
        }

        /// <summary>
        /// Runs test for resolve names returns names from child resolver.
        /// </summary>
        [TestMethod]
        public void ResolveNamesReturnsNamesFromChildResolverTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            String firstName = Guid.NewGuid().ToString();
            const String SecondName = "Test";
            Collection<String> testNames = new Collection<String>
                                           {
                                               firstName, 
                                               SecondName
                                           };

            using (mock.Record())
            {
                resolver.ResolveNames();
                LastCall.Return(testNames);
            }

            using (mock.Playback())
            {
                CallingMethodResolver target = new CallingMethodResolver(resolver);
                Collection<String> actual = target.ResolveNames();

                Assert.AreEqual(testNames, actual, "ResolveNames returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for resolve returns source from child resolver.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsSourceFromChildResolverTest()
        {
            MockRepository mock = new MockRepository();
            ITraceSourceResolver resolver = mock.StrictMock<ITraceSourceResolver>();
            const String Name = "Test";
            TraceSource expected = new TraceSource(Name);

            using (mock.Record())
            {
                resolver.Resolve(Name, StringComparison.OrdinalIgnoreCase);
                LastCall.Return(expected);
            }

            using (mock.Playback())
            {
                CallingMethodResolver target = new CallingMethodResolver(resolver);
                TraceSource actual = target.Resolve(Name, StringComparison.OrdinalIgnoreCase);

                Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
            }
        }

        #region Static Helper Methods

        /// <summary>
        /// Runs the get trace source name for method test.
        /// </summary>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <param name="skipAssemblyNameValues">
        /// If set to <c>true</c> [skip assembly name values].
        /// </param>
        private static void RunDetectTraceSourceNameTest(String expected, Boolean skipAssemblyNameValues)
        {
            Collection<String> expectedNames = new Collection<String>
                                               {
                                                   "ignored"
                                               };
            Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            String assemblyPath = declaringType.Assembly.Location;

            if (Path.GetFileName(assemblyPath).Equals(expected, StringComparison.OrdinalIgnoreCase))
            {
                expectedNames.Add(expected);
            }

            // Populate the list with the set of items descending in size using . as the delimiter
            if (skipAssemblyNameValues == false)
            {
                if (Path.GetFileNameWithoutExtension(assemblyPath).Equals(expected, StringComparison.OrdinalIgnoreCase))
                {
                    expectedNames.Add(expected);
                }
            }

            String typeValue = declaringType.FullName;
            Boolean valueFound = false;

            while (String.IsNullOrEmpty(typeValue) == false)
            {
                if (typeValue.Equals(expected, StringComparison.OrdinalIgnoreCase) || valueFound)
                {
                    if (Path.GetFileNameWithoutExtension(assemblyPath).Equals(typeValue, StringComparison.OrdinalIgnoreCase) == false ||
                        skipAssemblyNameValues == false)
                    {
                        expectedNames.Add(typeValue);

                        valueFound = true;
                    }
                }

                // Check if there is a . in the name
                Int32 lastIndex = typeValue.LastIndexOf('.');

                if (lastIndex > -1)
                {
                    // There is another namespace value to check
                    typeValue = typeValue.Substring(0, lastIndex);
                }
                else
                {
                    // We have exhausted all options using the namespace
                    break;
                }
            }

            ITraceSourceResolver resolver = MockRepository.GenerateStub<ITraceSourceResolver>();

            resolver.Stub(x => x.ResolveNames()).Return(expectedNames);

            CallingMethodResolver target = new CallingMethodResolver(resolver);

            CallingMethodResult result = target.Evaluate();

            Assert.IsNotNull(result, "Evaluate returned an incorrect value");

            String actual = result.TraceSourceName;

            Assert.AreEqual(expected, actual, "DetectTraceSourceName returned an incorrect value");
        }

        #endregion

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