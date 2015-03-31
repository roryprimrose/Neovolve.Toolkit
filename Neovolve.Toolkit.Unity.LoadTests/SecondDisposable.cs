namespace Neovolve.Toolkit.Unity.LoadTests
{
    /// <summary>
    /// The <see cref="SecondDisposable"/>
    ///   class is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public class SecondDisposable : DisposableTester, ISecondDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecondDisposable"/> class.
        /// </summary>
        /// <param name="third">
        /// The third.
        /// </param>
        public SecondDisposable(IThirdDisposable third)
        {
            Third = third;
        }

        /// <summary>
        ///   Gets or sets Third.
        /// </summary>
        /// <value>
        ///   The third.
        /// </value>
        public IThirdDisposable Third
        {
            get;
            set;
        }
    }
}