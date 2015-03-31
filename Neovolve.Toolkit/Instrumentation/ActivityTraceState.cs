namespace Neovolve.Toolkit.Instrumentation
{
    /// <summary>
    /// The <see cref="ActivityTraceState"/>
    ///   enum is used to define the state of a 
    ///   <see cref="ActivityTrace"/> instance.
    /// </summary>
    public enum ActivityTraceState
    {
        /// <summary>
        ///   The <see cref = "ActivityTrace" /> is stopped.
        /// </summary>
        Stopped, 

        /// <summary>
        ///   The <see cref = "ActivityTrace" /> is running.
        /// </summary>
        Running, 

        /// <summary>
        ///   The <see cref = "ActivityTrace" /> has been suspended.
        /// </summary>
        Suspended, 

        /// <summary>
        ///   The <see cref = "ActivityTrace" /> is disabled.
        /// </summary>
        Disabled
    }
}