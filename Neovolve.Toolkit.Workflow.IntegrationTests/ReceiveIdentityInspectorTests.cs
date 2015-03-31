namespace Neovolve.Toolkit.Workflow.IntegrationTests
{
    using System;
    using System.Activities.Hosting;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Activities;
    using System.Xaml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;
    using Neovolve.Toolkit.Workflow.Activities;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ReceiveIdentityInspectorTests"/>
    ///   class is used to test the <see cref="ReceiveIdentityInspector{T}"/> class.
    /// </summary>
    [TestClass]
    public class ReceiveIdentityInspectorTests
    {
        /// <summary>
        /// Runs test for receive identity inspector extracts identity.
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestServiceIdentity.xamlx")]
        public void ReceiveIdentityInspectorExtractsIdentityTest()
        {
            WorkflowService service = (WorkflowService)XamlServices.Load("TestServiceIdentity.xamlx");
            IWorkflowInstanceExtension extension = MockRepository.GenerateStub<IWorkflowInstanceExtension>();

            TestExtension.MockInstance = extension;

            using (WorkflowServiceHost testHost = new WorkflowServiceHost(service, new Uri("net.pipe://localhost/TestServiceIdentity")))
            {
                testHost.Open();

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("Identity")))
                {
                    String actual = manager.Proxy.GetData(Guid.NewGuid().ToString());

                    String expected = WindowsIdentity.GetCurrent().Name;

                    Assert.AreEqual(expected, actual, "Service failed to extract calling identity name");
                }
            }
        }

        /// <summary>
        /// Runs test for receive identity inspector throws exception for invalid identity type.
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestServiceInvalidIdentity.xamlx")]
        [ExpectedException(typeof(FaultException))]
        public void ReceiveIdentityInspectorThrowsExceptionForInvalidIdentityTypeTest()
        {
            WorkflowService service = (WorkflowService)XamlServices.Load("TestServiceInvalidIdentity.xamlx");
            IWorkflowInstanceExtension extension = MockRepository.GenerateStub<IWorkflowInstanceExtension>();

            TestExtension.MockInstance = extension;

            using (WorkflowServiceHost testHost = new WorkflowServiceHost(service, new Uri("net.pipe://localhost/TestServiceInvalidIdentity")))
            {
                testHost.Open();

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("InvalidIdentity")))
                {
                    manager.Proxy.GetData(Guid.NewGuid().ToString());
                }
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