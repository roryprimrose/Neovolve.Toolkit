namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Validation;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Windows.Markup;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="DisposalScope{T}"/>
    ///   class is used to provide disposal support for a resource held by a workflow.
    /// </summary>
    /// <typeparam name="T">
    /// The type of resource to dispose.
    /// </typeparam>
    /// <remarks>
    /// This activity uses a <see cref="NoPersistHandle"/> to prevent persistence within
    ///   its execution.
    /// </remarks>
    [ContentProperty("Body")]
    public sealed class DisposalScope<T> : NativeActivity<T> where T : class, IDisposable
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DisposalScope{T}" /> class.
        /// </summary>
        public DisposalScope()
        {
            NoPersistHandle = new Variable<NoPersistHandle>();
            Body = new ActivityAction<T>
                   {
                       Argument = new DelegateInArgument<T>("handler")
                   };
        }

        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddDelegate(Body);
            metadata.AddImplementationVariable(NoPersistHandle);

            RuntimeArgument instanceArgument = new RuntimeArgument("Instance", typeof(T), ArgumentDirection.In, true);

            metadata.Bind(Instance, instanceArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        instanceArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);

            if (Body == null || Body.Handler == null)
            {
                ValidationError validationError = new ValidationError(Resources.Activity_NoChildActivitiesDefined, true, "Body");

                metadata.AddValidationError(validationError);
            }
        }

        /// <summary>
        /// Runs the activity’s execution logic.
        /// </summary>
        /// <param name="context">
        /// The execution context in which the activity executes.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            NoPersistHandle noPersistHandle = NoPersistHandle.Get(context);

            noPersistHandle.Enter(context);

            T instance = Instance.Get(context);

            context.ScheduleAction(Body, instance, OnCompletion, OnFaulted);
        }

        /// <summary>
        /// Destroys the instance.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private void DestroyInstance(NativeActivityContext context)
        {
            T instance = Instance.Get(context);

            if (instance == null)
            {
                return;
            }

            try
            {
                instance.Dispose();

                Instance.Set(context, null);
            }
            catch (ObjectDisposedException)
            {
                // Ignore this exception
            }
        }

        /// <summary>
        /// Called when the activity has completed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="completedinstance">
        /// The completed instance.
        /// </param>
        private void OnCompletion(NativeActivityContext context, ActivityInstance completedinstance)
        {
            DestroyInstance(context);

            NoPersistHandle noPersistHandle = NoPersistHandle.Get(context);

            noPersistHandle.Exit(context);
        }

        /// <summary>
        /// Called when the activity has faulted.
        /// </summary>
        /// <param name="faultcontext">
        /// The fault context.
        /// </param>
        /// <param name="propagatedexception">
        /// The propagated exception.
        /// </param>
        /// <param name="propagatedfrom">
        /// The propagated from.
        /// </param>
        private void OnFaulted(NativeActivityFaultContext faultcontext, Exception propagatedexception, ActivityInstance propagatedfrom)
        {
            DestroyInstance(faultcontext);

            NoPersistHandle noPersistHandle = NoPersistHandle.Get(faultcontext);

            noPersistHandle.Exit(faultcontext);
        }

        /// <summary>
        ///   Gets or sets the body of the activity.
        /// </summary>
        /// <value>
        ///   The body of the activity.
        /// </value>
        [Browsable(false)]
        public ActivityAction<T> Body
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the instance.
        /// </summary>
        /// <value>
        ///   The instance.
        /// </value>
        [DefaultValue((String)null)]
        [RequiredArgument]
        public InArgument<T> Instance
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the no persist handle.
        /// </summary>
        /// <value>
        ///   The no persist handle.
        /// </value>
        private Variable<NoPersistHandle> NoPersistHandle
        {
            get;
            set;
        }
    }
}