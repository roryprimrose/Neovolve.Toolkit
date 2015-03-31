namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="BuildTreeStoreContracts"/>
    ///   class is used to define the contracts for the <see cref="IBuildTreeStore"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IBuildTreeStore))]
    internal abstract class BuildTreeStoreContracts : IBuildTreeStore
    {
        /// <summary>
        /// Adds the specified build tree.
        /// </summary>
        /// <param name="buildTree">
        /// The build tree.
        /// </param>
        public void Add(BuildTreeItemNode buildTree)
        {
            Contract.Requires<ArgumentNullException>(buildTree != null);
            Contract.Requires<ArgumentNullException>(buildTree.ItemReference != null);
            Contract.Requires<ArgumentNullException>(buildTree.ItemReference.IsAlive);
        }

        /// <summary>
        /// Gets the build tree for instance.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A <see cref="BuildTreeItemNode"/> instance.
        /// </returns>
        public BuildTreeItemNode GetBuildTreeForInstance(Object instance)
        {
            return null;
        }

        /// <summary>
        /// Gets the build trees.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> instance.
        /// </returns>
        public IEnumerable<BuildTreeItemNode> GetBuildTrees()
        {
            Contract.Ensures(Contract.Result<IEnumerable<BuildTreeItemNode>>() != null);

            return null;
        }

        /// <summary>
        /// Removes the specified build tree.
        /// </summary>
        /// <param name="buildTree">
        /// The build tree.
        /// </param>
        public void Remove(BuildTreeItemNode buildTree)
        {
        }
    }
}