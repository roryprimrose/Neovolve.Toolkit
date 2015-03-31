namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Threading;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// The <see cref="ActivityStore"/>
    ///   class is used to cache activity instances for reuse.
    /// </summary>
    public static class ActivityStore
    {
        /// <summary>
        ///   Stores the activity instances.
        /// </summary>
        private static readonly IDictionary<Type, Activity> _store = new Dictionary<Type, Activity>();

        /// <summary>
        ///   Provides the lock mechanism for working with the store.
        /// </summary>
        private static readonly ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T">
        /// The type of activity to return.
        /// </typeparam>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Resolve<T>() where T : Activity, new()
        {
            Type activityType = typeof(T);

            using (new LockReader(_syncLock))
            {
                if (_store.ContainsKey(activityType))
                {
                    return _store[activityType] as T;
                }
            }

            using (new LockWriter(_syncLock))
            {
                // Protect the store against multiple threads that get passed the reader lock
                if (_store.ContainsKey(activityType))
                {
                    return _store[activityType] as T;
                }

                T activity = new T();

                _store[activityType] = activity;

                return activity;
            }
        }
    }
}