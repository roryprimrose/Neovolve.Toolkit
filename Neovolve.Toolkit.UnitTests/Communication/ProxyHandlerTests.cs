namespace Neovolve.Toolkit.UnitTests.Communication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Remoting.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Communication;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ProxyHandlerTests"/>
    ///   class is used to test the <see cref="ProxyHandler{T}"/> class.
    /// </summary>
    [TestClass]
    public class ProxyHandlerTests
    {
        /// <summary>
        /// Runs test for proxy handler returns exception message with empty method name.
        /// </summary>
        [TestMethod]
        public void ProxyHandlerReturnsExceptionMessageWithEmptyMethodNameTest()
        {
            IMessage message = MockRepository.GenerateStub<IMessage>();
            ProxyHandler<Stream> target = new ProxyHandlerWrapper<Stream>();
            Dictionary<String, Object> properties = new Dictionary<String, Object>();

            properties["__MethodName"] = String.Empty;

            message.Stub(x => x.Properties).Return(properties);

            IMessage response = target.Invoke(message);

            Assert.IsNotNull(response, "Invoke failed to return an instance");
            Assert.IsInstanceOfType(response, typeof(ReturnMessage), "Invoke returned an incorrect type");

            ReturnMessage typedResponse = (ReturnMessage)response;

            Assert.IsNotNull(typedResponse.Exception, "Exception returned an incorrect value");
            Assert.IsInstanceOfType(typedResponse.Exception, typeof(InvalidOperationException));
        }

        /// <summary>
        /// Runs test for proxy handler throws exception with null method name.
        /// </summary>
        [TestMethod]
        public void ProxyHandlerReturnsExceptionMessageWithNullMethodNameTest()
        {
            IMessage message = MockRepository.GenerateStub<IMessage>();
            ProxyHandler<Stream> target = new ProxyHandlerWrapper<Stream>();
            Dictionary<String, Object> properties = new Dictionary<String, Object>();

            properties["__MethodName"] = null;

            message.Stub(x => x.Properties).Return(properties);

            IMessage response = target.Invoke(message);

            Assert.IsNotNull(response, "Invoke failed to return an instance");
            Assert.IsInstanceOfType(response, typeof(ReturnMessage), "Invoke returned an incorrect type");

            ReturnMessage typedResponse = (ReturnMessage)response;

            Assert.IsNotNull(typedResponse.Exception, "Exception returned an incorrect value");
            Assert.IsInstanceOfType(typedResponse.Exception, typeof(InvalidOperationException));
        }

        /// <summary>
        /// Runs test for proxy handler throws exception with null parameter type.
        /// </summary>
        [TestMethod]
        public void ProxyHandlerReturnsExceptionMessageWithNullParameterTypeTest()
        {
            IMessage message = MockRepository.GenerateStub<IMessage>();
            Object[] parameterTypes = new[]
                                      {
                                          typeof(String), null, typeof(Boolean)
                                      };
            ProxyHandler<Stream> target = new ProxyHandlerWrapper<Stream>();
            Dictionary<String, Object> properties = new Dictionary<String, Object>();

            properties["__MethodName"] = "ReadByte";
            properties["__MethodSignature"] = parameterTypes;

            message.Stub(x => x.Properties).Return(properties);

            IMessage response = target.Invoke(message);

            Assert.IsNotNull(response, "Invoke failed to return an instance");
            Assert.IsInstanceOfType(response, typeof(ReturnMessage), "Invoke returned an incorrect type");

            ReturnMessage typedResponse = (ReturnMessage)response;

            Assert.IsNotNull(typedResponse.Exception, "Exception returned an incorrect value");
            Assert.IsInstanceOfType(typedResponse.Exception, typeof(InvalidOperationException));
        }

        /// <summary>
        /// Runs test for proxy handler throws exception with undefined method name.
        /// </summary>
        [TestMethod]
        public void ProxyHandlerReturnsExceptionMessageWithUndefinedMethodNameTest()
        {
            IMessage message = MockRepository.GenerateStub<IMessage>();
            ProxyHandler<Stream> target = new ProxyHandlerWrapper<Stream>();

            message.Stub(x => x.Properties).Return(new Dictionary<String, Object>());

            IMessage response = target.Invoke(message);

            Assert.IsNotNull(response, "Invoke failed to return an instance");
            Assert.IsInstanceOfType(response, typeof(ReturnMessage), "Invoke returned an incorrect type");

            ReturnMessage typedResponse = (ReturnMessage)response;

            Assert.IsNotNull(typedResponse.Exception, "Exception returned an incorrect value");
            Assert.IsInstanceOfType(typedResponse.Exception, typeof(InvalidOperationException));
        }

        /// <summary>
        /// Runs test for proxy handler throws exception with white space method name.
        /// </summary>
        [TestMethod]
        public void ProxyHandlerReturnsExceptionMessageWithWhiteSpaceMethodNameTest()
        {
            IMessage message = MockRepository.GenerateStub<IMessage>();
            ProxyHandler<Stream> target = new ProxyHandlerWrapper<Stream>();
            Dictionary<String, Object> properties = new Dictionary<String, Object>();

            properties["__MethodName"] = " ";

            message.Stub(x => x.Properties).Return(properties);

            IMessage response = target.Invoke(message);

            Assert.IsNotNull(response, "Invoke failed to return an instance");
            Assert.IsInstanceOfType(response, typeof(ReturnMessage), "Invoke returned an incorrect type");

            ReturnMessage typedResponse = (ReturnMessage)response;

            Assert.IsNotNull(typedResponse.Exception, "Exception returned an incorrect value");
            Assert.IsInstanceOfType(typedResponse.Exception, typeof(InvalidOperationException));
        }

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}