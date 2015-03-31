namespace Neovolve.Toolkit.Workflow
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The <see cref="GenericArgumentCount"/>
    ///   enum defines the number of generic arguments available for an activity type.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", 
        Justification = "The value of the enum must convert to System.Int32 and retain the expected meaning.")]
    public enum GenericArgumentCount
    {
        /// <summary>
        ///   One generic argument is available.
        /// </summary>
        One = 1, 

        /// <summary>
        ///   Two generic arguments are available.
        /// </summary>
        Two, 

        /// <summary>
        ///   Three generic arguments are available.
        /// </summary>
        Three, 

        /// <summary>
        ///   Four generic arguments are available.
        /// </summary>
        Four, 

        /// <summary>
        ///   Five generic arguments are available.
        /// </summary>
        Five, 

        /// <summary>
        ///   Six generic arguments are available.
        /// </summary>
        Six, 

        /// <summary>
        ///   Seven generic arguments are available.
        /// </summary>
        Seven, 

        /// <summary>
        ///   Eight generic arguments are available.
        /// </summary>
        Eight, 

        /// <summary>
        ///   Nine generic arguments are available.
        /// </summary>
        Nine, 

        /// <summary>
        ///   Ten generic arguments are available.
        /// </summary>
        Ten, 

        /// <summary>
        ///   Eleven generic arguments are available.
        /// </summary>
        Eleven, 

        /// <summary>
        ///   Twelve generic arguments are available.
        /// </summary>
        Twelve, 

        /// <summary>
        ///   Thirteen generic arguments are available.
        /// </summary>
        Thirteen, 

        /// <summary>
        ///   Fourteen generic arguments are available.
        /// </summary>
        Fourteen, 

        /// <summary>
        ///   Fifteen generic arguments are available.
        /// </summary>
        Fifteen, 

        /// <summary>
        ///   Sixteen generic arguments are available.
        /// </summary>
        Sixteen
    }
}