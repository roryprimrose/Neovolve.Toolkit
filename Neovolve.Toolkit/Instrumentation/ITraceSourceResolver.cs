namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ITraceSourceResolver"/>
    ///   interface is used to resolve a collection of
    ///   <see cref="TraceSource"/> instances.
    /// </summary>
    [ContractClass(typeof(TraceSourceResolverContracts))]
    public interface ITraceSourceResolver
    {
        /// <summary>
        /// Reloads the <see cref="TraceSource"/> names and instances available from the resolver.
        /// </summary>
        void Reload();

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
        TraceSource Resolve(String name, StringComparison comparison);

        /// <summary>
        /// Resolves the available <see cref="TraceSource"/> names.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        Collection<String> ResolveNames();

        /// <summary>
        ///   Gets the child resolver.
        /// </summary>
        /// <value>
        ///   The child resolver or <c>null</c> if there is no child resolver.
        /// </value>
        ITraceSourceResolver ChildResolver
        {
            get;
        }
    }
}