namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Extensions;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="InstanceHandlerTests"/>
    ///   class is used to test the <see cref="InstanceHandler{T}"/> class.
    /// </summary>
    [TestClass]
    public class InstanceHandlerTests
    {
        /// <summary>
        /// Runs test for instance handler can be constructed with data.
        /// </summary>
        [TestMethod]
        public void InstanceHandlerCanBeConstructedWithDataTest()
        {
            String resolutionName = Guid.NewGuid().ToString();

            InstanceHandler<Int32> target = new InstanceHandler<Int32>(resolutionName);

            Assert.AreEqual(resolutionName, target.ResolutionName, "ResolutionName returned an incorrect value");
        }

        /// <summary>
        /// Runs test for instance handler returns instance resolved from extension.
        /// </summary>
        [TestMethod]
        public void InstanceHandlerReturnsInstanceResolvedFromExtensionTest()
        {
            String resolutionName = Guid.NewGuid().ToString();
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            Int32 expected = Environment.TickCount;

            container.Stub(x => x.Resolve(typeof(Int32), resolutionName)).Return(expected);

            try
            {
                InstanceManagerExtension.Container = container;

                InstanceHandler<Int32> target = new InstanceHandler<Int32>(resolutionName);

                Int32 actual = target.Instance;

                Assert.AreEqual(expected, actual, "Instance returned an incorrect value");
                container.AssertWasCalled(x => x.Resolve(typeof(Int32), resolutionName));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance handler returns instance resolved from extension without resolution name.
        /// </summary>
        [TestMethod]
        public void InstanceHandlerReturnsInstanceResolvedFromExtensionWithoutResolutionNameTest()
        {
            String resolutionName = null;
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            Int32 expected = Environment.TickCount;

            container.Stub(x => x.Resolve(typeof(Int32), resolutionName)).Return(expected);

            try
            {
                InstanceManagerExtension.Container = container;

                InstanceHandler<Int32> target = new InstanceHandler<Int32>(resolutionName);

                Int32 actual = target.Instance;

                Assert.AreEqual(expected, actual, "Instance returned an incorrect value");
                container.AssertWasCalled(x => x.Resolve(typeof(Int32), resolutionName));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Runs test for instance handler returns previously resolved instance on subsequent calls.
        /// </summary>
        [TestMethod]
        public void InstanceHandlerReturnsPreviouslyResolvedInstanceOnSubsequentCallsTest()
        {
            String resolutionName = Guid.NewGuid().ToString();
            IUnityContainer container = MockRepository.GenerateStub<IUnityContainer>();
            Int32 expected = Environment.TickCount;

            container.Stub(x => x.Resolve(typeof(Int32), resolutionName)).Return(expected).Repeat.Once();

            try
            {
                InstanceManagerExtension.Container = container;

                InstanceHandler<Int32> target = new InstanceHandler<Int32>(resolutionName);

                Int32 first = target.Instance;
                Int32 second = target.Instance;
                Int32 third = target.Instance;

                Assert.AreEqual(expected, first, "Instance returned an incorrect value");
                Assert.AreEqual(first, second, "Instance returned an incorrect value");
                Assert.AreEqual(second, third, "Instance returned an incorrect value");
                container.AssertWasCalled(x => x.Resolve(typeof(Int32), resolutionName));
            }
            finally
            {
                InstanceManagerExtension.Container = null;
            }
        }

        /// <summary>
        /// Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        /// The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}
