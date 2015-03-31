namespace Neovolve.Toolkit.LoadTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Instrumentation;

    /// <summary>
    /// The <see cref="TestTraceSourceResolver"/>
    ///   class is used to assist with unit testing the instrumentation namespace.
    /// </summary>
    internal class TestTraceSourceResolver : ITraceSourceResolver
    {
        /// <summary>
        ///   Defines the name of the test trace source.
        /// </summary>
        private const String SourceName = "Neovolve";

        /// <summary>
        ///   Stores the trace source names.
        /// </summary>
        private static readonly Collection<String> _names;

        /// <summary>
        ///   Defines the test trace source.
        /// </summary>
        private static readonly TraceSource _source;

        /// <summary>
        ///   Initializes static members of the <see cref = "TestTraceSourceResolver" /> class.
        /// </summary>
        static TestTraceSourceResolver()
        {
            // Configure the source
            _source = new TraceSource(SourceName);
            _source.Listeners.Add(new TestTraceListener());
            _source.Switch.Level = SourceLevels.ActivityTracing | SourceLevels.All;

            _names = new Collection<String>
                     {
                         SourceName
                     };
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TestTraceSourceResolver" /> class.
        /// </summary>
        public TestTraceSourceResolver()
        {
            ChildResolver = null;
        }

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
            return _source;
        }

        /// <summary>
        /// Resolves the available <see cref="TraceSource"/> names.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        public Collection<String> ResolveNames()
        {
            return _names;
        }

        /// <summary>
        ///   Gets the child resolver.
        /// </summary>
        /// <value>
        ///   The child resolver or <c>null</c> if there is no child resolver.
        /// </value>
        public ITraceSourceResolver ChildResolver
        {
            get;
            private set;
        }
    }
}