namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ExecuteBookmark"/>
    ///   class is a workflow activity that is used to process bookmarks.
    /// </summary>
    public sealed class ExecuteBookmark : NativeActivity
    {
        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            RuntimeArgument bookmarkNameArgument = new RuntimeArgument("BookmarkName", typeof(String), ArgumentDirection.In, true);

            metadata.Bind(BookmarkName, bookmarkNameArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        bookmarkNameArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);
        }

        /// <summary>
        /// Runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// No <see cref="BookmarkName"/> has been provided.
        /// </exception>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
            Justification = "The workflow property is acting as the argument in this scenario.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            String bookmarkName = context.GetValue(BookmarkName);

            if (String.IsNullOrWhiteSpace(bookmarkName))
            {
                throw new ArgumentNullException("BookmarkName");
            }

            context.CreateBookmark(bookmarkName, BookmarkResumed);
        }

        /// <summary>
        /// Executes the callback logic when a bookmark is resumed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="bookmark">
        /// The bookmark.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private static void BookmarkResumed(NativeActivityContext context, Bookmark bookmark, Object value)
        {
            // Nothing to do here
            // This callback is required to ensure that the workflow does not continue
        }

        /// <summary>
        ///   Gets or sets the name of the bookmark.
        /// </summary>
        /// <value>
        ///   The name of the bookmark.
        /// </value>
        [RequiredArgument]
        [Category("Inputs")]
        [Description("The name used to identify the bookmark")]
        public InArgument<String> BookmarkName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether the activity can cause the workflow to become idle.
        /// </summary>
        /// <value>
        ///   The can induce idle.
        /// </value>
        /// <returns>
        ///   True if the activity can cause the workflow to become idle. This value is false by default.
        /// </returns>
        protected override Boolean CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}