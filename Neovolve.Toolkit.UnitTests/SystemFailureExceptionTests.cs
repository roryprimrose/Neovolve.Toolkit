namespace Neovolve.Toolkit.UnitTests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="SystemFailureExceptionTests"/>
    ///   class is used to test the <see cref="SystemFailureException"/> class.
    /// </summary>
    [TestClass]
    public class SystemFailureExceptionTests
    {
        /// <summary>
        /// A test for SystemFailureException(string, Exception).
        /// </summary>
        [TestMethod]
        public void ConstructorMessageExceptionTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();
            String innerExceptionMessage = Guid.NewGuid().ToString();

            TypeLoadException innerException = new TypeLoadException(innerExceptionMessage);
            SystemFailureException target = new SystemFailureException(exceptionMessage, innerException);

            Assert.AreEqual(target.Message, exceptionMessage, "Message returned an incorrect value");
            Assert.AreEqual(target.InnerException, innerException, "InnerException returned an incorrect value");
            Assert.AreEqual(target.InnerException.Message, innerExceptionMessage, "InnerException.Message returned an incorrect value");
        }

        /// <summary>
        /// A test for SystemFailureException(string).
        /// </summary>
        [TestMethod]
        public void ConstructorMessageTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();

            SystemFailureException target = new SystemFailureException(exceptionMessage);

            Assert.AreEqual(target.Message, exceptionMessage, "Message returned an incorrect value");
        }

        /// <summary>
        /// A test for SystemFailureException (SerializationInfo, StreamingContext).
        /// </summary>
        [TestMethod]
        public void ConstructorSerializationTest()
        {
            String exceptionMessage = Guid.NewGuid().ToString();
            String innerExceptionMessage = Guid.NewGuid().ToString();
            TypeLoadException innerException = new TypeLoadException(innerExceptionMessage);
            SystemFailureException target = new SystemFailureException(exceptionMessage, innerException);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(ms, target);
                ms.Seek(0, SeekOrigin.Begin);

                SystemFailureException outputException = formatter.Deserialize(ms) as SystemFailureException;

                Assert.IsNotNull(outputException, "Failed to serialize and deserialize the exception.");
                Assert.IsNotNull(outputException.InnerException, "InnerException failed to return a value");
                Assert.AreEqual(outputException.Message, exceptionMessage, "Message returned an incorrect value");
                Assert.AreEqual(outputException.InnerException.Message, innerExceptionMessage, "InnerException.Message returned an incorrect value");
            }
        }

        /// <summary>
        /// A test for SystemFailureException().
        /// </summary>
        [TestMethod]
        public void ConstructorTest()
        {
            SystemFailureException target = new SystemFailureException();

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