namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="TraceSourceLoadException"/>
    ///   class is used to identify scenarios where a 
    ///   <see cref="TraceSource"/> is not retrieved for use by a
    ///   <see cref="RecordTrace"/> instance.
    /// </summary>
    [Serializable]
    public class TraceSourceLoadException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "TraceSourceLoadException" /> class.
        /// </summary>
        public TraceSourceLoadException()
            : this(Resources.TraceSourceLoadExceptionMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceLoadException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public TraceSourceLoadException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceLoadException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public TraceSourceLoadException(String message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceLoadException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected TraceSourceLoadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}