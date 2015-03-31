namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="TraceSourceResolverContracts"/>
    ///   class is used to define the code contracts for the <see cref="ITraceSourceResolver"/> interface.
    /// </summary>
    [ContractClassFor(typeof(ITraceSourceResolver))]
    internal abstract class TraceSourceResolverContracts : ITraceSourceResolver
    {
        /// <summary>
        /// Reloads the <see cref="TraceSource"/> names and instances available from the resolver.
        /// </summary>
        public void Reload()
        {
        }

        /// <summary>
        /// Resolves a <see cref="TraceSource"/> using the specified name and string comparison.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="TraceSource"/>.
        /// </param>
        /// <param name="comparison">
        /// The string comparison to apply.
        /// </param>
        /// <returns>
        /// A <see cref="TraceSource"/> instance or <c>null</c> if the name is not found.
        /// </returns>
        public TraceSource Resolve(String name, StringComparison comparison)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(name) == false);

            return null;
        }

        /// <summary>
        /// Resolves the available <see cref="TraceSource"/> names.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        public Collection<String> ResolveNames()
        {
            return null;
        }

        /// <summary>
        ///   Gets the child resolver.
        /// </summary>
        /// <value>
        ///   The child resolver or <c>null</c> if there is no child resolver.
        /// </value>
        public ITraceSourceResolver ChildResolver
        {
            get
            {
                return null;
            }
        }
    }
}