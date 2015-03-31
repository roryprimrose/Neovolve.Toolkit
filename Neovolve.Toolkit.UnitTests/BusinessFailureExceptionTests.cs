namespace Neovolve.Toolkit.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="BusinessFailureExceptionTests"/>
    ///   class is used to test the <see cref="BusinessFailureException{T}"/> class.
    /// </summary>
    [TestClass]
    public class BusinessFailureExceptionTests
    {
        /// <summary>
        /// Runs test for business failure exception can be created.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeCreatedTest()
        {
            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>();

            Assert.IsFalse(String.IsNullOrEmpty(target.Message), "Message returned an incorrect value");
        }

        /// <summary>
        /// Runs test for business failure exception can be created with business failure details.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeCreatedWithBusinessFailureDetailsTest()
        {
            BusinessFailure<Int32> originalFailure = new BusinessFailure<Int32>(234, Guid.NewGuid().ToString());
            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(originalFailure.Code, originalFailure.Description);

            Assert.AreEqual(1, target.Failures.Count(), "Failures returned an incorrect number of items");
            Assert.AreEqual(originalFailure.Code, target.Failures.First().Code, "Code returned an incorrect value");
            Assert.AreEqual(originalFailure.Description, target.Failures.First().Description, "Description returned an incorrect value");
        }

        /// <summary>
        /// Runs test for business failure exception can be created with business failure.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeCreatedWithBusinessFailureTest()
        {
            const Int32 Code = 234;
            BusinessFailure<Int32> originalFailure = new BusinessFailure<Int32>(Code, Guid.NewGuid().ToString());
            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(originalFailure);

            Assert.AreEqual(1, target.Failures.Count(), "Failures returned an incorrect number of items");
            Assert.AreEqual(originalFailure.Code, target.Failures.First().Code, "Code returned an incorrect value");
            Assert.AreEqual(originalFailure.Description, target.Failures.First().Description, "Description returned an incorrect value");
        }

        /// <summary>
        /// Runs test for business failure exception can be created with list of business failures.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeCreatedWithListOfBusinessFailuresTest()
        {
            List<BusinessFailure<Int32>> failures = new List<BusinessFailure<Int32>>(3);

            failures.Add(new BusinessFailure<Int32>(12, Guid.NewGuid().ToString()));
            failures.Add(new BusinessFailure<Int32>(245, Guid.NewGuid().ToString()));
            failures.Add(new BusinessFailure<Int32>(3113, Guid.NewGuid().ToString()));

            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(failures);

            AssertFailuresMatch(target, failures);
        }

        /// <summary>
        /// Runs test for business failure exception can be created with message and inner exception.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeCreatedWithMessageAndInnerExceptionTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();
            String innerExceptionMessage = Guid.NewGuid().ToString();

            TypeLoadException innerException = new TypeLoadException(innerExceptionMessage);
            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(exceptionMessage, innerException);

            Assert.IsTrue(target.Message.Contains(exceptionMessage), "Message returned an incorrect value");
            Assert.AreEqual(target.InnerException, innerException, "InnerException returned an incorrect value");
            Assert.AreEqual(target.InnerException.Message, innerExceptionMessage, "InnerException.Message returned an incorrect value");
        }

        /// <summary>
        /// Runs test for business failure exception can be created with message.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeCreatedWithMessageTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();

            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(exceptionMessage);

            Assert.IsTrue(target.Message.Contains(exceptionMessage), "Message returned an incorrect value");
        }

        /// <summary>
        /// Runs test for business failure exception can be serialized and de-serialized.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionCanBeSerializedAndDeserializedTest()
        {
            BusinessFailure<Int32> failure = new BusinessFailure<Int32>(4564555, Guid.NewGuid().ToString());
            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(failure);
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, target);
                ms.Seek(0, SeekOrigin.Begin);

                BusinessFailureException<Int32> outputException = formatter.Deserialize(ms) as BusinessFailureException<Int32>;

                Assert.IsNotNull(outputException, "Failed to serialize and de-serialize the exception.");
                Assert.AreEqual((Object)outputException.Message, target.Message, "Message returned an incorrect value");

                AssertFailuresMatch(target, outputException.Failures);
            }
        }

        /// <summary>
        /// Runs test for business failure exception returns failures details in message.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionReturnsFailuresDetailsInMessageTest()
        {
            List<BusinessFailure<Int32>> failures = new List<BusinessFailure<Int32>>(3);

            failures.Add(new BusinessFailure<Int32>(234, Guid.NewGuid().ToString()));
            failures.Add(new BusinessFailure<Int32>(789789, Guid.NewGuid().ToString()));
            failures.Add(new BusinessFailure<Int32>(12, Guid.NewGuid().ToString()));

            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(failures);

            String actual = target.Message;

            foreach (BusinessFailure<Int32> failure in target.Failures)
            {
                Assert.IsTrue(actual.Contains(failure.Code.ToString(CultureInfo.InvariantCulture)), "Message does not contain failure code");
                Assert.IsTrue(actual.Contains(failure.Description), "Description does not contain failure message");
            }
        }

        /// <summary>
        /// Runs test for business failure exception returns message when created with no message.
        /// </summary>
        [TestMethod]
        public void BusinessFailureExceptionReturnsMessageWhenCreatedWithNoMessageTest()
        {
            BusinessFailureException<Int32> target = new BusinessFailureException<Int32>(String.Empty);

            String actual = target.Message;

            Assert.IsFalse(String.IsNullOrEmpty(actual), "Message failed to return a value");
        }

        /// <summary>
        /// Runs test for business failure exception throws exception with empty description.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessFailureExceptionThrowsExceptionWithEmptyDescriptionTest()
        {
            new BusinessFailureException<Int32>(0, String.Empty);
        }

        /// <summary>
        /// Runs test for business failure exception throws exception with null description.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessFailureExceptionThrowsExceptionWithNullDescriptionTest()
        {
            new BusinessFailureException<Int32>(0, null);
        }

        /// <summary>
        /// Runs test for business failure exception throws exception with null failure.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessFailureExceptionThrowsExceptionWithNullFailureTest()
        {
            new BusinessFailureException<Int32>((BusinessFailure<Int32>)null);
        }

        /// <summary>
        /// Runs test for business failure exception throws exception with null failures.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessFailureExceptionThrowsExceptionWithNullFailuresTest()
        {
            new BusinessFailureException<Int32>((IEnumerable<BusinessFailure<Int32>>)null);
        }

        /// <summary>
        /// Runs test for business failure exception throws exception with white space description.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessFailureExceptionThrowsExceptionWithWhiteSpaceDescriptionTest()
        {
            new BusinessFailureException<Int32>(0, " ");
        }

        #region Static Helper Methods

        /// <summary>
        /// Asserts the failures match.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="expectedFailures">
        /// The expected failures.
        /// </param>
        private static void AssertFailuresMatch(BusinessFailureException<Int32> target, IEnumerable<BusinessFailure<Int32>> expectedFailures)
        {
            Assert.AreEqual(expectedFailures.Count(), target.Failures.Count(), "Failures returned an incorrect number of items");

            foreach (BusinessFailure<Int32> failure in target.Failures)
            {
                BusinessFailure<Int32> businessFailure = failure;

                BusinessFailure<Int32> originalFailure =
                    expectedFailures.Where(x => x.Code == businessFailure.Code && x.Description == businessFailure.Description).SingleOrDefault();

                Assert.IsNotNull(originalFailure, "No match found on failure");
            }
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