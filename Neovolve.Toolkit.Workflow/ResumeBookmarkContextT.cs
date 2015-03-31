namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ResumeBookmarkContext{T}"/>
    ///   class is used to define the information required to resume a workflow bookmark.
    /// </summary>
    /// <typeparam name="T">
    /// The type of data to resume the workflow with.
    /// </typeparam>
    public class ResumeBookmarkContext<T> : ResumeBookmarkContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Toolkit.Workflow.ResumeBookmarkContext{T}"/> class.
        /// </summary>
        /// <param name="workflowInstanceId">
        /// The workflow instance id.
        /// </param>
        /// <param name="bookmarkName">
        /// Name of the bookmark.
        /// </param>
        public ResumeBookmarkContext(Guid workflowInstanceId, String bookmarkName)
            : this(workflowInstanceId, bookmarkName, default(T))
        {
            Contract.Requires<ArgumentException>(workflowInstanceId.Equals(Guid.Empty) == false);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(bookmarkName) == false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResumeBookmarkContext{T}"/> class.
        /// </summary>
        /// <param name="workflowInstanceId">
        /// The workflow instance id.
        /// </param>
        /// <param name="bookmarkName">
        /// Name of the bookmark.
        /// </param>
        /// <param name="bookmarkValue">
        /// The bookmark value.
        /// </param>
        public ResumeBookmarkContext(Guid workflowInstanceId, String bookmarkName, T bookmarkValue)
            : base(workflowInstanceId, bookmarkName)
        {
            Contract.Requires<ArgumentException>(workflowInstanceId.Equals(Guid.Empty) == false);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(bookmarkName) == false);

            BookmarkValue = bookmarkValue;
        }

        /// <summary>
        ///   Gets the bookmark value.
        /// </summary>
        /// <value>
        ///   The bookmark value.
        /// </value>
        public T BookmarkValue
        {
            get;
            private set;
        }
    }
}