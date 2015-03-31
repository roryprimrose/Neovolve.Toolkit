namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="ThirdDisposable"/>
    ///   class is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public class ThirdDisposable : DisposableTester, IThirdDisposable
    {
        /// <summary>
        ///   Gets or sets the fourth.
        /// </summary>
        /// <value>
        ///   The fourth.
        /// </value>
        [Dependency]
        public IFourthDisposable Fourth
        {
            get;
            set;
        }
    }
}