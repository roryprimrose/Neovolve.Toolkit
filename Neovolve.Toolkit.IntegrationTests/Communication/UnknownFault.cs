namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="UnknownFault"/>
    ///   class is used to describe unknown faults for testing.
    /// </summary>
    [DataContract]
    public class UnknownFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownFault"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public UnknownFault(String message)
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