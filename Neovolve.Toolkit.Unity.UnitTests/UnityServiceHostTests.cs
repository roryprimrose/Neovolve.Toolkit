namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityServiceHostTests"/>
    ///   class is used to test the <see cref="UnityServiceHost"/> class.
    /// </summary>
    [TestClass]
    public class UnityServiceHostTests
    {
        /// <summary>
        /// Runs test for unity service host creates channel with unity behaviour.
        /// </summary>
        [TestMethod]
        public void UnityServiceHostCreatesChannelWithUnityBehaviorTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            using (UnityServiceHost target = new UnityServiceHost(typeof(TestService), container))
            {
                target.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), "net.pipe://localhost/TestService/Test.svc");

                target.Open();

                UnityServiceBehavior behavior = target.Description.Behaviors.Find<UnityServiceBehavior>();

                target.Close();

                Assert.IsNotNull(behavior, "Behaviour does not contain UnityServiceBehavior");
            }
        }

        /// <summary>
        /// Runs test for unity service host throws exception when created with null container.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityServiceHostThrowsExceptionWhenCreatedWithNullContainerTest()
        {
            using (new UnityServiceHost(typeof(TestService), null))
            {
            }
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