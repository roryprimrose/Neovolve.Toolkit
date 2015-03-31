namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The <see cref="WorkflowExtensionWrapper{T}"/>
    ///   class is used to generate a function that creates an extension.
    /// </summary>
    /// <typeparam name="T">
    /// The type of extension to create.
    /// </typeparam>
    internal class WorkflowExtensionWrapper<T> where T : new()
    {
        /// <summary>
        /// Generates the create extension wrapper.
        /// </summary>
        /// <returns>
        /// A <see cref="Func&lt;T&gt;"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This member is used via reflection.")]
        public Func<T> GenerateCreateExtensionWrapper()
        {
            return () => new T();
        }
    }
}