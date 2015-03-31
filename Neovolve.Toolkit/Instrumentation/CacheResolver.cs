namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="CacheResolver"/>
    ///   class is used to cache a set of 
    ///   <see cref="TraceSource"/> instances.
    /// </summary>
    internal class CacheResolver : ITraceSourceResolver
    {
        /// <summary>
        ///   Defines the default cache expiry length for <see cref = "TraceSource" /> instances.
        /// </summary>
        public const Int64 TraceSourceCacheExpiryDefaultLength = 60000;

        /// <summary>
        ///   Defines the configuration key used to load the value for the <see cref = "TraceSourceCacheExpiryLength" /> property.
        /// </summary>
        public const String TraceSourceCacheExpiryLengthKey = "CacheResolver.TraceSourceCacheExpiryLength";

        /// <summary>
        ///   Defines the default cache expiry length for trace source names.
        /// </summary>
        public const Int64 TraceSourceNameCacheExpiryDefaultLength = 1200000;

        /// <summary>
        ///   Defines  the configuration key used to load the value for the <see cref = "TraceSourceNameCacheExpiryLength" /> property.
        /// </summary>
        public const String TraceSourceNameCacheExpiryLengthKey = "CacheResolver.TraceSourceNameCacheExpiryLength";

        /// <summary>
        ///   Defines the cache key prefix used to avoid cache key collisions.
        /// </summary>
        private const String TraceSourceCacheKeyPrefix = "CTSM.SourcePrefix";

        /// <summary>
        ///   Defines the cache key used to store the list of trace source names.
        /// </summary>
        private const String TraceSourceNamesCacheKeyPrefix = "CTSM.NamesPrefix";

        /// <summary>
        ///   Stores the cache used to store <see cref = "RuntimeMethodHandle" /> values.
        /// </summary>
        private static readonly ICacheStore _cacheStore = CacheStoreFactory.Create();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheResolver"/> class.
        /// </summary>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="resolver"/> value is <c>null</c>.
        /// </exception>
        public CacheResolver(ITraceSourceResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);

            // Store the resolver
            ChildResolver = resolver;

            IConfigurationStore configStore = ConfigurationStoreFactory.Create();

            TraceSourceCacheExpiryLength = configStore.GetApplicationSetting(TraceSourceCacheExpiryLengthKey, TraceSourceCacheExpiryDefaultLength);
            TraceSourceNameCacheExpiryLength = configStore.GetApplicationSetting(
                TraceSourceNameCacheExpiryLengthKey, TraceSourceNameCacheExpiryDefaultLength);
        }

        /// <summary>
        /// Builds the cache policies.
        /// </summary>
        /// <returns>
        /// A <see cref="IExpirationPolicy"/> array instance.
        /// </returns>
        public IExpirationPolicy[] BuildCachePolicies()
        {
            TimeSpan policyLength = TimeSpan.FromMilliseconds(TraceSourceCacheExpiryLength);
            SlidingExpirationPolicy expirationPolicy = new SlidingExpirationPolicy(policyLength);

            return new[]
                   {
                       expirationPolicy
                   };
        }

        /// <summary>
        /// Reloads the <see cref="TraceSource"/> names and instances available from the resolver.
        /// </summary>
        public void Reload()
        {
            String namesCacheKey = GenerateNamesCacheKey();

            // Remove the trace source names from the cache
            _cacheStore.Remove(namesCacheKey);

            String partialSourceCacheKey = GeneratePartialTraceSourceCacheKey();

            IEnumerable<String> keysToRemove = from x in _cacheStore.Keys
                                               where x.StartsWith(partialSourceCacheKey, StringComparison.Ordinal)
                                               select x;

            keysToRemove.ToList().ForEach(x => _cacheStore.Remove(x));

            // Call the stored resolver to reload itself
            ChildResolver.Reload();
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
            String cacheKey = GenerateTraceSourceCacheKey(name);

            return _cacheStore.GetItem(cacheKey, () => ChildResolver.Resolve(name, StringComparison.OrdinalIgnoreCase), BuildCachePolicies);
        }

        /// <summary>
        /// Gets the trace source names.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        public Collection<String> ResolveNames()
        {
            String namesCacheKey = GenerateNamesCacheKey();

            return _cacheStore.GetItem(namesCacheKey, () => ChildResolver.ResolveNames());
        }

        /// <summary>
        /// Generates the names cache key.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private String GenerateNamesCacheKey()
        {
            Contract.Ensures(String.IsNullOrWhiteSpace(Contract.Result<String>()) == false);

            String resolverType = GetChildResolverType();

            // The cache key for the trace source names is the combination of the key prefix and the resolver type name
            String cacheKey = TraceSourceNamesCacheKeyPrefix + Path.PathSeparator + resolverType;

            Contract.Assume(String.IsNullOrWhiteSpace(cacheKey) == false);

            return cacheKey;
        }

        /// <summary>
        /// Generates the partial cache key for trace sources.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private String GeneratePartialTraceSourceCacheKey()
        {
            Contract.Ensures(String.IsNullOrWhiteSpace(Contract.Result<String>()) == false);

            String resolverType = GetChildResolverType();

            // The cache key for this trace source name is the combination of the key prefix, the resolver type name and the trace source name
            String cacheKey = TraceSourceCacheKeyPrefix + Path.PathSeparator + resolverType;

            Contract.Assume(String.IsNullOrWhiteSpace(cacheKey) == false);

            return cacheKey;
        }

        /// <summary>
        /// Generates the cache key for a trace source name.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private String GenerateTraceSourceCacheKey(String traceSourceName)
        {
            Contract.Ensures(String.IsNullOrWhiteSpace(Contract.Result<String>()) == false);

            // Make the name uppercase in order to cover cases where the same name is used with different casing
            String name = traceSourceName.ToUpperInvariant();

            // Get the initial part of the key
            String partialKey = GeneratePartialTraceSourceCacheKey();

            // The cache key for this trace source name is the combination of the key prefix, the resolver type name and the trace source name
            String cacheKey = partialKey + Path.PathSeparator + name;

            Contract.Assume(String.IsNullOrWhiteSpace(cacheKey) == false);

            return cacheKey;
        }

        /// <summary>
        /// Gets the type of the child resolver.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private String GetChildResolverType()
        {
            Contract.Ensures(String.IsNullOrWhiteSpace(Contract.Result<String>()) == false);

            String resolverTypeName = ChildResolver.GetType().AssemblyQualifiedName;

            Contract.Assume(String.IsNullOrWhiteSpace(resolverTypeName) == false);

            return resolverTypeName;
        }

        /// <summary>
        ///   Gets or sets the length of the trace source cache expiry.
        /// </summary>
        /// <value>
        ///   The length of the trace source cache expiry.
        /// </value>
        public static Int64 TraceSourceCacheExpiryLength
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the length of the trace source name cache expiry.
        /// </summary>
        /// <value>
        ///   The length of the trace source name cache expiry.
        /// </value>
        public static Int64 TraceSourceNameCacheExpiryLength
        {
            get;
            set;
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