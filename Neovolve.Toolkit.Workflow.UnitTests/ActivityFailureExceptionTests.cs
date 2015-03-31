namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="ActivityFailureExceptionTests"/>
    ///   class is used to test the <see cref="ActivityFailureException"/> class.
    /// </summary>
    [TestClass]
    public class ActivityFailureExceptionTests
    {
        /// <summary>
        /// A test for ActivityFailureException(string, Exception).
        /// </summary>
        [TestMethod]
        public void ConstructorMessageExceptionTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();
            String innerExceptionMessage = Guid.NewGuid().ToString();

            TypeLoadException innerException = new TypeLoadException(innerExceptionMessage);
            ActivityFailureException target = new ActivityFailureException(exceptionMessage, innerException);

            Assert.AreEqual(target.Message, exceptionMessage, "Message returned an incorrect value");
            Assert.AreEqual(target.InnerException, innerException, "InnerException returned an incorrect value");
            Assert.AreEqual(target.InnerException.Message, innerExceptionMessage, "InnerException.Message returned an incorrect value");
        }

        /// <summary>
        /// A test for ActivityFailureException(string).
        /// </summary>
        [TestMethod]
        public void ConstructorMessageTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();

            ActivityFailureException target = new ActivityFailureException(exceptionMessage);

            Assert.AreEqual(target.Message, exceptionMessage, "Message returned an incorrect value");
        }

        /// <summary>
        /// A test for ActivityFailureException (SerializationInfo, StreamingContext).
        /// </summary>
        [TestMethod]
        public void ConstructorSerializationTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();
            String innerExceptionMessage = Guid.NewGuid().ToString();
            TypeLoadException innerException = new TypeLoadException(innerExceptionMessage);
            ActivityFailureException target = new ActivityFailureException(exceptionMessage, innerException);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(ms, target);
                ms.Seek(0, SeekOrigin.Begin);

                ActivityFailureException outputException = formatter.Deserialize(ms) as ActivityFailureException;

                Assert.IsNotNull(outputException, "Failed to serialize and deserialize the exception.");
                Assert.IsNotNull(outputException.InnerException, "InnerException failed to return a value");
                Assert.AreEqual(outputException.Message, exceptionMessage, "Message returned an incorrect value");
                Assert.AreEqual(outputException.InnerException.Message, innerExceptionMessage, "InnerException.Message returned an incorrect value");
            }
        }

        /// <summary>
        /// A test for ActivityFailureException().
        /// </summary>
        [TestMethod]
        public void ConstructorTest()
        {
            ActivityFailureException target = new ActivityFailureException();

            Assert.IsFalse(String.IsNullOrEmpty(target.Message), "Message returned an incorrect value");
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