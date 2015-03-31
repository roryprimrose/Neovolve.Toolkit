namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="RecordWriterContracts"/>
    ///   class is used to define code contracts for the <see cref="IRecordWriter"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IRecordWriter))]
    internal abstract class RecordWriterContracts : IRecordWriter
    {
        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
        }

        /// <summary>
        /// Writes the specified message and type.
        /// </summary>
        /// <param name="type">
        /// The type of message.
        /// </param>
        /// <param name="message">
        /// The message content.
        /// </param>
        /// <param name="arguments">
        /// The arguments related to the message.
        /// </param>
        public void Write(RecordType type, String message, params Object[] arguments)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(message) == false);
        }

        /// <summary>
        /// Writes the specified exception and type.
        /// </summary>
        /// <param name="type">
        /// The type of message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Write(RecordType type, Exception exception)
        {
            Contract.Requires<ArgumentNullException>(exception != null);
        }
    }
}