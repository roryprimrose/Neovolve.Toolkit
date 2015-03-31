namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Globalization;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="TraceSourceResolverFactory"/>
    ///   class is used to create an instance of a <see cref="ITraceSourceResolver"/>.
    /// </summary>
    public static class TraceSourceResolverFactory
    {
        /// <summary>
        ///   Stores the <see cref = "ResolverType" /> value.
        /// </summary>
        private static Type _resolverType;

        /// <summary>
        /// Creates a <see cref="ITraceSourceResolver"/> instance.
        /// </summary>
        /// <returns>
        /// A <see cref="ITraceSourceResolver"/> instance.
        /// </returns>
        public static ITraceSourceResolver Create()
        {
            return (ITraceSourceResolver)Activator.CreateInstance(ResolverType);
        }

        /// <summary>
        ///   Gets or sets the type of the resolver.
        /// </summary>
        /// <value>
        ///   The type of the resolver.
        /// </value>
        public static Type ResolverType
        {
            get
            {
                if (_resolverType == null)
                {
                    // Use the ConfigurationResolver as the default type
                    _resolverType = typeof(ConfigurationResolver);
                }

                return _resolverType;
            }

            set
            {
                if (value != null)
                {
                    if (typeof(ITraceSourceResolver).IsAssignableFrom(value) == false)
                    {
                        throw new InvalidCastException(
                            String.Format(
                                CultureInfo.InvariantCulture, Resources.InvalidCastExceptionMessage, value.GetType(), typeof(ITraceSourceResolver)));
                    }
                }

                _resolverType = value;
            }
        }
    }
}