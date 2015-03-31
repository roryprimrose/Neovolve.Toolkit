namespace Neovolve.Toolkit.TestHarness
{
    using System;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="KnownSourceActivityTraceAction"/>
    ///   class invokes a <see cref="ActivityTrace"/> action.
    /// </summary>
    internal class KnownSourceActivityTraceAction : RepeatableActionBase<ActivityTrace>
    {
        /// <summary>
        ///   Defines the test message used to write to <see cref = "ActivityTrace" /> events.
        /// </summary>
        private const String TestMessage = "Test Message";

        /// <summary>
        ///   Defines the resolver to use for the tests.
        /// </summary>
        private static readonly TestTraceSourceResolver _resolver = new TestTraceSourceResolver();

        /// <summary>
        /// Completes the action.
        /// </summary>
        /// <param name="recursionDepth">
        /// The recursion depth.
        /// </param>
        /// <param name="recursionIndex">
        /// Index of the recursion.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void CompleteAction(Int32 recursionDepth, Int32 recursionIndex, ActivityTrace context)
        {
            // Avoid the recursive call here as it is already processed in InvokeAction
            return;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="recursionDepth">
        /// The recursion depth.
        /// </param>
        /// <param name="recursionIndex">
        /// Index of the recursion.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="context"/> value is <c>null</c>.
        /// </exception>
        protected override void InvokeAction(Int32 recursionDepth, Int32 recursionIndex, ActivityTrace context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.StartActivity(TestMessage);
            context.Write(RecordType.Verbose, TestMessage);
            context.Write(RecordType.Information, TestMessage);
            context.Write(RecordType.Warning, TestMessage);

            // Run the recursion
            base.CompleteAction(recursionDepth, recursionIndex, context);

            context.Write(RecordType.Error, TestMessage);
            context.Write(RecordType.Critical, TestMessage);
            context.StopActivity(TestMessage);
        }

        /// <summary>
        /// Prepares the action.
        /// </summary>
        /// <param name="recursionDepth">
        /// The recursion depth.
        /// </param>
        /// <param name="recursionIndex">
        /// Index of the recursion.
        /// </param>
        /// <returns>
        /// A <see cref="ActivityTrace"/> instance.
        /// </returns>
        protected override ActivityTrace PrepareAction(Int32 recursionDepth, Int32 recursionIndex)
        {
            return new ActivityTrace(TestTraceSourceResolver.SourceName, _resolver);
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is recursive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recursive; otherwise, <c>false</c>.
        /// </value>
        public override Boolean IsRecursive
        {
            get
            {
                return true;
            }
        }
    }
}