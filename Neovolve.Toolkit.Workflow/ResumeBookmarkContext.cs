namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ResumeBookmarkContext"/>
    ///   class is used to resume a workflow from a bookmark.
    /// </summary>
    public class ResumeBookmarkContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResumeBookmarkContext"/> class.
        /// </summary>
        /// <param name="workflowInstanceId">
        /// The workflow instance id.
        /// </param>
        /// <param name="bookmarkName">
        /// Name of the bookmark.
        /// </param>
        public ResumeBookmarkContext(Guid workflowInstanceId, String bookmarkName)
        {
            Contract.Requires<ArgumentException>(workflowInstanceId.Equals(Guid.Empty) == false);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(bookmarkName) == false);

            WorkflowInstanceId = workflowInstanceId;
            BookmarkName = bookmarkName;
        }

        /// <summary>
        /// Promotes to value bookmark.
        /// </summary>
        /// <returns>
        /// A <see cref="ResumeBookmarkContext&lt;T&gt;"/> instance.
        /// </returns>
        internal ResumeBookmarkContext<Object> PromoteToValueBookmark()
        {
            Contract.Assume(WorkflowInstanceId.Equals(Guid.Empty) == false);
            Contract.Assume(String.IsNullOrWhiteSpace(BookmarkName) == false);

            return new ResumeBookmarkContext<Object>(WorkflowInstanceId, BookmarkName);
        }

        /// <summary>
        ///   Gets the name of the bookmark.
        /// </summary>
        /// <value>
        ///   The name of the bookmark.
        /// </value>
        public String BookmarkName
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the workflow instance id.
        /// </summary>
        /// <value>
        ///   The workflow instance id.
        /// </value>
        public Guid WorkflowInstanceId
        {
            get;
            private set;
        }
    }
}