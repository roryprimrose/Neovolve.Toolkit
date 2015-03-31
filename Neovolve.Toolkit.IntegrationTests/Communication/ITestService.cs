namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
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

        /// <summary>
        /// Does something else.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// If set to <c>true</c> [second].
        /// </param>
        /// <param name="third">
        /// The third.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        [OperationContract]
        String DoSomethingElse(String first, Boolean second, Int32 third);

        /// <summary>
        /// Gets the current identity.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        [OperationContract]
        String GetCurrentIdentity();

        /// <summary>
        /// Throws the exception.
        /// </summary>
        [OperationContract]
        void ThrowException();

        /// <summary>
        /// Throws the known fault.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(KnownFault))]
        void ThrowKnownFault();

        /// <summary>
        /// Throws the timeout exception.
        /// </summary>
        [OperationContract]
        void ThrowTimeoutException();

        /// <summary>
        /// Throws the unknown fault.
        /// </summary>
        [OperationContract]
        void ThrowUnknownFault();
    }
}