namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="KnownFault"/>
    ///   class is used to describe a known fault for testing.
    /// </summary>
    [DataContract]
    public class KnownFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFault"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public KnownFault(String message)
        {
            Message = message;
        }

        /// <summary>
        ///   Gets or sets the message.
        /// </summary>
        /// <value>
        ///   The message.
        /// </value>
        [DataMember]
        public String Message
        {
            get;
            set;
        }
    }
}