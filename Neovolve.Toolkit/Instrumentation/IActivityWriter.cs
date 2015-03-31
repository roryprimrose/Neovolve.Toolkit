namespace Neovolve.Toolkit.Instrumentation
{
    using System;

    /// <summary>
    /// The <see cref="IActivityWriter"/>
    ///   interface is used to define how instrumentation records are written.
    /// </summary>
    public interface IActivityWriter : IRecordWriter
    {
        /// <summary>
        /// Resumes the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        void ResumeActivity(String message, params Object[] arguments);

        /// <summary>
        /// Starts the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        void StartActivity(String message, params Object[] arguments);

        /// <summary>
        /// Stops the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        void StopActivity(String message, params Object[] arguments);

        /// <summary>
        /// Suspends the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        void SuspendActivity(String message, params Object[] arguments);

        /// <summary>
        ///   Gets the state of the activity.
        /// </summary>
        /// <value>
        ///   The state of the activity.
        /// </value>
        ActivityTraceState State
        {
            get;
        }
    }
}