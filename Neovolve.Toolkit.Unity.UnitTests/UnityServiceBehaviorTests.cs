namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityServiceBehaviorTests"/>
    ///   class is used to test the <see cref="UnityServiceBehavior"/> class.
    /// </summary>
    [TestClass]
    public class UnityServiceBehaviorTests
    {
        /// <summary>
        /// Runs test for unity service behavior apply dispatch behavior throws exception with null service description.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityServiceBehaviorApplyDispatchBehaviorThrowsExceptionWithNullServiceDescriptionTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            UnityServiceBehavior target = new UnityServiceBehavior(container);

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                target.ApplyDispatchBehavior(null, host);
            }
        }

        /// <summary>
        /// Runs test for unity service behavior apply dispatch behavior throws exception with null service host.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityServiceBehaviorApplyDispatchBehaviorThrowsExceptionWithNullServiceHostTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            UnityServiceBehavior target = new UnityServiceBehavior(container);
            ServiceDescription description = new ServiceDescription();

            target.ApplyDispatchBehavior(description, null);
        }

        /// <summary>
        /// Runs test for unity service behaviour assigns unity instance provider to endpoint.
        /// </summary>
        [TestMethod]
        public void UnityServiceBehaviorAssignsUnityInstanceProviderToEndpointTest()
        {
            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
                UnityServiceBehavior target = new UnityServiceBehavior(container);

                host.Description.Behaviors.Add(target);

                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), "net.pipe://localhost/TestService/Test.svc");

                host.Open();

                ChannelDispatcher dispatcher = (ChannelDispatcher)host.ChannelDispatchers.First();

                IInstanceProvider provider = dispatcher.Endpoints.First().DispatchRuntime.InstanceProvider;

                host.Close();

                Assert.IsInstanceOfType(provider, typeof(UnityInstanceProvider));
            }
        }

        /// <summary>
        /// Unities the service behavior can be created with null resolve name test.
        /// </summary>
        [TestMethod]
        public void UnityServiceBehaviorCanBeCreatedWithNullResolveNameTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            new UnityServiceBehavior(container, null);
        }

        /// <summary>
        /// Runs test for unity service behaviour throws exception when created with null container.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityServiceBehaviorThrowsExceptionWhenCreatedWithNullContainerTest()
        {
            new UnityServiceBehavior(null);
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