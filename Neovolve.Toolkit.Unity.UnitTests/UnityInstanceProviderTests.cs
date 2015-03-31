namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.IO;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="UnityInstanceProviderTests"/>
    ///   class is used to test the <see cref="UnityInstanceProvider"/> class.
    /// </summary>
    [TestClass]
    public class UnityInstanceProviderTests
    {
        /// <summary>
        /// Runs test for unity instance provider get instance returns instance from container.
        /// </summary>
        [TestMethod]
        public void UnityInstanceProviderGetInstanceReturnsInstanceFromContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            Type serviceType = typeof(ArgumentException);
            ArgumentException expected = new ArgumentNullException();

            container.Stub(x => x.Resolve(serviceType)).Return(expected);

            UnityInstanceProvider target = new UnityInstanceProvider(container, serviceType, null);

            Object actual = target.GetInstance(null);

            Assert.AreSame(expected, actual, "GetInstance returned an incorrect value");
        }

        /// <summary>
        /// Runs test for unity instance provider release instance releases resolved instance.
        /// </summary>
        [TestMethod]
        public void UnityInstanceProviderReleaseInstanceReleasesResolvedInstanceTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            Type serviceType = typeof(Stream);
            IDisposable expected = MockRepository.GenerateStub<IDisposable>();

            container.Stub(x => x.Resolve(serviceType)).Return(expected);
            container.Stub(x => x.Teardown(expected)).WhenCalled(x => ((IDisposable)x.Arguments[0]).Dispose());

            UnityInstanceProvider target = new UnityInstanceProvider(container, serviceType, null);

            Object actual = target.GetInstance(null);

            target.ReleaseInstance(null, actual);

            expected.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for unity instance provider throws exception when created with null container.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityInstanceProviderThrowsExceptionWhenCreatedWithNullContainerTest()
        {
            new UnityInstanceProvider(null, GetType(), null);
        }

        /// <summary>
        /// Runs test for unity instance provider throws exception when created with null service type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityInstanceProviderThrowsExceptionWhenCreatedWithNullServiceTypeTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            new UnityInstanceProvider(container, null, null);
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