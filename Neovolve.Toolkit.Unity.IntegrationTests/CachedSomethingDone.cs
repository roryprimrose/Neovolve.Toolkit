namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="CachedSomethingDone"/>
    ///   class is used to test dependency injection configuration.
    /// </summary>
    public class CachedSomethingDone : IDoSomething
    {
        /// <summary>
        ///   Stores the dependency.
        /// </summary>
        private readonly IDoSomething _dependency;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSomethingDone"/> class.
        /// </summary>
        /// <param name="dependency">
        /// The dependency.
        /// </param>
        /// <param name="maxAgeInMilliseconds">
        /// The max age in milliseconds.
        /// </param>
        public CachedSomethingDone(IDoSomething dependency, Int64 maxAgeInMilliseconds)
        {
            _dependency = dependency;
            MaxAgeInMilliseconds = maxAgeInMilliseconds;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public String Execute()
        {
            // Check cache for value
            // If cache has value and is not too old then return the value
            // If not, get value from dependency, cache it for the next call and return the value
            return _dependency.Execute();
        }

        /// <summary>
        ///   Gets the max age in milliseconds.
        /// </summary>
        /// <value>
        ///   The max age in milliseconds.
        /// </value>
        public Int64 MaxAgeInMilliseconds
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the property test.
        /// </summary>
        /// <value>
        ///   The property test.
        /// </value>
        [Dependency]
        public String PropertyTest
        {
            get;
            set;
        }
    }
}