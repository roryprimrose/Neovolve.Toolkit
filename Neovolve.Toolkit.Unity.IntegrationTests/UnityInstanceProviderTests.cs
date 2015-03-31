namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;
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
        /// Runs test for unity instance provider can be created with null resolve name.
        /// </summary>
        [TestMethod]
        public void UnityInstanceProviderCanBeCreatedWithNullResolveNameTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();

            new UnityInstanceProvider(container, typeof(IDisposable), null);
        }

        /// <summary>
        /// Runs test for unity instance provider returns instance from container.
        /// </summary>
        [TestMethod]
        public void UnityInstanceProviderReturnsInstanceFromContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            IDoSomething expected = MockRepository.GenerateStub<IDoSomething>();
            String name = Guid.NewGuid().ToString();

            container.Stub(x => x.Resolve<IDoSomething>(name)).Return(expected);

            UnityInstanceProvider target = new UnityInstanceProvider(container, typeof(IDoSomething), name);

            Object actual = target.GetInstance(null, null);

            Assert.AreSame(expected, actual, "GetInstance returned an incorrect value");
            container.AssertWasCalled(x => x.Resolve(typeof(IDoSomething), name));
        }

        /// <summary>
        /// Runs test for unity instance provider returns instance from container without name.
        /// </summary>
        [TestMethod]
        public void UnityInstanceProviderReturnsInstanceFromContainerWithoutNameTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            IDoSomething expected = MockRepository.GenerateStub<IDoSomething>();

            container.Stub(x => x.Resolve<IDoSomething>()).Return(expected);

            UnityInstanceProvider target = new UnityInstanceProvider(container, typeof(IDoSomething), null);

            Object actual = target.GetInstance(null);

            Assert.AreSame(expected, actual, "GetInstance returned an incorrect value");
            container.AssertWasCalled(x => x.Resolve(typeof(IDoSomething), null));
        }

        /// <summary>
        /// Runs test for unity instance provider tears down instance in container.
        /// </summary>
        [TestMethod]
        public void UnityInstanceProviderTearsDownInstanceInContainerTest()
        {
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            IDoSomething expected = MockRepository.GenerateStub<IDoSomething>();

            UnityInstanceProvider target = new UnityInstanceProvider(container, typeof(IDoSomething), null);

            target.ReleaseInstance(null, expected);

            container.AssertWasCalled(x => x.Teardown(expected));
        }

        /// <summary>
        /// Runs test for unity instance provider throws exception with null container.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityInstanceProviderThrowsExceptionWithNullContainerTest()
        {
            new UnityInstanceProvider(null, typeof(String), null);
        }

        /// <summary>
        /// Runs test for unity instance provider throws exception with null service type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnityInstanceProviderThrowsExceptionWithNullServiceTypeTest()
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