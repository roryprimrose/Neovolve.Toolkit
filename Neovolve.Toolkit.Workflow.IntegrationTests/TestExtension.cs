namespace Neovolve.Toolkit.Workflow.IntegrationTests
{
    using System;
    using System.Activities.Hosting;
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="TestExtension"/>
    ///   class provides an extension to test the <see cref="WorkflowExtensionElement"/> class.
    /// </summary>
    public class TestExtension : IWorkflowInstanceExtension
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "TestExtension" /> class.
        /// </summary>
        public TestExtension()
        {
            TestValue = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// When implemented, returns any additional extensions the implementing class requires.
        /// </summary>
        /// <returns>
        /// A collection of additional workflow extensions.
        /// </returns>
        public IEnumerable<Object> GetAdditionalExtensions()
        {
            return null;
        }

        /// <summary>
        /// Sets the specified target <see cref="T:System.Activities.Hosting.WorkflowInstanceProxy"/>.
        /// </summary>
        /// <param name="instance">
        /// The target workflow instance to set.
        /// </param>
        public void SetInstance(WorkflowInstanceProxy instance)
        {
            if (MockInstance == null)
            {
                throw new InvalidOperationException("Workflow extension requires mock instance to be provided.");
            }

            MockInstance.SetInstance(instance);
        }

        /// <summary>
        ///   Gets or sets the mock instance.
        /// </summary>
        /// <value>
        ///   The mock instance.
        /// </value>
        public static IWorkflowInstanceExtension MockInstance
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the test value.
        /// </summary>
        /// <value>
        ///   The test value.
        /// </value>
        public String TestValue
        {
            get;
            private set;
        }
    }
}