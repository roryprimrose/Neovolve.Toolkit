namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="BuildTreeRecoveryTests"/>
    ///   class is used to test the <see cref="BuildTreeRecovery"/> class.
    /// </summary>
    [TestClass]
    public class BuildTreeRecoveryTests
    {
        /// <summary>
        /// Runs test for build tree recovery disposes failed node executed.
        /// </summary>
        [TestMethod]
        public void BuildTreeRecoveryDisposesFailedNodeExecutedTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode node = new BuildTreeItemNode(buildKey, true, null);
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);

                node.AssignInstance(disposable);

                BuildTreeRecovery target = new BuildTreeRecovery(context, node, null);

                target.Recover();
            }

            disposable.AssertWasCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for build tree recovery invokes recovery action when executed.
        /// </summary>
        [TestMethod]
        public void BuildTreeRecoveryInvokesRecoveryActionWhenExecutedTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode node = new BuildTreeItemNode(buildKey, true, null);
            Boolean actionInvoked = false;

            BuildTreeRecovery target = new BuildTreeRecovery(context, node, x => actionInvoked = true);

            target.Recover();

            Assert.IsTrue(actionInvoked, "Action was not invoked");
        }

        /// <summary>
        /// Runs test for build tree recovery removes failed node from parent executed.
        /// </summary>
        [TestMethod]
        public void BuildTreeRecoveryRemovesFailedNodeFromParentExecutedTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode parentNode = new BuildTreeItemNode(buildKey, true, null);
            BuildTreeItemNode node = new BuildTreeItemNode(buildKey, true, parentNode);
            IDisposable disposable = MockRepository.GenerateStub<IDisposable>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);

                node.AssignInstance(disposable);

                BuildTreeRecovery target = new BuildTreeRecovery(context, node, null);

                target.Recover();
            }

            Assert.AreEqual(0, parentNode.Children.Count, "Count returned an incorrect value");
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