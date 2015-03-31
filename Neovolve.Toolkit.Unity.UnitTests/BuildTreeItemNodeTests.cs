namespace Neovolve.Toolkit.Unity.UnitTests
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="BuildTreeItemNodeTests"/>
    ///   class is used to test the <see cref="BuildTreeItemNode"/> class.
    /// </summary>
    [TestClass]
    public class BuildTreeItemNodeTests
    {
        /// <summary>
        /// Runs test for build tree item node can assign null instance.
        /// </summary>
        [TestMethod]
        public void BuildTreeItemNodeCanAssignNullInstanceTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode target = new BuildTreeItemNode(buildKey, false, null);
            String instance = null;

            target.AssignInstance(instance);

            Assert.IsNotNull(target.ItemReference, "ItemReference failed to return a value");
            Assert.AreSame(instance, target.ItemReference.Target, "Target returned an incorrect value");
        }

        /// <summary>
        /// Runs test for build tree item node returns construction values.
        /// </summary>
        [TestMethod]
        public void BuildTreeItemNodeReturnsConstructionValuesTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode parentNode = new BuildTreeItemNode(buildKey, false, null);
            BuildTreeItemNode target = new BuildTreeItemNode(buildKey, true, parentNode);

            Assert.AreEqual(buildKey, target.BuildKey, "BuildKey returned an incorrect value");
            Assert.IsTrue(target.NodeCreatedByContainer, "NodeCreatedByContainer returned an incorrect value");
            Assert.AreEqual(parentNode, target.Parent, "Parent returned an incorrect value");
            Assert.IsNotNull(target.Children, "Children collection was not created");
        }

        /// <summary>
        /// Runs test for build tree item node returns reference to instance.
        /// </summary>
        [TestMethod]
        public void BuildTreeItemNodeReturnsReferenceToInstanceTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode target = new BuildTreeItemNode(buildKey, false, null);
            String instance = Guid.NewGuid().ToString();

            target.AssignInstance(instance);

            Assert.IsNotNull(target.ItemReference, "ItemReference failed to return a value");
            Assert.AreSame(instance, target.ItemReference.Target, "Target returned an incorrect value");
        }

        /// <summary>
        /// Runs test for build tree item node throws exception when assigned instance when one is already assigned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildTreeItemNodeThrowsExceptionWhenAssignedInstanceWhenOneIsAlreadyAssignedTest()
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeof(String));
            BuildTreeItemNode target = new BuildTreeItemNode(buildKey, false, null);
            String instance = Guid.NewGuid().ToString();

            target.AssignInstance(instance);

            target.AssignInstance(Guid.NewGuid());
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