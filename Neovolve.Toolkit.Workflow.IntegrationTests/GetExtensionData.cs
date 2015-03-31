namespace Neovolve.Toolkit.Workflow.IntegrationTests
{
    using System;
    using System.Activities;

    /// <summary>
    /// The <see cref="GetExtensionData"/>
    ///   class is used to extract a value from an extension and return it.
    /// </summary>
    public sealed class GetExtensionData : CodeActivity
    {
        /// <summary>
        /// Performs the execution of the activity.
        /// </summary>
        /// <param name="context">
        /// The execution context under which the activity executes.
        /// </param>
        protected override void Execute(CodeActivityContext context)
        {
            TestExtension testExtension = context.GetExtension<TestExtension>();

            context.SetValue(TestValue, testExtension.TestValue);
        }

        /// <summary>
        ///   Gets or sets the test value.
        /// </summary>
        /// <value>
        ///   The test value.
        /// </value>
        public OutArgument<String> TestValue
        {
            get;
            set;
        }
    }
}