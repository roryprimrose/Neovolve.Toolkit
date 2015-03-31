namespace Neovolve.Toolkit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="BusinessFailureException{T}"/>
    ///   class is used to throw an exception when business failures are encountered.
    /// </summary>
    /// <typeparam name="T">
    /// The type of failure code.
    /// </typeparam>
    [Serializable]
    public class BusinessFailureException<T> : Exception where T : struct
    {
        /// <summary>
        ///   Defines the key used to identify the <see cref = "Failures" /> property for serialization.
        /// </summary>
        private const String FailuresKey = "FailuresKey";

        /// <summary>
        ///   Defines the key used to identify the <see cref = "IncludeBaseMessage" /> property for serialization.
        /// </summary>
        private const String IncludeBaseMessageKey = "IncludeBaseMessageKey";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BusinessFailureException{T}" /> class.
        /// </summary>
        public BusinessFailureException()
        {
            IncludeBaseMessage = false;
            Failures = new Collection<BusinessFailure<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailureException{T}"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public BusinessFailureException(String message)
            : base(message)
        {
            IncludeBaseMessage = true;
            Failures = new Collection<BusinessFailure<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailureException{T}"/> class.
        /// </summary>
        /// <param name="code">
        /// The failure code.
        /// </param>
        /// <param name="description">
        /// The failure description.
        /// </param>
        public BusinessFailureException(T code, String description)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(description) == false);

            IncludeBaseMessage = false;
            BusinessFailure<T> failure = new BusinessFailure<T>(code, description);

            Failures = new Collection<BusinessFailure<T>>
                       {
                           failure
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailureException{T}"/> class.
        /// </summary>
        /// <param name="failure">
        /// The failure.
        /// </param>
        public BusinessFailureException(BusinessFailure<T> failure)
        {
            Contract.Requires<ArgumentNullException>(failure != null);

            IncludeBaseMessage = false;
            Failures = new Collection<BusinessFailure<T>>
                       {
                           failure
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailureException{T}"/> class.
        /// </summary>
        /// <param name="failures">
        /// The failures.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "Nesting the generic type is required for strong typing the enumerable instance.")]
        public BusinessFailureException(IEnumerable<BusinessFailure<T>> failures)
        {
            Contract.Requires<ArgumentNullException>(failures != null);

            IncludeBaseMessage = false;
            Failures = new List<BusinessFailure<T>>(failures);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailureException{T}"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public BusinessFailureException(String message, Exception inner)
            : base(message, inner)
        {
            IncludeBaseMessage = true;
            Failures = new Collection<BusinessFailure<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailureException{T}"/> class.
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
        protected BusinessFailureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires<ArgumentNullException>(info != null);

            IncludeBaseMessage = info.GetBoolean(IncludeBaseMessageKey);
            Failures = (ICollection<BusinessFailure<T>>)info.GetValue(FailuresKey, typeof(Collection<BusinessFailure<T>>));
        }

        /// <summary>
        /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Contract.Assume(info != null);

            info.AddValue(IncludeBaseMessageKey, IncludeBaseMessage);
            info.AddValue(FailuresKey, Failures);

            base.GetObjectData(info, context);
        }

        /// <summary>
        ///   Gets the failures.
        /// </summary>
        /// <value>
        ///   The failures.
        /// </value>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "The nested generic type is required for strong typing.")]
        public IEnumerable<BusinessFailure<T>> Failures
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets a message that describes the current exception.
        /// </summary>
        /// <value>
        ///   The exception message.
        /// </value>
        /// <returns>
        ///   The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        public override String Message
        {
            get
            {
                String message = String.Empty;

                if (IncludeBaseMessage)
                {
                    message = base.Message;
                }

                String failureMessages = String.Empty;

                List<BusinessFailure<T>> businessFailures = new List<BusinessFailure<T>>(Failures);

                if (businessFailures.Count == 0)
                {
                    // Add a default failure
                    String description = Resources.BusinessFailureException_UnknownFailure;

                    if (String.IsNullOrWhiteSpace(description))
                    {
                        const String AssemblyResourceStringWasNotFound = "Assembly resource string was not found.";

                        throw new InvalidOperationException(AssemblyResourceStringWasNotFound);
                    }

                    BusinessFailure<T> defaultFailure = new BusinessFailure<T>(default(T), description);

                    businessFailures.Add(defaultFailure);
                }

                for (Int32 index = 0; index < businessFailures.Count; index++)
                {
                    BusinessFailure<T> failure = businessFailures[index];
                    String failureMessage = String.Format(
                        CultureInfo.CurrentUICulture, Resources.BusinessFailureException_FailureMessageFormat, failure.Code, failure.Description);

                    failureMessages = String.Concat(failureMessages, Environment.NewLine, failureMessage);
                }

                message += String.Format(CultureInfo.CurrentUICulture, Resources.BusinessFailureException_MessageHeader, failureMessages);

                return message;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the message from the base class will be included.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the message from the base class will be included; otherwise, <c>false</c>.
        /// </value>
        protected Boolean IncludeBaseMessage
        {
            get;
            set;
        }
    }
}