namespace Neovolve.Toolkit.Instrumentation
{
    /// <summary>
    /// The <see cref="RecordType"/>
    ///   enum is used to define the type of record created.
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        ///   The record relates to a detailed event.
        /// </summary>
        Verbose = 0, 

        /// <summary>
        ///   The record relates to an information event.
        /// </summary>
        Information, 

        /// <summary>
        ///   The record relates to a warning.
        /// </summary>
        Warning, 

        /// <summary>
        ///   The record relates an an error.
        /// </summary>
        Error, 

        /// <summary>
        ///   The record relates to a critical event.
        /// </summary>
        Critical
    }
}