namespace Neovolve.Toolkit
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="BusinessFailure&lt;T&gt;"/>
    ///   class is used to define a business failure.
    /// </summary>
    /// <typeparam name="T">
    /// The type of code that identifies the failure. This is usually an enum value.
    /// </typeparam>
    [Serializable]
    public class BusinessFailure<T> where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFailure{T}"/> class.
        /// </summary>
        /// <param name="code">
        /// The code of the failure.
        /// </param>
        /// <param name="description">
        /// The description of the failure.
        /// </param>
        public BusinessFailure(T code, String description)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrEmpty(description) == false);

            Code = code;
            Description = description;
        }

        /// <summary>
        ///   Gets the code that identifies the failure.
        /// </summary>
        /// <value>
        ///   The code that identifies the failure.
        /// </value>
        public T Code
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the description of the failure.
        /// </summary>
        /// <value>
        ///   The description of the failure.
        /// </value>
        public String Description
        {
            get;
            private set;
        }
    }
}