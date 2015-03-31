namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="ActivityTraceContext"/>
    ///   class is used to provide information about a
    ///   <see cref="ActivityTrace"/> as it is stored in the
    ///   <see cref="CorrelationManager.LogicalOperationStack"/>.
    /// </summary>
    internal class ActivityTraceContext
    {
        /// <summary>
        ///   Defines an empty <see cref = "ActivityTraceContext" /> instance.
        /// </summary>
        public static readonly ActivityTraceContext Empty = new ActivityTraceContext();

        /// <summary>
        ///   Stores the State value.
        /// </summary>
        private ActivityTraceState _state;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ActivityTraceContext" /> class.
        /// </summary>
        public ActivityTraceContext()
        {
            State = ActivityTraceState.Disabled;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.
        /// </param>
        /// <returns>
        /// True if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            ActivityTraceContext compare = obj as ActivityTraceContext;

            if (compare == null)
            {
                return false;
            }

            return GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override Int32 GetHashCode()
        {
            Int32 sourceHashCode;

            if (Source != null)
            {
                sourceHashCode = Source.GetHashCode();
            }
            else
            {
                sourceHashCode = 1;
            }

            return ActivityId.GetHashCode() ^ PreviousActivityId.GetHashCode() ^ State.GetHashCode() ^ sourceHashCode;
        }

        /// <summary>
        ///   Gets or sets the activity id.
        /// </summary>
        /// <value>
        ///   The activity id.
        /// </value>
        public Guid ActivityId
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the previous activity id.
        /// </summary>
        /// <value>
        ///   The previous activity id.
        /// </value>
        public Guid PreviousActivityId
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the source.
        /// </summary>
        /// <value>
        ///   The source.
        /// </value>
        public TraceSource Source
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the state of the current instance.
        /// </summary>
        /// <value>
        ///   The state of the current instance.
        /// </value>
        /// <remarks>
        ///   The returned value will always be <see cref = "ActivityTraceState.Disabled" /> if <see cref = "Source" /> is <c>null</c>.
        /// </remarks>
        public ActivityTraceState State
        {
            get
            {
                if (Source == null)
                {
                    return ActivityTraceState.Disabled;
                }

                return _state;
            }

            set
            {
                _state = value;
            }
        }
    }
}