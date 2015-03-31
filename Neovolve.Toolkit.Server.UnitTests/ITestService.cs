namespace Neovolve.Toolkit.Server.UnitTests
{
    using System.ServiceModel;

    /// <summary>
    /// The <see cref="ITestService"/>
    ///   interface is used to test WCF unit tests.
    /// </summary>
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// Does something.
        /// </summary>
        [OperationContract]
        void DoSomething();
    }
}