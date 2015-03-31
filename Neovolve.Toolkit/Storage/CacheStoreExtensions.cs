namespace Neovolve.Toolkit.Storage
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Threading;

    /// <summary>
    /// The <see cref="CacheStoreExtensions"/>
    ///   class is used to provide extension methods for the <see cref="ICacheStore"/> interface.
    /// </summary>
    public static class CacheStoreExtensions
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T">
        /// The type of item to return.
        /// </typeparam>
        /// <param name="store">
        /// The cache store.
        /// </param>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T GetItem<T>(this ICacheStore store, String key)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            return GetItem<T>(store, key, null);
        }

        /// <summary>
        /// Gets the item from the cache, adding it as necessary.
        /// </summary>
        /// <typeparam name="T">
        /// The type of item to return.
        /// </typeparam>
        /// <param name="store">
        /// The cache store.
        /// </param>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <param name="resolver">
        /// The resolver function.
        /// </param>
        /// <param name="policySet">
        /// The policy set.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        /// <exception cref="InvalidCastException">
        /// The item stored in the cache is not convertable to <typeparamref name="T"/>.
        /// </exception>
        public static T GetItem<T>(this ICacheStore store, String key, Func<T> resolver, params IExpirationPolicy[] policySet)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            Func<IExpirationPolicy[]> policyResolver = null;

            if (policySet != null)
            {
                if (policySet.Length > 0)
                {
                    policyResolver = () => policySet;
                }
            }

            return GetItem(store, key, resolver, policyResolver);
        }

        /// <summary>
        /// Gets the item from the cache, adding it as necessary.
        /// </summary>
        /// <typeparam name="T">
        /// The type of item to return.
        /// </typeparam>
        /// <param name="store">
        /// The cache store.
        /// </param>
        /// <param name="key">
        /// The cache key.
        /// </param>
        /// <param name="resolver">
        /// The resolver function.
        /// </param>
        /// <param name="expirationPolicies">
        /// The expiration policies.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        /// <exception cref="InvalidCastException">
        /// The item stored in the cache is not convertable to <typeparamref name="T"/>.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        public static T GetItem<T>(this ICacheStore store, String key, Func<T> resolver, Func<IExpirationPolicy[]> expirationPolicies)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(key) == false);

            Type outputType = typeof(T);

            if (store.Contains(key))
            {
                Object storedValue = store.GetItem(key);

                if (storedValue is T)
                {
                    return (T)storedValue;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(outputType);

                if (converter != null)
                {
                    if (converter.CanConvertFrom(storedValue.GetType()))
                    {
                        return (T)converter.ConvertFrom(storedValue);
                    }
                }

                // Attempt a forced cast and let the CLR throw the InvalidCastException
                return (T)Convert.ChangeType(storedValue, outputType, Thread.CurrentThread.CurrentCulture);
            }

            if (resolver == null)
            {
                return default(T);
            }

            // The item is not stored in the cache
            T resolvedValue = resolver();
            Object valueToStore = ConvertToStorageType(resolvedValue);

            if (valueToStore != null)
            {
                IExpirationPolicy[] policySet = null;

                if (expirationPolicies != null)
                {
                    policySet = expirationPolicies();
                }

                store.Add(key, valueToStore, policySet);
            }

            return resolvedValue;
        }

        /// <summary>
        /// Converts the type of to storage.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value being resolved.
        /// </typeparam>
        /// <param name="valueToStore">
        /// The value to store.
        /// </param>
        /// <returns>
        /// A <see cref="Object"/> instance.
        /// </returns>
        private static Object ConvertToStorageType<T>(T valueToStore)
        {
            return valueToStore;
        }
    }
}