namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using System.ServiceModel.Description;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// This is a test class for ErrorHandlerAttributeTests and is intended
    ///   to contain all ErrorHandlerAttributeTests Unit Tests.
    /// </summary>
    [TestClass]
    public class ErrorHandlerAttributeTests
    {
        /// <summary>
        /// A test for AddBindingParameters.
        /// </summary>
        [TestMethod]
        public void AddBindingParametersTest()
        {
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(typeof(KnownErrorHandler));
            IServiceBehavior behaviour = target;

            // AddBindingParameters currently has not implementation
            behaviour.AddBindingParameters(null, null, null, null);
        }

        /// <summary>
        /// Runs test for apply dispatch behavior throws exception with null service host base.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplyDispatchBehaviorThrowsExceptionWithNullServiceHostBaseTest()
        {
            ServiceDescription serviceDescription = new ServiceDescription();
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(typeof(KnownErrorHandler));

            target.ApplyDispatchBehavior(serviceDescription, null);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ErrorHandlerAttributeConstructorStringArrayEmptyStringArrayTest()
        {
            String[] errorHandlerTypeNames = new String[0];

            new ErrorHandlerAttribute(errorHandlerTypeNames);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringArrayEmptyStringTest()
        {
            String[] errorHandlerTypeName = new[]
                                            {
                                                String.Empty
                                            };

            new ErrorHandlerAttribute(errorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ErrorHandlerAttributeConstructorStringArrayIncorrectTypeTest()
        {
            String[] errorHandlerTypeName = new[]
                                            {
                                                typeof(String).AssemblyQualifiedName
                                            };

            new ErrorHandlerAttribute(errorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void ErrorHandlerAttributeConstructorStringArrayInvalidTypeNameTest()
        {
            String errorHandlerTypeName = Guid.NewGuid().ToString();

            new ErrorHandlerAttribute(errorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringArrayNullStringArrayTest()
        {
            const String[] ErrorHandlerTypeNames = null;

            new ErrorHandlerAttribute(ErrorHandlerTypeNames);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringArrayNullStringTest()
        {
            String[] errorHandlerTypeName = new[]
                                            {
                                                (String)null
                                            };

            new ErrorHandlerAttribute(errorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorStringArrayTest()
        {
            String[] errorHandlerTypeNames = new[]
                                             {
                                                 typeof(KnownErrorHandler).AssemblyQualifiedName, typeof(UnknownErrorHandler).AssemblyQualifiedName
                                             };
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(errorHandlerTypeNames);

            Assert.AreEqual(2, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(
                typeof(KnownErrorHandler).AssemblyQualifiedName, 
                target.ErrorHandlerTypes[0].AssemblyQualifiedName, 
                "AssemblyQualifiedName return an incorrect value");
            Assert.AreEqual(
                typeof(UnknownErrorHandler).AssemblyQualifiedName, 
                target.ErrorHandlerTypes[1].AssemblyQualifiedName, 
                "AssemblyQualifiedName return an incorrect value");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringEmptyStringTest()
        {
            String errorHandlerTypeName = String.Empty;

            new ErrorHandlerAttribute(errorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringNullStringTest()
        {
            const String ErrorHandlerTypeName = null;

            new ErrorHandlerAttribute(ErrorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorHandlerAttributeConstructorStringStringDuplicateTypeTest()
        {
            String firstErrorHandlerTypeName = typeof(KnownErrorHandler).AssemblyQualifiedName;
            String secondErrorHandlerTypeName = typeof(KnownErrorHandler).AssemblyQualifiedName;
            new ErrorHandlerAttribute(firstErrorHandlerTypeName, secondErrorHandlerTypeName);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringEmptyFirstStringTest()
        {
            new ErrorHandlerAttribute(String.Empty, "test");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringEmptySecondStringTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler).AssemblyQualifiedName, String.Empty);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringNullFirstStringTest()
        {
            new ErrorHandlerAttribute(null, "test");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringNullSecondStringTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler).AssemblyQualifiedName, null);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringStringEmptyFirstStringTest()
        {
            new ErrorHandlerAttribute(String.Empty, "test", "test");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringStringEmptySecondStringTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler).AssemblyQualifiedName, String.Empty, "test");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringStringEmptyThirdStringTest()
        {
            new ErrorHandlerAttribute(
                typeof(KnownErrorHandler).AssemblyQualifiedName, typeof(UnknownErrorHandler).AssemblyQualifiedName, String.Empty);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringStringNullFirstStringTest()
        {
            new ErrorHandlerAttribute(null, "test", "test");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringStringNullSecondStringTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler).AssemblyQualifiedName, null, "test");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorStringStringStringNullThirdStringTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler).AssemblyQualifiedName, typeof(UnknownErrorHandler).AssemblyQualifiedName, null);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorStringStringStringTest()
        {
            String firstErrorHandlerTypeName = typeof(KnownErrorHandler).AssemblyQualifiedName;
            String secondErrorHandlerTypeName = typeof(UnknownErrorHandler).AssemblyQualifiedName;
            String thirdErrorHandlerTypeName = typeof(AnotherErrorHandler).AssemblyQualifiedName;
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(firstErrorHandlerTypeName, secondErrorHandlerTypeName, thirdErrorHandlerTypeName);

            Assert.AreEqual(3, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(
                firstErrorHandlerTypeName, target.ErrorHandlerTypes[0].AssemblyQualifiedName, "AssemblyQualifiedName returned an incorrect value");
            Assert.AreEqual(
                secondErrorHandlerTypeName, target.ErrorHandlerTypes[1].AssemblyQualifiedName, "AssemblyQualifiedName returned an incorrect value");
            Assert.AreEqual(
                thirdErrorHandlerTypeName, target.ErrorHandlerTypes[2].AssemblyQualifiedName, "AssemblyQualifiedName returned an incorrect value");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorStringStringTest()
        {
            String firstErrorHandlerTypeName = typeof(KnownErrorHandler).AssemblyQualifiedName;
            String secondErrorHandlerTypeName = typeof(UnknownErrorHandler).AssemblyQualifiedName;
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(firstErrorHandlerTypeName, secondErrorHandlerTypeName);

            Assert.AreEqual(2, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(
                firstErrorHandlerTypeName, target.ErrorHandlerTypes[0].AssemblyQualifiedName, "AssemblyQualifiedName returned an incorrect value");
            Assert.AreEqual(
                secondErrorHandlerTypeName, target.ErrorHandlerTypes[1].AssemblyQualifiedName, "AssemblyQualifiedName returned an incorrect value");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorStringTest()
        {
            String errorHandlerTypeName = typeof(KnownErrorHandler).AssemblyQualifiedName;
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(errorHandlerTypeName);

            Assert.AreEqual(1, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(
                errorHandlerTypeName, target.ErrorHandlerTypes[0].AssemblyQualifiedName, "AssemblyQualifiedName returned an incorrect value");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ErrorHandlerAttributeConstructorTypeArrayEmptyTypeArrayTest()
        {
            Type[] errorHandlerTypes = new Type[0];

            new ErrorHandlerAttribute(errorHandlerTypes);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ErrorHandlerAttributeConstructorTypeArrayIncorrectTypeTest()
        {
            Type[] errorHandlerType = new[]
                                      {
                                          typeof(String)
                                      };

            new ErrorHandlerAttribute(errorHandlerType);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeArrayNullTypeArrayEntryTest()
        {
            Type[] errorHandlerTypes = new Type[]
                                       {
                                           null
                                       };

            new ErrorHandlerAttribute(errorHandlerTypes);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeArrayNullTypeArrayTest()
        {
            const Type[] ErrorHandlerTypes = null;

            new ErrorHandlerAttribute(ErrorHandlerTypes);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorTypeArrayTest()
        {
            Type[] errorHandlerTypes = new[]
                                       {
                                           typeof(KnownErrorHandler), typeof(UnknownErrorHandler)
                                       };
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(errorHandlerTypes);

            Assert.AreEqual(2, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(typeof(KnownErrorHandler), target.ErrorHandlerTypes[0], "Incorrect type returned");
            Assert.AreEqual(typeof(UnknownErrorHandler), target.ErrorHandlerTypes[1], "Incorrect type returned");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ErrorHandlerAttributeConstructorTypeIncorrectTypeTest()
        {
            Type errorHandlerType = typeof(String);

            new ErrorHandlerAttribute(errorHandlerType);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeNullTypeTest()
        {
            const Type ErrorHandlerType = null;

            new ErrorHandlerAttribute(ErrorHandlerType);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorTypeTest()
        {
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(typeof(KnownErrorHandler));

            Assert.AreEqual(1, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(typeof(KnownErrorHandler), target.ErrorHandlerTypes[0], "Incorrect type returned");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeTypeNullFirstTypeTest()
        {
            new ErrorHandlerAttribute(null, typeof(KnownErrorHandler));
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeTypeNullSecondTypeTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler), null);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorTypeTypeTest()
        {
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(typeof(KnownErrorHandler), typeof(UnknownErrorHandler));

            Assert.AreEqual(2, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(typeof(KnownErrorHandler), target.ErrorHandlerTypes[0], "Incorrect type returned");
            Assert.AreEqual(typeof(UnknownErrorHandler), target.ErrorHandlerTypes[1], "Incorrect type returned");
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeTypeTypeNullFirstTypeTest()
        {
            new ErrorHandlerAttribute(null, typeof(KnownErrorHandler), typeof(UnknownErrorHandler));
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeTypeTypeNullSecondTypeTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler), null, typeof(UnknownErrorHandler));
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorHandlerAttributeConstructorTypeTypeTypeNullThirdTypeTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler), typeof(UnknownErrorHandler), null);
        }

        /// <summary>
        /// A test for ErrorHandlerAttribute Constructor.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerAttributeConstructorTypeTypeTypeTest()
        {
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(
                typeof(KnownErrorHandler), typeof(UnknownErrorHandler), typeof(AnotherErrorHandler));

            Assert.AreEqual(3, target.ErrorHandlerTypes.Count, "Count returned an incorrect value");
            Assert.AreEqual(typeof(KnownErrorHandler), target.ErrorHandlerTypes[0], "Incorrect type returned");
            Assert.AreEqual(typeof(UnknownErrorHandler), target.ErrorHandlerTypes[1], "Incorrect type returned");
            Assert.AreEqual(typeof(AnotherErrorHandler), target.ErrorHandlerTypes[2], "Incorrect type returned");
        }

        /// <summary>
        /// Runs test for ignores duplicate error handler type.
        /// </summary>
        [TestMethod]
        public void IgnoresDuplicateErrorHandlerTypeTest()
        {
            new ErrorHandlerAttribute(typeof(KnownErrorHandler), typeof(KnownErrorHandler));
        }

        /// <summary>
        /// A test for Validate.
        /// </summary>
        [TestMethod]
        public void ValidateTest()
        {
            ErrorHandlerAttribute target = new ErrorHandlerAttribute(typeof(KnownErrorHandler));
            IServiceBehavior behaviour = target;

            // Validate currently has not implementation
            behaviour.Validate(null, null);
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