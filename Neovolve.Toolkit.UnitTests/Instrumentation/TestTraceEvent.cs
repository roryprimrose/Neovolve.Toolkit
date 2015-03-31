namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="TestTraceEvent"/>
    ///   class is used to test the instrumentation toolkit.
    /// </summary>
    public class TestTraceEvent
    {
        /// <summary>
        ///   Gets or sets the type of the event.
        /// </summary>
        /// <value>
        ///   The type of the event.
        /// </value>
        public TraceEventType EventType
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the message.
        /// </summary>
        /// <value>
        ///   The message.
        /// </value>
        public String Message
        {
            get;
            set;
        }
    }
}