namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Activities;
    using System.Activities.DurableInstancing;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Runtime.DurableInstancing;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// The <see cref="ActivityInvoker"/>
    ///   class is used to invoke activities.
    /// </summary>
    public static class ActivityInvoker
    {
        /// <summary>
        ///   Stores the parent property reference.
        /// </summary>
        private static readonly PropertyInfo _parentProperty = GetActivityParentProperty();

        /// <summary>
        ///   Stores the preserve stack trace method.
        /// </summary>
        private static readonly MethodInfo _preserveStackTraceMethod = GetExceptionPreserveMethod();

        /// <summary>
        /// Invokes the specified activity.
        /// </summary>
        /// <param name="activity">
        /// The activity.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary&lt;TKey,TValue&gt;"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(Activity activity)
        {
            return Invoke(activity, null);
        }

        /// <summary>
        /// Invokes the specified activity using the provided input parameters.
        /// </summary>
        /// <param name="activity">
        /// The activity.
        /// </param>
        /// <param name="inputParameters">
        /// The input parameters.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary&lt;TKey,TValue&gt;"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(Activity activity, IDictionary<String, Object> inputParameters)
        {
            if (inputParameters == null)
            {
                inputParameters = new Dictionary<String, Object>();
            }

            WorkflowApplication application = new WorkflowApplication(activity, inputParameters);

            return Invoke(application, null, null, null);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary&lt;TKey,TValue&gt;"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(WorkflowApplication application)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            return Invoke(application, null, null, null);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="instanceStore">
        /// The instance store.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary{TKey,TValue}"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(WorkflowApplication application, InstanceStore instanceStore)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            return Invoke(application, instanceStore, null, null);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="instanceStore">
        /// The instance store.
        /// </param>
        /// <param name="resumeContext">
        /// The resume context.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary{TKey,TValue}"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(
            WorkflowApplication application, InstanceStore instanceStore, ResumeBookmarkContext resumeContext)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            return Invoke(application, instanceStore, resumeContext, null);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="initialize">
        /// The initialize.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary{TKey,TValue}"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(WorkflowApplication application, Action<Activity> initialize)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            return Invoke(application, null, null, initialize);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="instanceStore">
        /// The instance store.
        /// </param>
        /// <param name="resumeContext">
        /// The resume context.
        /// </param>
        /// <param name="initialize">
        /// The initialize.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary{TKey,TValue}"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke(
            WorkflowApplication application, InstanceStore instanceStore, ResumeBookmarkContext resumeContext, Action<Activity> initialize)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            ResumeBookmarkContext<Object> valueResumeContext = null;

            if (resumeContext != null)
            {
                valueResumeContext = resumeContext.PromoteToValueBookmark();
            }

            return Invoke(application, instanceStore, valueResumeContext, initialize);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <typeparam name="T">
        /// The type of resume value.
        /// </typeparam>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="instanceStore">
        /// The instance store.
        /// </param>
        /// <param name="resumeContext">
        /// The resume context.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary&lt;TKey,TValue&gt;"/> instance.
        /// </returns>
        public static IDictionary<String, Object> Invoke<T>(
            WorkflowApplication application, InstanceStore instanceStore, ResumeBookmarkContext<T> resumeContext)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            return Invoke(application, instanceStore, resumeContext, null);
        }

        /// <summary>
        /// Invokes the specified application.
        /// </summary>
        /// <typeparam name="T">
        /// The type of resume value.
        /// </typeparam>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="instanceStore">
        /// The instance store.
        /// </param>
        /// <param name="resumeContext">
        /// The resume context.
        /// </param>
        /// <param name="initialize">
        /// The initialize.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary&lt;TKey,TValue&gt;"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        public static IDictionary<String, Object> Invoke<T>(
            WorkflowApplication application, InstanceStore instanceStore, ResumeBookmarkContext<T> resumeContext, Action<Activity> initialize)
        {
            Contract.Requires<ArgumentNullException>(application != null);

            if (instanceStore != null)
            {
                application.InstanceStore = instanceStore;

                if (resumeContext == null)
                {
                    InstanceHandle instanceHandle = instanceStore.CreateInstanceHandle();
                    CreateWorkflowOwnerCommand createWorkflowOwnerCommand = new CreateWorkflowOwnerCommand();
                    InstanceView view = instanceStore.Execute(instanceHandle, createWorkflowOwnerCommand, TimeSpan.FromSeconds(30));

                    instanceStore.DefaultInstanceOwner = view.InstanceOwner;
                }
                else
                {
                    application.Load(resumeContext.WorkflowInstanceId);

                    Bookmark bookmark = new Bookmark(resumeContext.BookmarkName);

                    application.ResumeBookmark(bookmark, resumeContext.BookmarkValue);
                }
            }

            Exception thrownException = null;
            IDictionary<String, Object> outputParameters = null;
            ManualResetEvent waitHandle = new ManualResetEvent(false);

            application.OnUnhandledException = (WorkflowApplicationUnhandledExceptionEventArgs arg) =>
                                               {
                                                   // Preserve the stack trace in this exception
                                                   // This is a hack into the Exception.InternalPreserveStackTrace method that allows us to re-throw and preserve the call stack
                                                   _preserveStackTraceMethod.Invoke(arg.UnhandledException, null);

                                                   thrownException = CreateActivityFailureException(arg.UnhandledException, arg.ExceptionSource);

                                                   return UnhandledExceptionAction.Terminate;
                                               };
            application.Completed = (WorkflowApplicationCompletedEventArgs obj) =>
                                    {
                                        outputParameters = obj.Outputs;

                                        waitHandle.Set();
                                    };
            application.Aborted = delegate(WorkflowApplicationAbortedEventArgs arg)
                                  {
                                      if (arg.Reason != null)
                                      {
                                          // Preserve the stack trace in this exception
                                          // This is a hack into the Exception.InternalPreserveStackTrace method that allows us to re-throw and preserve the call stack
                                          _preserveStackTraceMethod.Invoke(arg.Reason, null);

                                          thrownException = CreateActivityFailureException(arg.Reason, application.WorkflowDefinition);
                                      }

                                      waitHandle.Set();
                                  };

            // application.Idle = (WorkflowApplicationIdleEventArgs obj) => waitHandle.Set();
            application.PersistableIdle = (WorkflowApplicationIdleEventArgs arg) =>
                                          {
                                              return PersistableIdleAction.Unload;
                                          };
            application.Unloaded = (WorkflowApplicationEventArgs obj) => waitHandle.Set();

            if (initialize != null)
            {
                initialize(application.WorkflowDefinition);
            }

            application.Run();

            waitHandle.WaitOne();

            if (thrownException != null)
            {
                throw thrownException;
            }

            return outputParameters;
        }

        /// <summary>
        /// Invokes the specified activity asynchronously.
        /// </summary>
        /// <param name="activity">
        /// The activity.
        /// </param>
        public static void InvokeAsync(Activity activity)
        {
            Invoke(activity, null);
        }

        /// <summary>
        /// Invokes the activity asynchronously using the specified input parameters.
        /// </summary>
        /// <param name="activity">
        /// The activity.
        /// </param>
        /// <param name="inputParameters">
        /// The input parameters.
        /// </param>
        public static void InvokeAsync(Activity activity, IDictionary<String, Object> inputParameters)
        {
            if (inputParameters == null)
            {
                inputParameters = new Dictionary<String, Object>();
            }

            WorkflowApplication application = new WorkflowApplication(activity, inputParameters);

            application.Run();
        }

        /// <summary>
        /// Creates the activity failure exception.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// A <see cref="Exception"/> instance.
        /// </returns>
        private static Exception CreateActivityFailureException(Exception exception, Activity source)
        {
            // Create the hierarchy of activity names
            Activity activity = source;
            StringBuilder builder = new StringBuilder(exception.Message);
            builder.AppendLine();
            builder.AppendLine();

            builder.AppendLine("Workflow exception throw up the following activity stack:");

            while (activity != null)
            {
                builder.AppendLine(activity + " - " + activity.GetType().FullName);

                activity = _parentProperty.GetValue(activity, null) as Activity;
            }

            return new ActivityFailureException(builder.ToString(), exception);
        }

        /// <summary>
        /// Gets the activity parent property.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyInfo"/> instance.
        /// </returns>
        private static PropertyInfo GetActivityParentProperty()
        {
            return typeof(Activity).GetProperty("Parent", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets the exception preserve method.
        /// </summary>
        /// <returns>
        /// A <see cref="MethodInfo"/> instance.
        /// </returns>
        private static MethodInfo GetExceptionPreserveMethod()
        {
            return typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}