namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="ErrorHandlerAttributeTests"/>
    ///   class is used to test the <see cref="ErrorHandlerAttribute"/> class.
    /// </summary>
    [TestClass]
    public class ErrorHandlerAttributeTests
    {
        /// <summary>
        /// A test for ApplyDispatchBehavior.
        /// </summary>
        [TestMethod]
        public void ApplyDispatchBehaviorTest()
        {
            const String ServiceAddress = "net.pipe://localhost/ErrorHandlerAttributeTests";

            using (ServiceHost host = new ServiceHost(typeof(TestService)))
            {
                host.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), ServiceAddress);

                try
                {
                    host.Open();

                    ITestService channel;

                    using (ChannelFactory<ITestService> factory = new ChannelFactory<ITestService>(new NetNamedPipeBinding(), ServiceAddress))
                    {
                        try
                        {
                            factory.Open();

                            channel = factory.CreateChannel();

                            channel.DoSomething();

                            try
                            {
                                channel.ThrowKnownFault();
                            }
                            catch (FaultException<KnownFault>)
                            {
                            }

                            try
                            {
                                // This call should fault the channel
                                channel.ThrowUnknownFault();
                            }
                            catch (FaultException)
                            {
                            }
                        }
                        finally
                        {
                            if (factory.State != CommunicationState.Faulted)
                            {
                                try
                                {
                                    factory.Close();
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);
                                }
                            }
                        }
                    }

                    using (ChannelFactory<ITestService> secondFactory = new ChannelFactory<ITestService>(new NetNamedPipeBinding(), ServiceAddress))
                    {
                        secondFactory.Open();

                        channel = secondFactory.CreateChannel();

                        channel.DoSomething();

                        try
                        {
                            // This call will fault the channel
                            channel.ThrowException();
                        }
                        catch (FaultException)
                        {
                        }

                        // This channel is dead so we can't close it, just abort instead
                        secondFactory.Abort();
                    }
                }
                finally
                {
                    if (host.State != CommunicationState.Faulted)
                    {
                        try
                        {
                            host.Close();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///   The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}