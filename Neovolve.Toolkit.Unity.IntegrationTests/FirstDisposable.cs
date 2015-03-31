namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="FirstDisposable"/>
    ///   class is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public class FirstDisposable : DisposableTester, IFirstDisposable
    {
        /// <summary>
        ///   Gets or sets the duplicate dependency.
        /// </summary>
        /// <value>
        ///   The duplicate dependency.
        /// </value>
        [Dependency]
        public IThirdDisposable DuplicateDependency
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the second.
        /// </summary>
        /// <value>
        ///   The second.
        /// </value>
        [Dependency]
        public ISecondDisposable Second
        {
            get;
            set;
        }
    }
}