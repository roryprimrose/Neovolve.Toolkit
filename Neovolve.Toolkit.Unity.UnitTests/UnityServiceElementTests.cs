namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.ServiceModel.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityServiceElementTests"/>
    ///   class is used to test the <see cref="UnityServiceElement"/> class.
    /// </summary>
    [TestClass]
    public class UnityServiceElementTests
    {
        /// <summary>
        /// Runs test for unity service element behavior type returns expected type.
        /// </summary>
        [TestMethod]
        public void UnityServiceElementBehaviorTypeReturnsExpectedTypeTest()
        {
            UnityServiceElement target = new UnityServiceElement();

            Type actual = target.BehaviorType;

            Assert.AreEqual(typeof(UnityServiceBehavior), actual, "BehaviorType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service element copy from copies configuration parameters.
        /// </summary>
        [TestMethod]
        public void UnityServiceElementCopyFromCopiesConfigurationParametersTest()
        {
            UnityServiceElement source = new UnityServiceElement
                                         {
                                             UnitySectionName = Guid.NewGuid().ToString(), 
                                             UnityContainerName = Guid.NewGuid().ToString(), 
                                             UnityResolveName = Guid.NewGuid().ToString()
                                         };
            UnityServiceElement target = new UnityServiceElement();

            target.CopyFrom(source);

            Assert.AreEqual(source.UnitySectionName, target.UnitySectionName, "UnitySectionName returned an incorrect value");
            Assert.AreEqual(source.UnityContainerName, target.UnityContainerName, "UnityContainerName returned an incorrect value");
            Assert.AreEqual(source.UnityResolveName, target.UnityResolveName, "UnityResolveName returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity service element copy from returns instance for incorrect element type.
        /// </summary>
        [TestMethod]
        public void UnityServiceElementCopyFromReturnsInstanceForIncorrectElementTypeTest()
        {
            ServiceModelExtensionElement source = MockRepository.GenerateStub<ServiceModelExtensionElement>();
            UnityServiceElement target = new UnityServiceElement();

            target.CopyFrom(source);

            Assert.IsTrue(String.IsNullOrWhiteSpace(target.UnitySectionName), "UnitySectionName returned an incorrect value");
            Assert.IsTrue(String.IsNullOrWhiteSpace(target.UnityContainerName), "UnityContainerName returned an incorrect value");
            Assert.IsTrue(String.IsNullOrWhiteSpace(target.UnityResolveName), "UnityResolveName returned an incorrect value");
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