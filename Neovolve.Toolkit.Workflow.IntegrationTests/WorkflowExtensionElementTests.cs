namespace Neovolve.Toolkit.Workflow.IntegrationTests
{
    using System;
    using System.Activities.Hosting;
    using System.Configuration;
    using System.Diagnostics;
    using System.ServiceModel.Activities;
    using System.Xaml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="WorkflowExtensionElementTests"/>
    ///   class is used to test the <see cref="WorkflowExtensionElement"/> class.
    /// </summary>
    [TestClass]
    public class WorkflowExtensionElementTests
    {
        /// <summary>
        /// Runs test for workflow extension element adds extension to workflow service host.
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestServiceSingleton.xamlx")]
        public void WorkflowExtensionElementAddsExtensionToWorkflowServiceHostTest()
        {
            WorkflowService service = (WorkflowService)XamlServices.Load("TestServiceSingleton.xamlx");
            IWorkflowInstanceExtension extension = MockRepository.GenerateStub<IWorkflowInstanceExtension>();

            TestExtension.MockInstance = extension;

            using (WorkflowServiceHost testHost = new WorkflowServiceHost(service, new Uri("net.pipe://localhost/TestServiceSingleton")))
            {
                testHost.Open();

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("Singleton")))
                {
                    manager.Proxy.GetData(Guid.NewGuid().ToString());
                }
            }

            extension.AssertWasCalled(x => x.SetInstance(null), opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// Runs test for workflow extension element adds instance extension to workflow service host.
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestServiceInstance.xamlx")]
        public void WorkflowExtensionElementAddsInstanceExtensionToWorkflowServiceHostTest()
        {
            WorkflowService service = (WorkflowService)XamlServices.Load("TestServiceInstance.xamlx");
            IWorkflowInstanceExtension extension = MockRepository.GenerateStub<IWorkflowInstanceExtension>();

            TestExtension.MockInstance = extension;

            String firstResponse;
            String secondResponse;

            using (WorkflowServiceHost testHost = new WorkflowServiceHost(service, new Uri("net.pipe://localhost/TestServiceInstance")))
            {
                try
                {
                    testHost.Open();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());

                    throw;
                }

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("Instance")))
                {
                    firstResponse = manager.Proxy.GetData(Guid.NewGuid().ToString());
                }

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("Instance")))
                {
                    secondResponse = manager.Proxy.GetData(Guid.NewGuid().ToString());
                }
            }

            Assert.AreNotEqual(firstResponse, secondResponse, "Extension was not added to the host as an individual instance");
        }

        /// <summary>
        /// Runs test for workflow extension element adds singleton extension to workflow service host.
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestServiceSingleton.xamlx")]
        public void WorkflowExtensionElementAddsSingletonExtensionToWorkflowServiceHostTest()
        {
            WorkflowService service = (WorkflowService)XamlServices.Load("TestServiceSingleton.xamlx");
            IWorkflowInstanceExtension extension = MockRepository.GenerateStub<IWorkflowInstanceExtension>();

            TestExtension.MockInstance = extension;

            String firstResponse;
            String secondResponse;

            using (WorkflowServiceHost testHost = new WorkflowServiceHost(service, new Uri("net.pipe://localhost/TestServiceSingleton")))
            {
                testHost.Open();

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("Singleton")))
                {
                    firstResponse = manager.Proxy.GetData(Guid.NewGuid().ToString());
                }

                using (ProxyManager<ITestService> manager = new ProxyManager<ITestService>(new ChannelProxyHandler<ITestService>("Singleton")))
                {
                    secondResponse = manager.Proxy.GetData(Guid.NewGuid().ToString());
                }
            }

            Assert.AreEqual(firstResponse, secondResponse, "Extension was not added to the host as a singleton");
        }

        /// <summary>
        /// Runs test for workflow extension element throws exception for unknown extension type.
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestServiceFailure.xamlx")]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WorkflowExtensionElementThrowsExceptionForUnknownExtensionTypeTest()
        {
            WorkflowService service = (WorkflowService)XamlServices.Load("TestServiceFailure.xamlx");

            using (WorkflowServiceHost testHost = new WorkflowServiceHost(service, new Uri("net.pipe://localhost/TestServiceFailure")))
            {
                testHost.Open();
            }
        }
    }
}