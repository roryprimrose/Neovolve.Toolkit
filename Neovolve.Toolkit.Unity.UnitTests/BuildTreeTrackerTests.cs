namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using System.Linq;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="BuildTreeTrackerTests"/>
    ///   class is used to test the <see cref="BuildTreeTracker"/> class.
    /// </summary>
    [TestClass]
    public class BuildTreeTrackerTests
    {
        /// <summary>
        /// Runs test for build tree tracker can assign an instance to the current node for the correct build key.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCanAssignAnInstanceToTheCurrentNodeForTheCorrectBuildKeyTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target = new BuildTreeTracker();

            context.BuildKey = buildKey;
            context.Stub(x => x.OriginalBuildKey).Return(buildKey);
            context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

            target.PreBuildUp(context);

            context.Existing = instance;

            target.PostBuildUp(context);

            Assert.AreEqual(1, target.BuildTrees.Count(), "Count returned an incorrect value");
            BuildTreeItemNode firstBuildTree = target.BuildTrees.First();
            Assert.IsTrue(firstBuildTree.ItemReference.IsAlive, "IsAlive returned an incorrect value");
            Assert.AreSame(instance, firstBuildTree.ItemReference.Target, "Target returned an incorrect value");
        }

        /// <summary>
        /// Runs test for build tree tracker can dispose and remove all trees.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCanDisposeAndRemoveAllTreesTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            IDisposable firstDisposable = MockRepository.GenerateStub<IDisposable>();
            IDisposable secondDisposable = MockRepository.GenerateStub<IDisposable>();

            BuildTreeTracker target = new BuildTreeTracker();

            context.BuildKey = buildKey;
            context.Stub(x => x.OriginalBuildKey).Return(buildKey);
            context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

            target.PreBuildUp(context);
            context.Existing = firstDisposable;
            target.PostBuildUp(context);
            context.Existing = null;

            target.PreBuildUp(context);
            context.Existing = secondDisposable;
            target.PostBuildUp(context);

            target.DisposeAllTrees();

            firstDisposable.AssertWasCalled(x => x.Dispose());
            secondDisposable.AssertWasCalled(x => x.Dispose());

            Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
        }

        /// <summary>
        /// Runs test for build tree tracker can handle disposing tree node when node is already disposed.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCanHandleDisposingTreeNodeWhenNodeIsAlreadyDisposedTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            IDisposable instance = MockRepository.GenerateStub<IDisposable>();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(new NamedTypeBuildKey(typeof(IDisposable)));
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());
                instance.Stub(x => x.Dispose()).Throw(new ObjectDisposedException("This instance is already disposed"));

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);
                target.PostTearDown(context);

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
                instance.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker correctly populates tree node for item built up by container.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCorrectlyPopulatesTreeNodeForItemBuiltUpByContainerTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Existing = instance;
                context.BuildKey = buildKey;
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);
                target.PostBuildUp(context);

                Assert.AreEqual(1, target.BuildTrees.Count(), "Count returned an incorrect value");
                BuildTreeItemNode firstBuildTree = target.BuildTrees.First();
                Assert.IsTrue(firstBuildTree.ItemReference.IsAlive, "IsAlive returned an incorrect value");
                Assert.IsFalse(firstBuildTree.NodeCreatedByContainer, "NodeCreatedByContainer returned an incorrect value");
                Assert.AreSame(instance, firstBuildTree.ItemReference.Target, "Target returned an incorrect value");
                Assert.AreEqual(buildKey, firstBuildTree.BuildKey, "BuildKey returned an incorrect value");
                Assert.IsNull(firstBuildTree.Parent, "Parent returned an incorrect value");
                Assert.AreEqual(0, firstBuildTree.Children.Count, "Count returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker correctly populates tree node for item created by container.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCorrectlyPopulatesTreeNodeForItemCreatedByContainerTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);

                Assert.AreEqual(1, target.BuildTrees.Count(), "Count returned an incorrect value");
                BuildTreeItemNode buildTree = target.BuildTrees.First();
                Assert.IsTrue(buildTree.ItemReference.IsAlive, "IsAlive returned an incorrect value");
                Assert.IsTrue(buildTree.NodeCreatedByContainer, "NodeCreatedByContainer returned an incorrect value");
                Assert.AreSame(instance, buildTree.ItemReference.Target, "Target returned an incorrect value");
                Assert.AreEqual(buildKey, buildTree.BuildKey, "BuildKey returned an incorrect value");
                Assert.IsNull(buildTree.Parent, "Parent returned an incorrect value");
                Assert.AreEqual(0, buildTree.Children.Count, "Count returned an incorrect value");
            }
        }

        /// <summary>
        /// Builds the tree tracker creates partial tree when create child fails with successful recovery on unique context test.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCreatesPartialTreeWhenCreateChildFailsWithSuccessfulRecoveryOnUniqueContextTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target = new BuildTreeTracker();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));
                Object firstInstance = Guid.NewGuid().ToString();

                // Assign details for first node into the context
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                Object secondInstance = Guid.NewGuid().ToString();

                // Assign details for second node into the context
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PreBuildUp(context);

                // Reset second tree node details
                context.Existing = secondInstance;
                target.PostBuildUp(context);

                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));
                IBuilderContext uniqueContext = MockRepository.GenerateStub<IBuilderContext>();

                // Assign details for third node into the context
                uniqueContext.BuildKey = thirdBuildKey;
                uniqueContext.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                uniqueContext.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                target.PreBuildUp(uniqueContext);

                // Reset the third node details
                uniqueContext.RecoveryStack.ExecuteRecovery();

                // Reset first tree node details
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PostBuildUp(context);

                BuildTreeItemNode buildTree = target.BuildTrees.First();
                Assert.AreSame(firstInstance, buildTree.ItemReference.Target, "Target returned an incorrect value");
                Assert.AreEqual(1, buildTree.Children.Count, "Count returned an incorrect value");
                Assert.AreSame(secondInstance, buildTree.Children[0].ItemReference.Target, "Target returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker creates tree with multiple nodes all with one child.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCreatesTreeWithMultipleNodesAllWithOneChildTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target = new BuildTreeTracker();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));
                Object firstInstance = Guid.NewGuid().ToString();

                // Assign details for first node into the context
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                Object secondInstance = Guid.NewGuid().ToString();

                // Assign details for second node into the context
                context.Existing = secondInstance;
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));
                Object thirdInstance = Guid.NewGuid().ToString();

                // Assign details for third node into the context
                context.BuildKey = thirdBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                target.PreBuildUp(context);

                // Start the post build up events
                context.Existing = thirdInstance;
                target.PostBuildUp(context);

                // Reset second tree node details
                context.Existing = secondInstance;
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PostBuildUp(context);

                // Reset first tree node details
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PostBuildUp(context);

                BuildTreeItemNode buildTree = target.BuildTrees.First();
                Assert.AreSame(firstInstance, buildTree.ItemReference.Target, "Target returned an incorrect value");
                Assert.AreSame(secondInstance, buildTree.Children[0].ItemReference.Target, "Target returned an incorrect value");
                Assert.AreSame(thirdInstance, buildTree.Children[0].Children[0].ItemReference.Target, "Target returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker creates tree with multiple nodes with multiple children.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCreatesTreeWithMultipleNodesWithMultipleChildrenTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target = new BuildTreeTracker();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));
                Object firstInstance = Guid.NewGuid().ToString();

                // Assign details for first node into the context
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                Object secondInstance = Guid.NewGuid().ToString();

                // Assign details for second node into the context
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PreBuildUp(context);

                // Reset second tree node details
                context.Existing = secondInstance;
                target.PostBuildUp(context);

                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));
                Object thirdInstance = Guid.NewGuid().ToString();

                // Assign details for third node into the context
                context.BuildKey = thirdBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                target.PreBuildUp(context);

                // Reset the third node details
                context.Existing = thirdInstance;
                target.PostBuildUp(context);

                // Reset first tree node details
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PostBuildUp(context);

                BuildTreeItemNode buildTree = target.BuildTrees.First();
                Assert.AreSame(firstInstance, buildTree.ItemReference.Target, "Target returned an incorrect value");
                Assert.AreSame(secondInstance, buildTree.Children[0].ItemReference.Target, "Target returned an incorrect value");
                Assert.AreSame(thirdInstance, buildTree.Children[1].ItemReference.Target, "Target returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker creates tree with single node.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerCreatesTreeWithSingleNodeTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);

                Assert.AreSame(instance, target.BuildTrees.First().ItemReference.Target, "Target returned an incorrect value");
            }
        }

        /// <summary>
        /// Builds the tree tracker disposes and removes single node when create fails test.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDisposesAndRemovesSingleNodeWhenCreateFailsTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                context.RecoveryStack.ExecuteRecovery();

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
            }
        }

        /// <summary>
        /// Builds the tree tracker disposes multiple nodes when create fails test.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDisposesCreatedNodesWhenCreateFailsTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target = new BuildTreeTracker();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));

                // Assign details for first node into the context
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable secondInstance = MockRepository.GenerateStub<IDisposable>();

                // Assign details for second node into the context
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PreBuildUp(context);

                // Reset second tree node details
                context.Existing = secondInstance;
                target.PostBuildUp(context);

                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));

                // Assign details for third node into the context
                context.BuildKey = thirdBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                target.PreBuildUp(context);

                context.RecoveryStack.ExecuteRecovery();

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
                secondInstance.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker disposes full tree when no nodes in lifetime manager.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDisposesFullTreeWhenNoNodesInLifetimeManagerTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                BuildTreeTracker target = new BuildTreeTracker();

                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable firstInstance = MockRepository.GenerateStub<IDisposable>();

                // Assign details for first node into the context
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable secondInstance = MockRepository.GenerateStub<IDisposable>();

                // Assign details for second node into the context
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);

                target.PreBuildUp(context);

                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable thirdInstance = MockRepository.GenerateStub<IDisposable>();

                // Assign details for third node into the context
                context.BuildKey = thirdBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                target.PreBuildUp(context);

                // Start the post build up events
                context.Existing = thirdInstance;
                target.PostBuildUp(context);

                // Reset second tree node details
                context.Existing = secondInstance;
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PostBuildUp(context);

                // Reset first tree node details
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PostBuildUp(context);

                target.PostTearDown(context);

                firstInstance.AssertWasCalled(x => x.Dispose());
                secondInstance.AssertWasCalled(x => x.Dispose());
                thirdInstance.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker disposes partial tree when child is in lifetime manager.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDisposesPartialTreeWhenChildIsInLifetimeManagerTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target;
            IDisposable thirdInstance;
            IDisposable secondInstance;

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                target = new BuildTreeTracker();

                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                // Assign details for first node into the context
                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                // Assign details for second node into the context
                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);

                target.PreBuildUp(context);

                // Assign details for third node into the context
                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));
                context.BuildKey = thirdBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                target.PreBuildUp(context);

                // Start the post build up events
                thirdInstance = MockRepository.GenerateStub<IDisposable>();
                context.Existing = thirdInstance;
                target.PostBuildUp(context);

                // Reset second tree node details
                secondInstance = MockRepository.GenerateStub<IDisposable>();
                ILifetimePolicy policy = MockRepository.GenerateStub<ILifetimePolicy>();

                policy.Stub(x => x.GetValue()).Return(secondInstance);

                lifetimeContainer.Add(policy);

                context.Existing = secondInstance;
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PostBuildUp(context);

                // Reset first tree node details
                IDisposable firstInstance = MockRepository.GenerateStub<IDisposable>();
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PostBuildUp(context);

                target.PostTearDown(context);

                firstInstance.AssertWasCalled(x => x.Dispose());
                secondInstance.AssertWasNotCalled(x => x.Dispose());
                thirdInstance.AssertWasNotCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker disposes tree skipping root node when not created by container.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDisposesTreeSkippingRootNodeWhenNotCreatedByContainerTest()
        {
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                BuildTreeTracker target = new BuildTreeTracker();

                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                NamedTypeBuildKey firstBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable firstInstance = MockRepository.GenerateStub<IDisposable>();

                // Assign details for first node into the context
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PreBuildUp(context);

                NamedTypeBuildKey secondBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable secondInstance = MockRepository.GenerateStub<IDisposable>();

                context.Existing = null;

                // Assign details for second node into the context
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);

                target.PreBuildUp(context);

                NamedTypeBuildKey thirdBuildKey = new NamedTypeBuildKey(typeof(String));
                IDisposable thirdInstance = MockRepository.GenerateStub<IDisposable>();

                // Assign details for third node into the context
                context.BuildKey = thirdBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(thirdBuildKey);
                target.PreBuildUp(context);

                // Start the post build up events
                context.Existing = thirdInstance;
                target.PostBuildUp(context);

                // Reset second tree node details
                context.Existing = secondInstance;
                context.BuildKey = secondBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(secondBuildKey);
                target.PostBuildUp(context);

                // Reset first tree node details
                context.Existing = firstInstance;
                context.BuildKey = firstBuildKey;
                context.Stub(x => x.OriginalBuildKey).Return(firstBuildKey);
                target.PostBuildUp(context);

                target.PostTearDown(context);

                firstInstance.AssertWasNotCalled(x => x.Dispose());
                secondInstance.AssertWasCalled(x => x.Dispose());
                thirdInstance.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker disposes tree when node is not in lifetime manager.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDisposesTreeWhenNodeIsNotInLifetimeManagerTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            IDisposable instance = MockRepository.GenerateStub<IDisposable>();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(new NamedTypeBuildKey(typeof(IDisposable)));
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);
                target.PostTearDown(context);

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
                instance.AssertWasCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker does not dispose instance when no build tree found.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDoesNotDisposeInstanceWhenNoBuildTreeFoundTest()
        {
            BuildTreeTracker target = new BuildTreeTracker();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            IDisposable instance = MockRepository.GenerateStub<IDisposable>();

            context.Existing = instance;
            context.BuildKey = new NamedTypeBuildKey(instance.GetType());
            context.Stub(x => x.OriginalBuildKey).Return(context.BuildKey);
            context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

            target.PostTearDown(context);

            instance.AssertWasNotCalled(x => x.Dispose());
        }

        /// <summary>
        /// Runs test for build tree tracker does not dispose tree when root node in lifetime manager.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerDoesNotDisposeTreeWhenRootNodeInLifetimeManagerTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            IDisposable instance = MockRepository.GenerateStub<IDisposable>();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                ILifetimePolicy policy = MockRepository.GenerateStub<ILifetimePolicy>();

                lifetimeContainer.Add(policy);

                policy.Stub(x => x.GetValue()).Return(instance);

                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);
                target.PostTearDown(context);

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
                instance.AssertWasNotCalled(x => x.Dispose());
            }
        }

        /// <summary>
        /// Runs test for build tree tracker ignores non disposable nodes.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerIgnoresNonDisposableNodesTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            String instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);
                target.PostTearDown(context);

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker removes dead trees from list when other tree is destroyed.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerRemovesDeadTreesFromListWhenOtherTreeIsDestroyedTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode buildTree = new BuildTreeItemNode(buildKey, false, null);
            BuildTreeItemNode deadBuildTree = new BuildTreeItemNode(buildKey, false, null);
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.Existing = instance;
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());
                buildTree.AssignInstance(instance);

                // Assign an instance that is not rooted to this method so it can be garbage collected
                deadBuildTree.AssignInstance(Guid.NewGuid().ToString());

                IBuildTreeStore store = new BuildTreeStore();
                BuildTreeTracker target = new BuildTreeTracker(store);

                store.Add(buildTree);
                store.Add(deadBuildTree);

                GC.Collect();

                // Ensure that the first tree is still rooted to this method and the second is not
                Assert.IsTrue(buildTree.ItemReference.IsAlive, "First tree node reference has died");
                Assert.IsFalse(deadBuildTree.ItemReference.IsAlive, "Second tree node reference is still alive");

                target.PostTearDown(context);

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker removes tree from list when tree is destroyed.
        /// </summary>
        [TestMethod]
        public void BuildTreeTrackerRemovesTreeFromListWhenTreeIsDestroyedTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();

            using (LifetimeContainer lifetimeContainer = new LifetimeContainer())
            {
                context.Stub(x => x.Lifetime).Return(lifetimeContainer);
                context.BuildKey = buildKey;
                context.Stub(x => x.OriginalBuildKey).Return(buildKey);
                context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

                BuildTreeTracker target = new BuildTreeTracker();

                target.PreBuildUp(context);

                context.Existing = instance;

                target.PostBuildUp(context);
                target.PostTearDown(context);

                Assert.AreEqual(0, target.BuildTrees.Count(), "Count returned an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for build tree tracker throws exception when assigning an instance to the current node for an incorrect build key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildTreeTrackerThrowsExceptionWhenAssigningAnInstanceToTheCurrentNodeForAnIncorrectBuildKeyTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            NamedTypeBuildKey incorrectBuildKey = new NamedTypeBuildKey(typeof(Guid));
            Object instance = Guid.NewGuid().ToString();
            IBuilderContext context = MockRepository.GenerateStub<IBuilderContext>();
            BuildTreeTracker target = new BuildTreeTracker();

            context.BuildKey = buildKey;
            context.Stub(x => x.OriginalBuildKey).Return(buildKey);
            context.Stub(x => x.RecoveryStack).Return(new RecoveryStack());

            target.PreBuildUp(context);

            IBuilderContext incorrectContext = MockRepository.GenerateStub<IBuilderContext>();

            incorrectContext.BuildKey = incorrectBuildKey;
            incorrectContext.Stub(x => x.OriginalBuildKey).Return(incorrectBuildKey);
            incorrectContext.Existing = instance;

            target.PostBuildUp(incorrectContext);
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