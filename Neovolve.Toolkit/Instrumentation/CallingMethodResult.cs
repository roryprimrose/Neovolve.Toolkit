namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// The <see cref="CallingMethodResult"/>
    ///   struct is used to describe the results of calculating the calling method and type
    ///   by the <see cref="CallingMethodResolver"/> class.
    /// </summary>
    internal struct CallingMethodResult
    {
        /// <summary>
        ///   Gets or sets the calling method.
        /// </summary>
        /// <value>
        ///   The calling method.
        /// </value>
        public MethodBase CallingMethod
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Gets the calling type.
        /// </summary>
        /// <value>
        ///   The calling type.
        /// </value>
        public Type CallingType
        {
            get
            {
                Debug.Assert(CallingMethod != null, "CallingMethod does not exist");

                return CallingMethod.DeclaringType;
            }
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
            internal set;
        }
    }
}