namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// The <see cref="ConfigurationResolver"/>
    ///   class is used to resolve a collection of 
    ///   <see cref="TraceSource"/> instances from application configuration.
    /// </summary>
    public class ConfigurationResolver : ITraceSourceResolver
    {
        /// <summary>
        ///   Stores the lock used to reference the <see cref = "TraceSourceNames" /> property.
        /// </summary>
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Reloads the <see cref="TraceSource"/> names and instances available from the resolver.
        /// </summary>
        public void Reload()
        {
            using (new LockWriter(_lock))
            {
                TraceSourceNames = null;
            }
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
            // Get the configured names
            Collection<String> names = ResolveNames();

            // Loop through each name
            for (Int32 index = 0; index < names.Count; index++)
            {
                String configurationName = names[index];

                if (configurationName.Equals(name, comparison))
                {
                    return new TraceSource(configurationName);
                }
            }

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
            using (new LockReader(_lock, true))
            {
                if (TraceSourceNames != null)
                {
                    return TraceSourceNames;
                }

                using (new LockWriter(_lock))
                {
                    // Check to see if another write lock has already calculated the names required
                    if (TraceSourceNames == null)
                    {
                        // Get the values from configuration
                        TraceSourceNames = ResolveFromConfiguration();
                    }

                    // Open the configuration for the calling application
                    return TraceSourceNames;
                }
            }
        }

        /// <summary>
        /// Resolves the source names from configuration.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        private static Collection<String> ResolveFromConfiguration()
        {
            IConfigurationStore store = ConfigurationStoreFactory.Create();
            ConfigurationSection section = store.GetSection<ConfigurationSection>("system.diagnostics");

            return ResolveFromSection(section);
        }

        /// <summary>
        /// Resolves the source names from the section.
        /// </summary>
        /// <param name="section">
        /// The section.
        /// </param>
        /// <returns>
        /// A <see cref="Collection{String}"/> instance.
        /// </returns>
        private static Collection<String> ResolveFromSection(ConfigurationSection section)
        {
            Collection<String> sources = new Collection<String>();

            // Check if there is no section available
            if (section == null)
            {
                // Return the empty collection
                return sources;
            }

            PropertyInfo sourceProperty = section.GetType().GetProperty("Sources");

            Debug.Assert(sourceProperty != null, "The Sources property doesn't exist on SystemDiagnosticsSection");

            ConfigurationElementCollection configuredSources = sourceProperty.GetValue(section, null) as ConfigurationElementCollection;

            if (configuredSources == null)
            {
                // Return the empty collection
                return sources;
            }

            if (configuredSources.Count == 0)
            {
                // Return the empty collection
                return sources;
            }

            PropertyInfo nameProperty = null;

            // The implementation of ConfigurationElementCollection.CopyTo does a foreach.
            // Copying the elements into an array in order to do a for loop is therefore more expensive
            // as a foreach and a for is being executed. The reason for this use of lower performance code
            // is to achieve more accurate code coverage in testing. This is balanced with the expectation that 
            // this method is most likely going to be invoked only once in the lifetime of an application domain
            // and the number of items in the collection is normally going to be small.
            // As this is not a hot code path, more accurate testing results was chosen over pure runtime performance.
            ConfigurationElement[] elements = new ConfigurationElement[configuredSources.Count];
            configuredSources.CopyTo(elements, 0);

            // Loop through each configured source
            for (Int32 index = 0; index < elements.Length; index++)
            {
                ConfigurationElement sourceElement = elements[index];

                // Check if we have the name property reference
                if (nameProperty == null)
                {
                    // Get the Name property reference
                    nameProperty = sourceElement.GetType().GetProperty("Name");

                    Debug.Assert(nameProperty != null, "The Name property doesn't exist on SourceElement.");
                }

                // Get the name of the source
                String name = nameProperty.GetValue(sourceElement, null) as String;

                // Check if there is a source name
                if (String.IsNullOrEmpty(name) == false)
                {
                    // Add the source name to the collection
                    sources.Add(name);
                }
            }

            // Return the sources
            return sources;
        }

        /// <summary>
        ///   Gets or sets the trace source names.
        /// </summary>
        /// <value>
        ///   The trace source names.
        /// </value>
        private static Collection<String> TraceSourceNames
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
            get
            {
                // This resolver doesn't support child resolvers
                return null;
            }
        }
    }
}