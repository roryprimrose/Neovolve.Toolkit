namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System.ServiceModel;

    /// <summary>
    /// The <see cref="ITestService"/> interface is a test service contract.
    /// </summary>
    [ServiceContract]
    internal interface ITestService
    {
        /// <summary>
        /// Does something.
        /// </summary>
        [OperationContract]
        void DoSomething();
    }
}