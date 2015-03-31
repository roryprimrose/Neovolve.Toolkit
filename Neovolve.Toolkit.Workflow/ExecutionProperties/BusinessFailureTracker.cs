namespace Neovolve.Toolkit.Workflow.ExecutionProperties
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="BusinessFailureTracker{T}"/>
    ///   class is used to add a business failure to a <see cref="BusinessFailureScope{T}"/> activity.
    /// </summary>
    /// <typeparam name="T">
    /// The type of failure code.
    /// </typeparam>
    [DataContract]
    public class BusinessFailureTracker<T> where T : struct
    {
        /// <summary>
        ///   Defines the name of the execution property.
        /// </summary>
        private static readonly String _executionPropertyName = typeof(BusinessFailureTracker<T>).FullName;

        /// <summary>
        ///   Stores the failures.
        /// </summary>
        private readonly Collection<BusinessFailure<T>> _failures = new Collection<BusinessFailure<T>>();

        /// <summary>
        ///   Gets the name of the property.
        /// </summary>
        /// <value>
        ///   The name of the property.
        /// </value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", 
            Justification = "This is the cleanest way to build a property name for the generic type.")]
        public static String Name
        {
            get
            {
                return _executionPropertyName;
            }
        }

        /// <summary>
        ///   Gets the add failure method.
        /// </summary>
        /// <value>
        ///   The add failure method.
        /// </value>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "Serialization requires that the collection of generic failures is exposed.")]
        [DataMember]
        public Collection<BusinessFailure<T>> Failures
        {
            get
            {
                return _failures;
            }
        }
    }
}