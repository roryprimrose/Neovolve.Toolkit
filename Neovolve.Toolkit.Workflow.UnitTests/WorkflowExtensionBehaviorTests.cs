namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;
    using System.Activities;
    using System.ServiceModel;
    using System.ServiceModel.Activities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Extensions;
    using Neovolve.Toolkit.Workflow.UnitTests.Activities;

    /// <summary>
    /// The <see cref="WorkflowExtensionBehaviorTests"/>
    ///   class is used to test the <see cref="WorkflowExtensionBehavior"/> class.
    /// </summary>
    [TestClass]
    public class WorkflowExtensionBehaviorTests
    {
        /// <summary>
        /// Runs test for workflow extension behavior apply dispatch behavior attaches provided instance extension type.
        /// </summary>
        [TestMethod]
        public void WorkflowExtensionBehaviorApplyDispatchBehaviorAttachesProvidedInstanceExtensionTypeTest()
        {
            WorkflowExtensionBehavior target = new WorkflowExtensionBehavior(typeof(InstanceManagerExtension), false);
            Activity activity = new AbortActivity();

            using (WorkflowServiceHost serviceHost = new WorkflowServiceHost(activity))
            {
                target.ApplyDispatchBehavior(null, serviceHost);
            }
        }

        /// <summary>
        /// Runs test for workflow extension behavior apply dispatch behavior attaches provided extension.
        /// </summary>
        [TestMethod]
        public void WorkflowExtensionBehaviorApplyDispatchBehaviorAttachesProvidedSingletonExtensionTypeTest()
        {
            WorkflowExtensionBehavior target = new WorkflowExtensionBehavior(typeof(DateTime), true);
            Activity activity = new AbortActivity();

            using (WorkflowServiceHost serviceHost = new WorkflowServiceHost(activity))
            {
                target.ApplyDispatchBehavior(null, serviceHost);
            }
        }

        /// <summary>
        /// Runs test for workflow extension behavior apply dispatch behavior throws exception for unsupported service host.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WorkflowExtensionBehaviorApplyDispatchBehaviorThrowsExceptionForUnsupportedServiceHostTest()
        {
            WorkflowExtensionBehavior target = new WorkflowExtensionBehavior(typeof(String), true);
            Activity activity = new AbortActivity();

            using (ServiceHost serviceHost = new ServiceHost(activity))
            {
                target.ApplyDispatchBehavior(null, serviceHost);
            }
        }

        /// <summary>
        /// Runs test for workflow extension behavior throws exception when created with null extension type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WorkflowExtensionBehaviorThrowsExceptionWhenCreatedWithNullExtensionTypeTest()
        {
            new WorkflowExtensionBehavior(null, true);
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