namespace Neovolve.Toolkit.Workflow.IntegrationTests
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// The <see cref="ITestService"/>
    ///   interface is used to provide a test service contract.
    /// </summary>
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(InvalidCastException))]
        String GetData(String data);
    }
}