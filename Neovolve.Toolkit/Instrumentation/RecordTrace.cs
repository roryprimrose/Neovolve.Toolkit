namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using Neovolve.Toolkit.Properties;
    using Neovolve.Toolkit.Storage;

    /// <summary>
    /// The <see cref="RecordTrace"/>
    ///   class is used to trace record information.
    /// </summary>
    public class RecordTrace : IRecordWriter
    {
        /// <summary>
        ///   Defines the configuration key used to load the value for the <see cref = "Enabled" /> property.
        /// </summary>
        public const String EnabledConfigurationKey = "NeovolveToolkitTrace.Enabled";

        /// <summary>
        ///   Defines the configuration key used to load the value for the <see cref = "ThrowOnSourceFailure" /> property.
        /// </summary>
        public const String ThrowOnSourceFailureConfigurationKey = "NeovolveToolkitTrace.ThrowOnSourceFailure";

        /// <summary>
        ///   Stores the Source value.
        /// </summary>
        private TraceSource _source;

        /// <summary>
        ///   Initializes static members of the <see cref = "RecordTrace" /> class.
        /// </summary>
        static RecordTrace()
        {
            IConfigurationStore store = ConfigurationStoreFactory.Create();

            ThrowOnSourceFailure = store.GetApplicationSetting(ThrowOnSourceFailureConfigurationKey, true);
            Enabled = store.GetApplicationSetting(EnabledConfigurationKey, true);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RecordTrace" /> class.
        /// </summary>
        public RecordTrace()
            : this(String.Empty, null, ThrowOnSourceFailure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        public RecordTrace(String traceSourceName)
            : this(traceSourceName, null, ThrowOnSourceFailure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="resolver">
        /// The <see cref="TraceSource"/> resolver.
        /// </param>
        public RecordTrace(String traceSourceName, ITraceSourceResolver resolver)
            : this(traceSourceName, resolver, ThrowOnSourceFailure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTrace"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        public RecordTrace(TraceSource source)
        {
            InitializationContext = new RecordTraceInitializationContext(source);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <param name="throwOnSourceLoadFailure">
        /// If set to <c>true</c> and exception will be thrown if the source cannot be loaded.
        /// </param>
        /// <remarks>
        /// This is used to support tracing within this Toolkit. It allows for exception throwing to be avoided
        ///   if the <see cref="TraceSource"/> is not able to be loaded.
        /// </remarks>
        internal RecordTrace(String traceSourceName, ITraceSourceResolver resolver, Boolean throwOnSourceLoadFailure)
        {
            InitializationContext = new RecordTraceInitializationContext(traceSourceName, resolver, throwOnSourceLoadFailure);
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            if (Source != null)
            {
                Source.Flush();
            }
        }

        /// <summary>
        /// Writes the specified exception and type.
        /// </summary>
        /// <param name="type">
        /// The type of message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Write(RecordType type, Exception exception)
        {
            TraceEventType eventType = ConvertToTraceEventType(type);

            WriteInternal(eventType, exception.ToString);
        }

        /// <summary>
        /// Writes the specified message and type.
        /// </summary>
        /// <param name="type">
        /// The type of message.
        /// </param>
        /// <param name="message">
        /// The message content.
        /// </param>
        /// <param name="arguments">
        /// The arguments related to the message.
        /// </param>
        public void Write(RecordType type, String message, params Object[] arguments)
        {
            TraceEventType eventType = ConvertToTraceEventType(type);

            Write(eventType, message, arguments);
        }

        /// <summary>
        /// Gets the source from context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// A <see cref="TraceSource"/> instance.
        /// </returns>
        protected static TraceSource GetSourceFromContext(RecordTraceInitializationContext context)
        {
            if (context.Source != null)
            {
                return context.Source;
            }

            return ResolveTraceSource(context.TraceSourceName, context.Resolver, context.ThrowOnSourceLoadFailure);
        }

        /// <summary>
        /// Ensures the initialized.
        /// </summary>
        protected void EnsureInitialized()
        {
            if (IsInitialized == false)
            {
                Initialize(InitializationContext);

                IsInitialized = true;

                // Reset the stored context to allow any possible memory to be released
                InitializationContext = default(RecordTraceInitializationContext);
            }
        }

        /// <overloads>
        /// <summary>
        /// Initializes this instance.
        ///   </summary>
        /// </overloads>
        /// <summary>
        /// Initializes this instance with the specified context.
        /// </summary>
        /// <param name="context">
        /// The trace context.
        /// </param>
        protected virtual void Initialize(RecordTraceInitializationContext context)
        {
            Source = GetSourceFromContext(context);
        }

        /// <summary>
        /// Shoulds the write record.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event.
        /// </param>
        /// <returns>
        /// <c>true</c> if a record should be written, otherwise <c>false</c>.
        /// </returns>
        protected virtual Boolean ShouldWriteRecord(TraceEventType eventType)
        {
            EnsureInitialized();

            if (Enabled == false)
            {
                return false;
            }

            if (Source == null)
            {
                return false;
            }

            // Check if the record should be written
            if (Source.Switch.ShouldTrace(eventType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Writes the specified event type.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        protected virtual void Write(TraceEventType eventType, String message, params Object[] arguments)
        {
            WriteInternal(eventType, () => CreateMessageWithArguments(message, arguments));
        }

        /// <summary>
        /// Writes the internal.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event.
        /// </param>
        /// <param name="getMessage">
        /// The get message function.
        /// </param>
        protected virtual void WriteInternal(TraceEventType eventType, Func<String> getMessage)
        {
            if (ShouldWriteRecord(eventType))
            {
                Debug.Assert(IsInitialized, "The trace instance is not initialized");

                String message = String.Empty;

                if (getMessage != null)
                {
                    message = getMessage();
                }

                Source.TraceEvent(eventType, 0, message);
            }
        }

        /// <summary>
        /// Converts the type of to trace event.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// A <see cref="TraceEventType"/> instance.
        /// </returns>
        private static TraceEventType ConvertToTraceEventType(RecordType source)
        {
            switch (source)
            {
                case RecordType.Verbose:

                    return TraceEventType.Verbose;

                case RecordType.Information:

                    return TraceEventType.Information;

                case RecordType.Warning:

                    return TraceEventType.Warning;

                case RecordType.Error:

                    return TraceEventType.Error;

                case RecordType.Critical:

                    return TraceEventType.Critical;

                default:

                    throw new ArgumentOutOfRangeException("source");
            }
        }

        /// <summary>
        /// Creates the message with arguments.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String CreateMessageWithArguments(String message, Object[] arguments)
        {
            Int32 maskCount = message.GetFormatMaskCount();
            Int32 argumentCount = 0;

            if (arguments != null)
            {
                argumentCount = arguments.Length;
            }

            String messageToWrite = message.FormatNullMasksInternal(CultureInfo.CurrentCulture, maskCount, arguments);

            if (argumentCount > maskCount)
            {
                Debug.Assert(arguments != null, "No arguments have been provided but an argument count has been determined");

                // We need to append additional arguments
                StringBuilder builder = new StringBuilder(messageToWrite);

                builder.AppendLine();
                builder.AppendLine();
                builder.Append(Resources.RecordTrace_AdditionalParametersHeader);

                for (Int32 index = maskCount; index < argumentCount; index++)
                {
                    builder.AppendLine();
                    builder.AppendFormat(CultureInfo.CurrentCulture, Resources.RecordTrace_AdditionalParameterOutput, index + 1, arguments[index]);
                }

                messageToWrite = builder.ToString();
            }

            return messageToWrite;
        }

        /// <summary>
        /// Resolves the trace source.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="resolver">
        /// The trace source resolver.
        /// </param>
        /// <param name="throwOnSourceLoadFailure">
        /// If set to <c>true</c> and exception will be thrown if the source cannot be loaded.
        /// </param>
        /// <returns>
        /// A <see cref="TraceSource"/> instance, or <c>null</c>.
        /// </returns>
        /// <exception cref="TraceSourceLoadException">
        /// No trace source was resolved and the <paramref name="throwOnSourceLoadFailure"/> value is <c>true</c>.
        /// </exception>
        private static TraceSource ResolveTraceSource(String traceSourceName, ITraceSourceResolver resolver, Boolean throwOnSourceLoadFailure)
        {
            ITraceSourceResolver internalResolver = resolver;

            if (internalResolver == null)
            {
                // Create the default resolver chain
                internalResolver = TraceSourceResolverFactory.Create();
            }

            // Always wrap the resolver in a cache resolver for performance
            // NOTE: The internal resolver must first be wrapped in the cache resolver before (optionally) the CallingMethodResolver
            // This is because the CacheResolver uses the resolver type name to generate cache keys to support
            // multiple resolvers storing difference TraceSource instances of the same name in the same memory cache
            CacheResolver cacheResolver = new CacheResolver(internalResolver);

            CallingMethodResolver callingMethodResolver = null;
            CallingMethodResult callingMethodResult = default(CallingMethodResult);

            // Check if there is a source name
            if (String.IsNullOrEmpty(traceSourceName))
            {
                // Get the resolver and evaluate the calling method information
                callingMethodResolver = CallingMethodResolver.Find(cacheResolver);
                callingMethodResult = callingMethodResolver.Evaluate();

                // Get the trace source name from the new resolver
                traceSourceName = callingMethodResult.TraceSourceName;
            }

            TraceSource source = null;

            // Check if there is a source name
            if (String.IsNullOrEmpty(traceSourceName) == false)
            {
                source = cacheResolver.Resolve(traceSourceName, StringComparison.OrdinalIgnoreCase);
            }

            // Check if there is a source
            if (source == null)
            {
                if (throwOnSourceLoadFailure)
                {
                    // Check if the calling method resolver has been created
                    // It will not be created at this point if there was a trace source name supplied
                    // but now we need to use it to find the calling method
                    if (callingMethodResolver == null)
                    {
                        // Get the resolver and evaluate the calling method information
                        callingMethodResolver = CallingMethodResolver.Find(cacheResolver);
                        callingMethodResult = callingMethodResolver.Evaluate();
                    }

                    // Check if the calling method is within the toolkit assembly
                    if (callingMethodResult.CallingMethod.DeclaringType.Assembly.Equals(Assembly.GetExecutingAssembly()))
                    {
                        // We don't want trace source load failures to throw exceptions for the toolkit itself
                        throwOnSourceLoadFailure = false;
                    }
                }

                // The resolver did not resolve a TraceSource instance
                if (throwOnSourceLoadFailure)
                {
                    // Determine whether an exception should be raised here
                    throw new TraceSourceLoadException();
                }
            }

            return source;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether tracing within the toolkit is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public static Boolean Enabled
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether a 
        ///   <see cref = "TraceSourceLoadException" /> is thrown when a
        ///   <see cref = "TraceSource" /> cannot be loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an exception should be thrown; otherwise, <c>false</c>.
        /// </value>
        public static Boolean ThrowOnSourceFailure
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        protected Boolean IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the trace source.
        /// </summary>
        /// <value>
        ///   The trace source.
        /// </value>
        protected virtual TraceSource Source
        {
            get
            {
                EnsureInitialized();

                return _source;
            }

            private set
            {
                _source = value;
            }
        }

        /// <summary>
        ///   Gets or sets the initialization context.
        /// </summary>
        /// <value>
        ///   The initialization context.
        /// </value>
        private RecordTraceInitializationContext InitializationContext
        {
            get;
            set;
        }
    }
}