namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The <see cref="UnknownErrorHandler"/>
    ///   class is an test error handler for unknown scenarios.
    /// </summary>
    internal class UnknownErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether subsequent HandleError implementations are called.
        /// </summary>
        /// <param name="error">
        /// The exception thrown during processing.
        /// </param>
        /// <returns>
        /// True if subsequent <see cref="T:System.ServiceModel.Dispatcher.IErrorHandler"/> implementations must not be called; otherwise, false. The default is false.
        /// </returns>
        public Boolean HandleError(Exception error)
        {
            if (error is UnknownException)
            {
                Debug.WriteLine("Unknown error encountered and logged");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"/> that is returned from an exception in the course of a service method.
        /// </summary>
        /// <param name="error">
        /// The <see cref="T:System.Exception"/> object thrown in the course of the service operation.
        /// </param>
        /// <param name="version">
        /// The SOAP version of the message.
        /// </param>
        /// <param name="fault">
        /// The <see cref="T:System.ServiceModel.Channels.Message"/> object that is returned to the client, or service, in the duplex case.
        /// </param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is UnknownException)
            {
                // Shield the unUnknown exception
                UnknownFault faultInstance = new UnknownFault("Unknown error encountered. All details have been logged.");
                FaultException<UnknownFault> faultException = new FaultException<UnknownFault>(faultInstance);
                MessageFault messageFault = faultException.CreateMessageFault();

                fault = Message.CreateMessage(version, messageFault, faultException.Action);
            }
        }
    }
}