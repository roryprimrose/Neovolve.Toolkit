namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="ConfigurationResolverMockWrapper"/>
    ///   class is used to provide a concreate type that wraps a mock of
    ///   <see cref="ITraceSourceResolver"/>.
    /// </summary>
    public class ConfigurationResolverMockWrapper : ITraceSourceResolver
    {
        /// <summary>
        /// Reloads the <see cref="TraceSource"/> names and instances available from the resolver.
        /// </summary>
        public void Reload()
        {
            MockInstance.Reload();
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
            return MockInstance.Resolve(name, comparison);
        }

        /// <summary>
        /// Resolves the available <see cref="TraceSource"/> names.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        public Collection<String> ResolveNames()
        {
            return MockInstance.ResolveNames();
        }

        /// <summary>
        ///   Gets or sets the mock instance.
        /// </summary>
        /// <value>
        ///   The mock instance.
        /// </value>
        public static ITraceSourceResolver MockInstance
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets ChildResolver.
        /// </summary>
        /// <value>
        ///   The child resolver.
        /// </value>
        public ITraceSourceResolver ChildResolver
        {
            get
            {
                return MockInstance.ChildResolver;
            }
        }
    }
}