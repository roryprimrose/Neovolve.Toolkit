namespace Neovolve.Toolkit.Server.Unity.WebIntegrationTests
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// The <see cref="ITestService"/>
    ///   interface is used to define a test service contract.
    /// </summary>
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// Gets some value.
        /// </summary>
        /// <param name="data">
        /// The data for the call.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        [OperationContract]
        String GetSomeValue(String data);
    }
}