namespace Neovolve.Toolkit.IntegrationTests.Communication.Security
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// The <see cref="IPasswordService"/>
    ///   interface is the service contract used to test the user name password security implementation.
    /// </summary>
    [ServiceContract]
    public interface IPasswordService
    {
        /// <summary>
        /// Gets the context password.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        [OperationContract]
        String GetContextPassword();

        /// <summary>
        /// Gets the name of the context user.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        [OperationContract]
        String GetContextUserName();

        /// <summary>
        /// Gets the is correct context identity.
        /// </summary>
        /// <returns>
        /// A <see cref="Boolean"/> value.
        /// </returns>
        [OperationContract]
        Boolean GetIsCorrectContextIdentity();

        /// <summary>
        /// Gets the is correct thread identity.
        /// </summary>
        /// <returns>
        /// A <see cref="Boolean"/> value.
        /// </returns>
        [OperationContract]
        Boolean GetIsCorrectThreadIdentity();

        /// <summary>
        /// Gets the thread password.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        [OperationContract]
        String GetThreadPassword();

        /// <summary>
        /// Gets the name of the thread user.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        [OperationContract]
        String GetThreadUserName();

        /// <summary>
        /// Sleeps the specified milliseconds.
        /// </summary>
        /// <param name="milliseconds">
        /// The milliseconds.
        /// </param>
        [OperationContract]
        void Sleep(Int32 milliseconds);
    }
}