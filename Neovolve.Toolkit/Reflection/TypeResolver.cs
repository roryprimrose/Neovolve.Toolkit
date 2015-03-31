namespace Neovolve.Toolkit.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using Neovolve.Toolkit.Properties;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="TypeResolver"/>
    ///   class is used to resolve types from configuration mapping information.
    /// </summary>
    [Obsolete("The TypeResolver class promotes a service locator pattern that is both confusing and undesirable.", false)]
    public static class TypeResolver
    {
        /// <summary>
        ///   Stores the configuration store reference.
        /// </summary>
        private static IConfigurationStore _configuration;

        /// <summary>
        ///   Stores the mappings of type names and configuration keys to the assembly qualified type name.
        /// </summary>
        private static ICacheStore _mappingStore;

        /// <summary>
        ///   Stores the type handles for assembly qualified type names.
        /// </summary>
        private static ICacheStore _typeHandleStore;

        /// <summary>
        /// Determines whether a type can be resolved from the specified source type.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <returns>
        /// <c>true</c>if a type can be resolved from the source type; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean CanResolveType(Type sourceType)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);

            Type resolvedType = ResolveInternal(sourceType, false);

            if (IsValidType(resolvedType, sourceType) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether a type can be resolved from the specified source type and configuration key.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="configurationKey">
        /// The configuration key used to load the type.
        /// </param>
        /// <returns>
        /// <c>true</c>if a type can be resolved from the source type and configuration key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The 
        ///   <paramref name="configurationKey"/>
        ///   parameter is <c>null</c> or equals <see cref="String.Empty">String.Empty</see>.
        /// </exception>
        public static Boolean CanResolveTypeFromKey(Type sourceType, String configurationKey)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(configurationKey) == false);

            Type resolvedType = ResolveFromKeyInternal(sourceType, configurationKey, false);

            if (IsValidType(resolvedType, sourceType) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates an instance of a type resolved from configuration using the specified type parameter.
        /// </summary>
        /// <typeparam name="T">
        /// The source type used to determine the type to create.
        /// </typeparam>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        /// <remarks>
        /// The type loaded from configuration must contain a parameterless constructor
        ///   in order for an instance to be created.
        /// </remarks>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// The loaded type does not contain a parameterless constructor and an instance can not be created.
        /// </exception>
        public static T Create<T>()
        {
            Type targetType = ResolveInternal(typeof(T), true);

            if (targetType == null)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(targetType);
        }

        /// <summary>
        /// Creates an instance of a type resolved from configuration using the specified type parameter and configuration key.
        /// </summary>
        /// <typeparam name="T">
        /// The source type used to determine the type to create.
        /// </typeparam>
        /// <param name="configurationKey">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        /// <remarks>
        /// The type loaded from configuration must contain a parameterless constructor
        ///   in order for an instance to be created.
        /// </remarks>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// The loaded type does not contain a parameterless constructor and an instance can not be created.
        /// </exception>
        public static T CreateFromKey<T>(String configurationKey)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(configurationKey) == false);

            Type targetType = ResolveFromKeyInternal(typeof(T), configurationKey, true);

            if (targetType == null)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(targetType);
        }

        /// <summary>
        /// Resolves a type from configuration using the specified source type.
        /// </summary>
        /// <param name="sourceType">
        /// The source Type.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        public static Type Resolve(Type sourceType)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);

            return ResolveInternal(sourceType, true);
        }

        /// <summary>
        /// Resolves a type from configuration using the specified source type and configuration key.
        /// </summary>
        /// <param name="sourceType">
        /// The source Type.
        /// </param>
        /// <param name="configurationKey">
        /// The configuration key.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The 
        ///   <paramref name="configurationKey"/>
        ///   parameter is <c>null</c> or equals <see cref="String.Empty">String.Empty</see>.
        /// </exception>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        public static Type ResolveFromKey(Type sourceType, String configurationKey)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(configurationKey) == false);

            return ResolveFromKeyInternal(sourceType, configurationKey, true);
        }

        /// <summary>
        /// Gets the partially qualified name of the type.
        /// </summary>
        /// <param name="evaluation">
        /// The evaluation.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private static String GetPartiallyQualifiedName(Type evaluation)
        {
            String assemblyPath = evaluation.Assembly.Location;

            if (String.IsNullOrWhiteSpace(assemblyPath))
            {
                return null;
            }

            String assemblyFileName = Path.GetFileNameWithoutExtension(assemblyPath);

            return String.Concat(evaluation.FullName, ", ", assemblyFileName);
        }

        /// <summary>
        /// Determines whether the resolved type is valid.
        /// </summary>
        /// <param name="resolvedType">
        /// The resolved type.
        /// </param>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <returns>
        /// <c>true</c>if the resolved type is valid; otherwise, <c>false</c>.
        /// </returns>
        private static Boolean IsValidType(Type resolvedType, Type sourceType)
        {
            if (resolvedType == null)
            {
                return false;
            }

            if (sourceType.IsAssignableFrom(resolvedType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the type using the specified type name.
        /// </summary>
        /// <param name="typeName">
        /// Name of the type.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance or <c>null</c> if no type was loaded.
        /// </returns>
        private static Type LoadType(String typeName)
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(typeName) == false);

            RuntimeTypeHandle handle = TypeHandleStore.GetItem(typeName, () => ResolveTypeHandle(typeName));

            if (default(RuntimeTypeHandle).Equals(handle))
            {
                return null;
            }

            return Type.GetTypeFromHandle(handle);
        }

        /// <summary>
        /// Resolves the configuration key that defines the target type to load for the specified source type.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <returns>
        /// A <see cref="KeyValuePair{TKey,TValue}"/> instance.
        /// </returns>
        private static KeyValuePair<String, String> ResolveConfigurationForType(Type sourceType)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);

            String configurationValue;

            if (String.IsNullOrWhiteSpace(sourceType.AssemblyQualifiedName) == false)
            {
                configurationValue = Configuration.GetApplicationSetting<String>(sourceType.AssemblyQualifiedName);

                if (String.IsNullOrEmpty(configurationValue) == false)
                {
                    return new KeyValuePair<String, String>(sourceType.AssemblyQualifiedName, configurationValue);
                }
            }

            String partiallyQualifiedName = GetPartiallyQualifiedName(sourceType);

            if (String.IsNullOrWhiteSpace(partiallyQualifiedName) == false)
            {
                configurationValue = Configuration.GetApplicationSetting<String>(partiallyQualifiedName);

                if (String.IsNullOrEmpty(configurationValue) == false)
                {
                    return new KeyValuePair<String, String>(partiallyQualifiedName, configurationValue);
                }
            }

            configurationValue = Configuration.GetApplicationSetting<String>(sourceType.FullName);

            if (String.IsNullOrEmpty(configurationValue) == false)
            {
                return new KeyValuePair<String, String>(sourceType.FullName, configurationValue);
            }

            return default(KeyValuePair<String, String>);
        }

        /// <summary>
        /// The internal implementation for resolving a target type from the specified source type and configuration key.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="configurationKey">
        /// The configuration key.
        /// </param>
        /// <param name="validateLoadedType">
        /// If set to <c>true</c>, the loaded type is validated.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        private static Type ResolveFromKeyInternal(Type sourceType, String configurationKey, Boolean validateLoadedType)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(configurationKey) == false);

            // Check if this type has already been discovered using the configuration key and has had its target type cached
            String targetTypeName = MappingStore.GetItem<String>(configurationKey);

            if (String.IsNullOrWhiteSpace(targetTypeName) == false)
            {
                // We have resolved from this source type and configuration key before and have cached the assembly qualified name of the target type
                return LoadType(targetTypeName);
            }

            Type resolvedType = null;
            String configurationValue = Configuration.GetApplicationSetting<String>(configurationKey);

            if (String.IsNullOrEmpty(configurationValue) == false)
            {
                resolvedType = LoadType(configurationValue);
            }

            if (validateLoadedType)
            {
                ValidateResolvedType(resolvedType, sourceType, configurationKey);
            }

            if (resolvedType != null)
            {
                // Check whether either the type has been validated (exception thrown in call to ValidateResolvedType if invalid) 
                // or the non-exception throwing validation checking passes
                if (validateLoadedType || IsValidType(resolvedType, sourceType))
                {
                    if (String.IsNullOrWhiteSpace(resolvedType.AssemblyQualifiedName) == false)
                    {
                        MappingStore.Add(configurationKey, resolvedType.AssemblyQualifiedName);
                    }
                }

                return resolvedType;
            }

            return null;
        }

        /// <summary>
        /// The internal implementation for resolving a target type from the specified source type.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="validateLoadedType">
        /// If set to <c>true</c>, the loaded type is validated.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="sourceType"/> parameter is <c>null</c>.
        /// </exception>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        private static Type ResolveInternal(Type sourceType, Boolean validateLoadedType)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);

            String assemblyQualifiedName = sourceType.AssemblyQualifiedName;

            if (String.IsNullOrWhiteSpace(assemblyQualifiedName))
            {
                return null;
            }

            // We have resolved from this source type before and have cached the assembly qualified name of the target type
            String targetTypeName = MappingStore.GetItem<String>(assemblyQualifiedName);

            if (String.IsNullOrWhiteSpace(targetTypeName) == false)
            {
                return LoadType(targetTypeName);
            }

            Type targetType = null;
            KeyValuePair<String, String> configuration = ResolveConfigurationForType(sourceType);

            if (String.IsNullOrWhiteSpace(configuration.Value) == false)
            {
                targetType = LoadType(configuration.Value);
            }

            if (validateLoadedType)
            {
                ValidateResolvedType(targetType, sourceType, configuration.Key);
            }

            if (targetType == null)
            {
                return null;
            }

            // Check whether either the type has been validated (exception thrown in call to ValidateResolvedType if invalid) 
            // or the non-exception throwing validation checking passes
            if (validateLoadedType || IsValidType(targetType, sourceType))
            {
                if (String.IsNullOrWhiteSpace(targetType.AssemblyQualifiedName) == false)
                {
                    MappingStore.Add(assemblyQualifiedName, targetType.AssemblyQualifiedName);
                }
            }

            return targetType;
        }

        /// <summary>
        /// Resolves the type handle.
        /// </summary>
        /// <param name="typeName">
        /// Name of the type.
        /// </param>
        /// <returns>
        /// A <see cref="RuntimeTypeHandle"/> instance.
        /// </returns>
        private static RuntimeTypeHandle ResolveTypeHandle(String typeName)
        {
            Type resolvedType = Type.GetType(typeName);

            if (resolvedType == null)
            {
                return default(RuntimeTypeHandle);
            }

            return resolvedType.TypeHandle;
        }

        /// <summary>
        /// Validates that the source type is source from the loaded type.
        /// </summary>
        /// <param name="resolvedType">
        /// The resolved type.
        /// </param>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="configurationKeyUsed">
        /// The configuration key used to resolve the type.
        /// </param>
        /// <exception cref="TypeLoadException">
        /// The type identified in configuration could not be loaded.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The source type is not assignable from the type loaded.
        /// </exception>
        private static void ValidateResolvedType(Type resolvedType, Type sourceType, String configurationKeyUsed)
        {
            if (resolvedType == null)
            {
                if (String.IsNullOrWhiteSpace(configurationKeyUsed))
                {
                    String message = String.Format(
                        CultureInfo.InvariantCulture, Resources.TypeResolver_TypeNotResolvedFromConfiguration, sourceType.AssemblyQualifiedName);

                    throw new TypeLoadException(message);
                }
                else
                {
                    String message = String.Format(
                        CultureInfo.InvariantCulture, 
                        Resources.TypeResolver_TypeNotResolvedFromConfigurationKey, 
                        sourceType.AssemblyQualifiedName, 
                        configurationKeyUsed);

                    throw new TypeLoadException(message);
                }
            }

            if (sourceType.IsAssignableFrom(resolvedType) == false)
            {
                String message = String.Format(
                    CultureInfo.InvariantCulture, 
                    Resources.InvalidTypeLoadFromConfigurationFailed, 
                    resolvedType.AssemblyQualifiedName, 
                    configurationKeyUsed, 
                    sourceType.AssemblyQualifiedName);

                throw new InvalidCastException(message);
            }
        }

        /// <summary>
        ///   Gets the configuration.
        /// </summary>
        /// <value>
        ///   The configuration.
        /// </value>
        private static IConfigurationStore Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = ConfigurationStoreFactory.Create();
                }

                return _configuration;
            }
        }

        /// <summary>
        ///   Gets the mappings of type names and configuration keys to the assembly qualified type name.
        /// </summary>
        /// <value>
        ///   The mapping store.
        /// </value>
        private static ICacheStore MappingStore
        {
            get
            {
                if (_mappingStore == null)
                {
                    _mappingStore = CacheStoreFactory.Create();
                }

                return _mappingStore;
            }
        }

        /// <summary>
        ///   Gets the type handles for assembly qualified type names.
        /// </summary>
        /// <value>
        ///   The type handle store.
        /// </value>
        private static ICacheStore TypeHandleStore
        {
            get
            {
                if (_typeHandleStore == null)
                {
                    _typeHandleStore = CacheStoreFactory.Create();
                }

                return _typeHandleStore;
            }
        }
    }
}