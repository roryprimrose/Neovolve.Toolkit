namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// This is a test class for TraceSourceLoadExceptionTests and is intended
    ///   to contain all TraceSourceLoadExceptionTests Unit Tests.
    /// </summary>
    [TestClass]
    public class TraceSourceLoadExceptionTests
    {
        /// <summary>
        /// Runs test for can create with message and inner exception.
        /// </summary>
        [TestMethod]
        public void CanCreateWithMessageAndInnerExceptionTest()
        {
            String message = Guid.NewGuid().ToString();
            Exception inner = new NotSupportedException();

            TraceSourceLoadException target = new TraceSourceLoadException(message, inner);

            Assert.AreEqual(message, target.Message, "Message returned an incorrect value");
            Assert.AreEqual(inner, target.InnerException, "InnerException returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can create with message.
        /// </summary>
        [TestMethod]
        public void CanCreateWithMessageTest()
        {
            String message = Guid.NewGuid().ToString();
            TraceSourceLoadException target = new TraceSourceLoadException(message);

            Assert.AreEqual(message, target.Message, "Message returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can deserialize exception.
        /// </summary>
        [TestMethod]
        public void CanDeserializeExceptionTest()
        {
            String message = Guid.NewGuid().ToString();
            TraceSourceLoadException target = new TraceSourceLoadException(message);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, target);
                ms.Seek(0, SeekOrigin.Begin);
                TraceSourceLoadException outputException = formatter.Deserialize(ms) as TraceSourceLoadException;

                Assert.IsNotNull(outputException, "Failed to serialize and deserialize the exception.");

                Assert.AreEqual(message, target.Message, "Message returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for create with no message returns default message.
        /// </summary>
        [TestMethod]
        public void CreateWithNoMessageReturnsDefaultMessageTest()
        {
            TraceSourceLoadException target = new TraceSourceLoadException();

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