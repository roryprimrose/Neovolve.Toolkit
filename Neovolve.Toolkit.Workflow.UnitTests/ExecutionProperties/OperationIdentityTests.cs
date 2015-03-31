namespace Neovolve.Toolkit.Workflow.UnitTests.ExecutionProperties
{
    using System.Security.Principal;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.ExecutionProperties;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="OperationIdentityTests"/>
    ///   class is used to test the <see cref="OperationIdentity"/> class.
    /// </summary>
    [TestClass]
    public class OperationIdentityTests
    {
        /// <summary>
        /// Runs test for on receive has no affect when context is null.
        /// </summary>
        [TestMethod]
        public void OnReceiveHasNoAffectWhenContextIsNullTest()
        {
            OperationIdentity target = new OperationIdentity();

            target.OnReceiveMessage(null, null);

            IIdentity actual = target.Identity;

            Assert.IsNull(actual, "Identity returned an incorrect value");
        }

        ///<summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}