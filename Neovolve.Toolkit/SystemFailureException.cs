namespace Neovolve.Toolkit
{
    using System;
    using System.Runtime.Serialization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="SystemFailureException"/>
    ///   class is an exception that is used to identify system level failures.
    /// </summary>
    [Serializable]
    public class SystemFailureException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "SystemFailureException" /> class.
        /// </summary>
        public SystemFailureException()
            : this(Resources.SystemFailureException_UnknownFailure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemFailureException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public SystemFailureException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemFailureException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner exception.
        /// </param>
        public SystemFailureException(String message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemFailureException"/> class.
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
        protected SystemFailureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}