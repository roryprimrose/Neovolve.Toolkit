namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows.Markup;
    using Neovolve.Toolkit.Workflow.Extensions;

    /// <summary>
    /// The <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/>
    ///   class is used to provide resolution of instances for workflow activities.
    /// </summary>
    /// <typeparam name="T1">
    /// The first instance type.
    /// </typeparam>
    /// <typeparam name="T2">
    /// The second instance type.
    /// </typeparam>
    /// <typeparam name="T3">
    /// The third instance type.
    /// </typeparam>
    /// <typeparam name="T4">
    /// The fourth instance type.
    /// </typeparam>
    /// <typeparam name="T5">
    /// The fifth instance type.
    /// </typeparam>
    /// <typeparam name="T6">
    /// The sixth instance type.
    /// </typeparam>
    /// <typeparam name="T7">
    /// The seventh instance type.
    /// </typeparam>
    /// <typeparam name="T8">
    /// The eighth instance type.
    /// </typeparam>
    /// <typeparam name="T9">
    /// The ninth instance type.
    /// </typeparam>
    /// <typeparam name="T10">
    /// The tenth instance type.
    /// </typeparam>
    /// <typeparam name="T11">
    /// The eleventh instance type.
    /// </typeparam>
    /// <typeparam name="T12">
    /// The twelfth instance type.
    /// </typeparam>
    /// <typeparam name="T13">
    /// The thirteenth instance type.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The fourteenth instance type.
    /// </typeparam>
    /// <typeparam name="T15">
    /// The fifteenth instance type.
    /// </typeparam>
    /// <typeparam name="T16">
    /// The sixteenth instance type.
    /// </typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", 
        Justification = "The ActivityAction requires all the generic type definitions.")]
    [ContentProperty("Body")]
    public class InstanceResolver<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : NativeActivity
    {
        /// <summary>
        ///   Stores the instance handler for this instance.
        /// </summary>
        private readonly Variable<List<Guid>> _handlers = new Variable<List<Guid>>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}" /> class.
        /// </summary>
        public InstanceResolver()
            : this(GenericArgumentCount.One)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> class.
        /// </summary>
        /// <param name="argumentCount">
        /// The argument count.
        /// </param>
        public InstanceResolver(GenericArgumentCount argumentCount)
        {
            ArgumentCount = argumentCount;
            DisplayName = ActivityHelper.GenerateDisplayName(GetType(), argumentCount);
            Body = new ActivityAction
                <InstanceHandler<T1>, InstanceHandler<T2>, InstanceHandler<T3>, InstanceHandler<T4>, InstanceHandler<T5>, InstanceHandler<T6>, 
                    InstanceHandler<T7>, InstanceHandler<T8>, InstanceHandler<T9>, InstanceHandler<T10>, InstanceHandler<T11>, InstanceHandler<T12>, 
                    InstanceHandler<T13>, InstanceHandler<T14>, InstanceHandler<T15>, InstanceHandler<T16>>
                   {
                       Argument1 = new DelegateInArgument<InstanceHandler<T1>>("handler1"), 
                       Argument2 = new DelegateInArgument<InstanceHandler<T2>>("handler2"), 
                       Argument3 = new DelegateInArgument<InstanceHandler<T3>>("handler3"), 
                       Argument4 = new DelegateInArgument<InstanceHandler<T4>>("handler4"), 
                       Argument5 = new DelegateInArgument<InstanceHandler<T5>>("handler5"), 
                       Argument6 = new DelegateInArgument<InstanceHandler<T6>>("handler6"), 
                       Argument7 = new DelegateInArgument<InstanceHandler<T7>>("handler7"), 
                       Argument8 = new DelegateInArgument<InstanceHandler<T8>>("handler8"), 
                       Argument9 = new DelegateInArgument<InstanceHandler<T9>>("handler9"), 
                       Argument10 = new DelegateInArgument<InstanceHandler<T10>>("handler10"), 
                       Argument11 = new DelegateInArgument<InstanceHandler<T11>>("handler11"), 
                       Argument12 = new DelegateInArgument<InstanceHandler<T12>>("handler12"), 
                       Argument13 = new DelegateInArgument<InstanceHandler<T13>>("handler13"), 
                       Argument14 = new DelegateInArgument<InstanceHandler<T14>>("handler14"), 
                       Argument15 = new DelegateInArgument<InstanceHandler<T15>>("handler15"), 
                       Argument16 = new DelegateInArgument<InstanceHandler<T16>>("handler16")
                   };
        }

        /// <summary>
        /// Aborts the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void Abort(NativeActivityAbortContext context)
        {
            DestroyHandlers(context);
        }

        /// <summary>
        /// Caches the metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.RequireExtension<InstanceManagerExtension>();
            metadata.AddDelegate(Body);
            metadata.AddDefaultExtensionProvider(() => new InstanceManagerExtension());
            metadata.AddImplementationVariable(_handlers);

            CacheMetadataBindProperties(metadata);
        }

        /// <summary>
        /// Cancels the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        protected override void Cancel(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            context.CancelChildren();

            DestroyHandlers(context);
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Code contracts validate the parameter.")]
        protected override void Execute(NativeActivityContext context)
        {
            Contract.Assume(context != null);

            if (CanExecute() == false)
            {
                return;
            }

            InstanceManagerExtension extension = context.GetExtension<InstanceManagerExtension>();

            // The extension is identified as required in CacheMetadata
            Contract.Assume(extension != null);

            GenericArgumentCount argumentCount = ArgumentCount;
            List<Guid> handlerIdList = new List<Guid>((Int32)argumentCount);

            Handlers.Set(context, handlerIdList);

            InstanceHandler<T1> handler1 = CreateHandler<T1>(context, extension, ResolutionName1, GenericArgumentCount.One, argumentCount);
            InstanceHandler<T2> handler2 = CreateHandler<T2>(context, extension, ResolutionName2, GenericArgumentCount.Two, argumentCount);
            InstanceHandler<T3> handler3 = CreateHandler<T3>(context, extension, ResolutionName3, GenericArgumentCount.Three, argumentCount);
            InstanceHandler<T4> handler4 = CreateHandler<T4>(context, extension, ResolutionName4, GenericArgumentCount.Four, argumentCount);
            InstanceHandler<T5> handler5 = CreateHandler<T5>(context, extension, ResolutionName5, GenericArgumentCount.Five, argumentCount);
            InstanceHandler<T6> handler6 = CreateHandler<T6>(context, extension, ResolutionName6, GenericArgumentCount.Six, argumentCount);
            InstanceHandler<T7> handler7 = CreateHandler<T7>(context, extension, ResolutionName7, GenericArgumentCount.Seven, argumentCount);
            InstanceHandler<T8> handler8 = CreateHandler<T8>(context, extension, ResolutionName8, GenericArgumentCount.Eight, argumentCount);
            InstanceHandler<T9> handler9 = CreateHandler<T9>(context, extension, ResolutionName9, GenericArgumentCount.Nine, argumentCount);
            InstanceHandler<T10> handler10 = CreateHandler<T10>(context, extension, ResolutionName10, GenericArgumentCount.Ten, argumentCount);
            InstanceHandler<T11> handler11 = CreateHandler<T11>(context, extension, ResolutionName11, GenericArgumentCount.Eleven, argumentCount);
            InstanceHandler<T12> handler12 = CreateHandler<T12>(context, extension, ResolutionName12, GenericArgumentCount.Twelve, argumentCount);
            InstanceHandler<T13> handler13 = CreateHandler<T13>(context, extension, ResolutionName13, GenericArgumentCount.Thirteen, argumentCount);
            InstanceHandler<T14> handler14 = CreateHandler<T14>(context, extension, ResolutionName14, GenericArgumentCount.Fourteen, argumentCount);
            InstanceHandler<T15> handler15 = CreateHandler<T15>(context, extension, ResolutionName15, GenericArgumentCount.Fifteen, argumentCount);
            InstanceHandler<T16> handler16 = CreateHandler<T16>(context, extension, ResolutionName16, GenericArgumentCount.Sixteen, argumentCount);

            context.ScheduleAction(
                Body, 
                handler1, 
                handler2, 
                handler3, 
                handler4, 
                handler5, 
                handler6, 
                handler7, 
                handler8, 
                handler9, 
                handler10, 
                handler11, 
                handler12, 
                handler13, 
                handler14, 
                handler15, 
                handler16, 
                OnCompleted, 
                null);
        }

        /// <summary>
        /// The cache metadata bind properties.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        private void CacheMetadataBindProperties(NativeActivityMetadata metadata)
        {
            RuntimeArgument resolutionName1Argument = new RuntimeArgument("ResolutionName1", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName2Argument = new RuntimeArgument("ResolutionName2", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName3Argument = new RuntimeArgument("ResolutionName3", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName4Argument = new RuntimeArgument("ResolutionName4", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName5Argument = new RuntimeArgument("ResolutionName5", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName6Argument = new RuntimeArgument("ResolutionName6", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName7Argument = new RuntimeArgument("ResolutionName7", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName8Argument = new RuntimeArgument("ResolutionName8", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName9Argument = new RuntimeArgument("ResolutionName9", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName10Argument = new RuntimeArgument("ResolutionName10", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName11Argument = new RuntimeArgument("ResolutionName11", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName12Argument = new RuntimeArgument("ResolutionName12", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName13Argument = new RuntimeArgument("ResolutionName13", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName14Argument = new RuntimeArgument("ResolutionName14", typeof(String), ArgumentDirection.In);
            RuntimeArgument resolutionName15Argument = new RuntimeArgument("ResolutionName15", typeof(String), ArgumentDirection.In);

            metadata.Bind(ResolutionName1, resolutionName1Argument);
            metadata.Bind(ResolutionName2, resolutionName2Argument);
            metadata.Bind(ResolutionName3, resolutionName3Argument);
            metadata.Bind(ResolutionName4, resolutionName4Argument);
            metadata.Bind(ResolutionName5, resolutionName5Argument);
            metadata.Bind(ResolutionName6, resolutionName6Argument);
            metadata.Bind(ResolutionName7, resolutionName7Argument);
            metadata.Bind(ResolutionName8, resolutionName8Argument);
            metadata.Bind(ResolutionName9, resolutionName9Argument);
            metadata.Bind(ResolutionName10, resolutionName10Argument);
            metadata.Bind(ResolutionName11, resolutionName11Argument);
            metadata.Bind(ResolutionName12, resolutionName12Argument);
            metadata.Bind(ResolutionName13, resolutionName13Argument);
            metadata.Bind(ResolutionName14, resolutionName14Argument);
            metadata.Bind(ResolutionName15, resolutionName15Argument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        resolutionName1Argument, 
                                                        resolutionName2Argument, 
                                                        resolutionName3Argument, 
                                                        resolutionName4Argument, 
                                                        resolutionName5Argument, 
                                                        resolutionName6Argument, 
                                                        resolutionName7Argument, 
                                                        resolutionName8Argument, 
                                                        resolutionName9Argument, 
                                                        resolutionName10Argument, 
                                                        resolutionName11Argument, 
                                                        resolutionName12Argument, 
                                                        resolutionName13Argument, 
                                                        resolutionName14Argument, 
                                                        resolutionName15Argument, 
                                                    };

            metadata.SetArgumentsCollection(arguments);
        }

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        private Boolean CanExecute()
        {
            if (Body == null)
            {
                return false;
            }

            if (Body.Handler == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The create handler.
        /// </summary>
        /// <typeparam name="TH">
        /// The type of handler to create.
        /// </typeparam>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="extension">
        /// The extension.
        /// </param>
        /// <param name="resolutionName">
        /// The resolution name.
        /// </param>
        /// <param name="handlerCount">
        /// The handler count.
        /// </param>
        /// <param name="argumentCount">
        /// The argument count.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceHandler&lt;H&gt;"/> instance.
        /// </returns>
        private InstanceHandler<TH> CreateHandler<TH>(
            ActivityContext context, 
            InstanceManagerExtension extension, 
            InArgument<String> resolutionName, 
            GenericArgumentCount handlerCount, 
            GenericArgumentCount argumentCount)
        {
            Contract.Requires<ArgumentNullException>(extension != null);

            if (handlerCount > argumentCount)
            {
                return null;
            }

            String resolveName = resolutionName.Get(context);
            InstanceHandler<TH> handler = extension.CreateInstanceHandler<TH>(resolveName);
            List<Guid> handlerIdList = Handlers.Get(context);

            handlerIdList.Add(handler.InstanceHandlerId);

            return handler;
        }

        /// <summary>
        /// Destroys the handlers.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private void DestroyHandlers(ActivityContext context)
        {
            InstanceManagerExtension extension = context.GetExtension<InstanceManagerExtension>();
            List<Guid> handlers = Handlers.Get(context);

            Contract.Assume(handlers != null);

            handlers.ToList().ForEach(extension.DestroyHandler);
        }

        /// <summary>
        /// Called when the activity has completed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="completedInstance">
        /// The completed instance.
        /// </param>
        private void OnCompleted(ActivityContext context, ActivityInstance completedInstance)
        {
            DestroyHandlers(context);
        }

        /// <summary>
        ///   Gets or sets the argument count.
        /// </summary>
        /// <value>
        ///   The argument count.
        /// </value>
        public GenericArgumentCount ArgumentCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the body of the activity.
        /// </summary>
        /// <value>
        ///   The body of the activity.
        /// </value>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "ActivityAction of a generic type requires nested generic types.")]
        [Browsable(false)]
        public
            ActivityAction
                <InstanceHandler<T1>, InstanceHandler<T2>, InstanceHandler<T3>, InstanceHandler<T4>, InstanceHandler<T5>, InstanceHandler<T6>, 
                    InstanceHandler<T7>, InstanceHandler<T8>, InstanceHandler<T9>, InstanceHandler<T10>, InstanceHandler<T11>, InstanceHandler<T12>, 
                    InstanceHandler<T13>, InstanceHandler<T14>, InstanceHandler<T15>, InstanceHandler<T16>> Body
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the first resolution name.
        /// </summary>
        /// <value>
        ///   The resolution first name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName1
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the tenth resolution name10.
        /// </summary>
        /// <value>
        ///   The resolution tenth name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName10
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the eleventh resolution name.
        /// </summary>
        /// <value>
        ///   The eleventh resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName11
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the twelfth resolution name.
        /// </summary>
        /// <value>
        ///   The twelfth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName12
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the thirteenth resolution name.
        /// </summary>
        /// <value>
        ///   The thirteenth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName13
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the fourteenth resolution name.
        /// </summary>
        /// <value>
        ///   The fourteenth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName14
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the fifteenth resolution name.
        /// </summary>
        /// <value>
        ///   The fifteenth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName15
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the sixteenth resolution name.
        /// </summary>
        /// <value>
        ///   The sixteenth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName16
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the second resolution name.
        /// </summary>
        /// <value>
        ///   The second resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName2
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the third resolution name.
        /// </summary>
        /// <value>
        ///   The third resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName3
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the fourth resolution name.
        /// </summary>
        /// <value>
        ///   The fourth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName4
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the fifth resolution name.
        /// </summary>
        /// <value>
        ///   The fifth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName5
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the sixth resolution name.
        /// </summary>
        /// <value>
        ///   The sixth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName6
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the seventh resolution name.
        /// </summary>
        /// <value>
        ///   The seventh resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName7
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the eighth resolution name.
        /// </summary>
        /// <value>
        ///   The eighth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName8
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the ninth resolution name.
        /// </summary>
        /// <value>
        ///   The ninth resolution name.
        /// </value>
        [DefaultValue((String)null)]
        public InArgument<String> ResolutionName9
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the instance handlers for this instance.
        /// </summary>
        /// <value>
        ///   The handlers.
        /// </value>
        private Variable<List<Guid>> Handlers
        {
            get
            {
                return _handlers;
            }
        }
    }
}