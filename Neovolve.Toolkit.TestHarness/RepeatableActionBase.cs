namespace Neovolve.Toolkit.TestHarness
{
    using System;

    /// <summary>
    /// The <see cref="RepeatableActionBase{T}"/>
    ///   class is an abstract class that provides the common logic for invoking repeatable tasks.
    /// </summary>
    /// <typeparam name="T">
    /// The context type used to invoke the action.
    /// </typeparam>
    internal abstract class RepeatableActionBase<T> : IRepeatableAction where T : class
    {
        /// <summary>
        ///   Occurs when an iteration is completed.
        /// </summary>
        public event EventHandler<IterationEventArgs> IterationCompleted;

        /// <summary>
        /// Invokes the specified iterations.
        /// </summary>
        /// <param name="iterations">
        /// The iterations.
        /// </param>
        public void Invoke(Int32 iterations)
        {
            InvokeRecursive(iterations, 0);
        }

        /// <summary>
        /// Invokes the recursive.
        /// </summary>
        /// <param name="iterations">
        /// The iterations.
        /// </param>
        /// <param name="recursionDepth">
        /// The recursion depth.
        /// </param>
        public void InvokeRecursive(Int32 iterations, Int32 recursionDepth)
        {
            for (Int32 index = 0; index < iterations; index++)
            {
                RunInvokeRecursive(recursionDepth, 0);

                IterationEventArgs args = new IterationEventArgs(index + 1, iterations + 1);

                OnIterationCompleted(args);
            }
        }

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
        protected virtual void CompleteAction(Int32 recursionDepth, Int32 recursionIndex, T context)
        {
            RunInvokeRecursive(recursionDepth, recursionIndex + 1);
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
        protected abstract void InvokeAction(Int32 recursionDepth, Int32 recursionIndex, T context);

        /// <summary>
        /// Raises the <see cref="IterationCompleted"/> event.
        /// </summary>
        /// <param name="args">
        /// The <see cref="Neovolve.Toolkit.TestHarness.IterationEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnIterationCompleted(IterationEventArgs args)
        {
            if (IterationCompleted != null)
            {
                IterationCompleted(this, args);
            }
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
        /// A <typeparamref name="T"/> instance or <c>null</c>.
        /// </returns>
        protected virtual T PrepareAction(Int32 recursionDepth, Int32 recursionIndex)
        {
            return null;
        }

        /// <summary>
        /// Runs the invoke recursive.
        /// </summary>
        /// <param name="recursionDepth">
        /// The recursion depth.
        /// </param>
        /// <param name="recursionIndex">
        /// Index of the recursion.
        /// </param>
        private void RunInvokeRecursive(Int32 recursionDepth, Int32 recursionIndex)
        {
            if (recursionIndex >= recursionDepth)
            {
                return;
            }

            T context = PrepareAction(recursionDepth, recursionIndex);

            InvokeAction(recursionDepth, recursionIndex, context);

            CompleteAction(recursionDepth, recursionIndex, context);
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is recursive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recursive; otherwise, <c>false</c>.
        /// </value>
        public abstract Boolean IsRecursive
        {
            get;
        }
    }
}