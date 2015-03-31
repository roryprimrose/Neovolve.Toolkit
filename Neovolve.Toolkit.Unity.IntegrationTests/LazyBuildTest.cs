namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="LazyBuildTest"/>
    ///   class is used to test build tree tracking for lazy unity injection.
    /// </summary>
    public class LazyBuildTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBuildTest"/> class.
        /// </summary>
        /// <param name="third">
        /// The third.
        /// </param>
        public LazyBuildTest(Func<IThirdDisposable> third)
        {
            Third = third;
        }

        /// <summary>
        ///   Gets or sets the forth.
        /// </summary>
        /// <value>
        ///   The forth.
        /// </value>
        [Dependency]
        public Func<IFourthDisposable> Forth
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the third.
        /// </summary>
        /// <value>
        ///   The third.
        /// </value>
        public Func<IThirdDisposable> Third
        {
            get;
            private set;
        }
    }
}