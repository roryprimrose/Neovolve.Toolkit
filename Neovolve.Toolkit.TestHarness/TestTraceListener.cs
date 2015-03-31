namespace Neovolve.Toolkit.TestHarness
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// The <see cref="TestTraceListener"/>
    ///   class is used to test the instrumentation toolkit.
    /// </summary>
    public class TestTraceListener : TraceListener
    {
        /// <summary>
        /// Writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">
        /// A message to write.
        /// </param>
        public override void Write(String message)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, message));
        }

        /// <summary>
        /// Writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">
        /// A message to write.
        /// </param>
        public override void WriteLine(String message)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, message));
        }
    }
}