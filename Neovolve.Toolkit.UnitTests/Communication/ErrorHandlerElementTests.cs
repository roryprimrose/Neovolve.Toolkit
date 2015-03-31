namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ErrorHandlerElementTests"/>
    ///   class is used to test the <see cref="ErrorHandlerElement"/> class.
    /// </summary>
    [TestClass]
    public class ErrorHandlerElementTests
    {
        /// <summary>
        /// Runs test for error handler type returns behaviour type.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerTypeReturnsBehaviourTypeTest()
        {
            ErrorHandlerElement target = new ErrorHandlerElement();

            Assert.AreEqual(typeof(ErrorHandlerAttribute), target.BehaviorType, "BehaviorType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for error handler type returns stored value handler type.
        /// </summary>
        [TestMethod]
        public void ErrorHandlerTypeReturnsStoredValueHandlerTypeTest()
        {
            ErrorHandlerElement target = new ErrorHandlerElement();
            String expected = Guid.NewGuid().ToString();

            target.ErrorHandlerType = expected;

            Assert.AreEqual(expected, target.ErrorHandlerType, "ErrorHandlerType returned an incorrect value");
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