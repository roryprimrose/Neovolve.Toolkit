namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="IRecordWriter"/>
    ///   interface defines the methods for writing instrumentation records.
    /// </summary>
    [ContractClass(typeof(RecordWriterContracts))]
    public interface IRecordWriter
    {
        /// <summary>
        /// Flushes this instance.
        /// </summary>
        void Flush();

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
        void Write(RecordType type, String message, params Object[] arguments);

        /// <summary>
        /// Writes the specified exception and type.
        /// </summary>
        /// <param name="type">
        /// The type of message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        void Write(RecordType type, Exception exception);
    }
}