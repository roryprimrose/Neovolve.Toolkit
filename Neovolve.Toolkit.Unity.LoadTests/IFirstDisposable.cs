namespace Neovolve.Toolkit.Unity.LoadTests
{
    using System;

    /// <summary>
    /// The <see cref="IFirstDisposable"/>
    ///   interface is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public interface IFirstDisposable : IDisposable
    {
        /// <summary>
        ///   Gets or sets the duplicate dependency.
        /// </summary>
        /// <value>
        ///   The duplicate dependency.
        /// </value>
        IThirdDisposable DuplicateDependency
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        Boolean IsDisposed
        {
            get;
        }

        /// <summary>
        ///   Gets or sets the second.
        /// </summary>
        /// <value>
        ///   The second.
        /// </value>
        ISecondDisposable Second
        {
            get;
            set;
        }
    }
}