namespace Neovolve.Toolkit.Reflection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="MethodResolver"/>
    ///   class resolves <see cref="MethodInfo"/>
    ///   instances of types and caches results for faster access.
    /// </summary>
    public static class MethodResolver
    {
        /// <summary>
        ///   Stores the cache used to store <see cref = "RuntimeMethodHandle" /> values.
        /// </summary>
        private static readonly ICacheStore _cacheStore = CacheStoreFactory.Create();

        /// <summary>
        /// Resolves the specified declaring type.
        /// </summary>
        /// <param name="declaringType">
        /// Type of the declaring.
        /// </param>
        /// <param name="methodName">
        /// Name of the method.
        /// </param>
        /// <param name="parameterTypes">
        /// The parameter types.
        /// </param>
        /// <returns>
        /// A <see cref="MethodInfo"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="declaringType"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="methodName"/> is <c>null</c> or equals <see cref="String.Empty"/>.
        /// </exception>
        public static MethodInfo Resolve(Type declaringType, String methodName, params Type[] parameterTypes)
        {
            Contract.Requires<ArgumentNullException>(declaringType != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(methodName) == false);

            MethodCacheKey cacheKey = new MethodCacheKey(declaringType, methodName, parameterTypes);
            String cacheKeyValue = cacheKey.ToString();

            Contract.Assume(String.IsNullOrWhiteSpace(cacheKeyValue) == false);

            RuntimeMethodHandle handle = _cacheStore.GetItem(cacheKeyValue, () => GetResolvedMethod(cacheKey));

            return MethodBase.GetMethodFromHandle(handle) as MethodInfo;
        }

        /// <summary>
        /// Gets the resolved method handle.
        /// </summary>
        /// <param name="cacheKey">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <see cref="RuntimeMethodHandle"/> instance.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// The method was not found.
        /// </exception>
        private static RuntimeMethodHandle GetResolvedMethod(MethodCacheKey cacheKey)
        {
            const BindingFlags Flags =
                BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod |
                BindingFlags.Public | BindingFlags.NonPublic;

            MethodInfo resolvedMethod = cacheKey.DeclaringType.GetMethod(cacheKey.MethodName, Flags, null, cacheKey.ParameterTypes.ToArray(), null);

            if (resolvedMethod == null)
            {
                // If the declaring type is an interface, the method being sought may not be found as interfaces implement interfaces
                if (cacheKey.DeclaringType.IsInterface)
                {
                    Type[] interfaces = cacheKey.DeclaringType.GetInterfaces();

                    for (Int32 index = 0; index < interfaces.Length; index++)
                    {
                        Type interfaceType = interfaces[index];

                        resolvedMethod = interfaceType.GetMethod(cacheKey.MethodName, Flags, null, cacheKey.ParameterTypes.ToArray(), null);

                        if (resolvedMethod != null)
                        {
                            break;
                        }
                    }
                }
            }

            if (resolvedMethod == null)
            {
                throw new MissingMethodException(cacheKey.DeclaringType.FullName, cacheKey.MethodName);
            }

            return resolvedMethod.MethodHandle;
        }
    }
}