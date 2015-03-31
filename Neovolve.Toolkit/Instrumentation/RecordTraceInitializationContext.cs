namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="RecordTraceInitializationContext"/>
    ///   struct is used to hold initialization information that determines
    ///   how a <see cref="RecordTrace"/> instances is initialized.
    /// </summary>
    public struct RecordTraceInitializationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTraceInitializationContext"/> struct.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <param name="throwOnSourceLoadFailure">
        /// If set to <c>true</c> and exception will be thrown if the source cannot be loaded.
        /// </param>
        public RecordTraceInitializationContext(String traceSourceName, ITraceSourceResolver resolver, Boolean throwOnSourceLoadFailure)
            : this()
        {
            TraceSourceName = traceSourceName;
            Resolver = resolver;
            ThrowOnSourceLoadFailure = throwOnSourceLoadFailure;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTraceInitializationContext"/> struct.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        public RecordTraceInitializationContext(TraceSource source)
            : this()
        {
            Source = source;
        }

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "contextA">
        ///   The context A.
        /// </param>
        /// <param name = "contextB">
        ///   The context B.
        /// </param>
        /// <returns>
        ///   The result of the operator.
        /// </returns>
        public static Boolean operator ==(RecordTraceInitializationContext contextA, RecordTraceInitializationContext contextB)
        {
            return contextA.Equals(contextB);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "contextA">
        ///   The context A.
        /// </param>
        /// <param name = "contextB">
        ///   The context B.
        /// </param>
        /// <returns>
        ///   The result of the operator.
        /// </returns>
        public static Boolean operator !=(RecordTraceInitializationContext contextA, RecordTraceInitializationContext contextB)
        {
            return contextA.Equals(contextB) == false;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">
        /// Another object to compare to.
        /// </param>
        /// <returns>
        /// True if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is RecordTraceInitializationContext == false)
            {
                return false;
            }

            RecordTraceInitializationContext compare = (RecordTraceInitializationContext)obj;

            return GetHashCode() == compare.GetHashCode();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override Int32 GetHashCode()
        {
            Int32 traceSourceNameHashCode;
            Int32 resolverHashCode;
            Int32 sourceHashCode;

            if (String.IsNullOrEmpty(TraceSourceName))
            {
                traceSourceNameHashCode = 1;
            }
            else
            {
                traceSourceNameHashCode = TraceSourceName.GetHashCode();
            }

            if (Resolver == null)
            {
                resolverHashCode = 2;
            }
            else
            {
                resolverHashCode = Resolver.GetHashCode();
            }

            if (Source == null)
            {
                sourceHashCode = 3;
            }
            else
            {
                sourceHashCode = Source.GetHashCode();
            }

            return traceSourceNameHashCode ^ resolverHashCode ^ ThrowOnSourceLoadFailure.GetHashCode() ^ sourceHashCode;
        }

        /// <summary>
        ///   Gets or sets the resolver.
        /// </summary>
        /// <value>
        ///   The resolver.
        /// </value>
        public ITraceSourceResolver Resolver
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
        ///   Gets or sets a value indicating whether an exception will be thrown if the source cannot be loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an exception will be thrown if the source cannot be loaded, otherwise <c>false</c>.
        /// </value>
        public Boolean ThrowOnSourceLoadFailure
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the trace source.
        /// </summary>
        /// <value>
        ///   The name of the trace source.
        /// </value>
        public String TraceSourceName
        {
            get;
            set;
        }
    }
}