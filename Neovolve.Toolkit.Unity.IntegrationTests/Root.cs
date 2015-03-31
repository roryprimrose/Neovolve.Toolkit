namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    /// <summary>
    /// The <see cref="Root"/>
    ///   class is used to test proxy injection.
    /// </summary>
    public class Root
    {
        /// <summary>
        ///   The _tester.
        /// </summary>
        private readonly ProxyTest _tester;

        /// <summary>
        /// Initializes a new instance of the <see cref="Root"/> class.
        /// </summary>
        /// <param name="tester">
        /// The tester.
        /// </param>
        public Root(ProxyTest tester)
        {
            _tester = tester;
        }

        /// <summary>
        /// Runs test for run.
        /// </summary>
        public void RunTest()
        {
            _tester.RenderMessage("This is the test message from the Root class");
        }
    }
}