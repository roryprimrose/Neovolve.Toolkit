namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="UnknownException"/>
    ///   class is used to throw unknown exceptions for testing.
    /// </summary>
    [Serializable]
    public class UnknownException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "UnknownException" /> class.
        /// </summary>
        public UnknownException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public UnknownException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public UnknownException(String message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownException"/> class.
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
        protected UnknownException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}