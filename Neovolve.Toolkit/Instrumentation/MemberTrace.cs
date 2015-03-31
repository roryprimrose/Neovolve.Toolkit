namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using Neovolve.Toolkit.Properties;

    /// <summary>
    /// The <see cref="MemberTrace"/>
    ///   class is used to provide activity tracing functionality for methods that declare them.
    /// </summary>
    public sealed class MemberTrace : IActivityWriter, IDisposable
    {
        /// <summary>
        ///   Initializes static members of the <see cref = "MemberTrace" /> class.
        /// </summary>
        static MemberTrace()
        {
            StartMessageFooter = BuildStartMessageFooter();
        }

        /// <overloads>
        ///   <summary>
        ///     Initializes a new instance of the <see cref = "MemberTrace" /> class.
        ///   </summary>
        /// </overloads>
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MemberTrace" /> class.
        /// </summary>
        /// <exception cref = "TraceSourceLoadException">
        ///   No <see cref = "TraceSource" /> instance was resolved.
        /// </exception>
        public MemberTrace()
            : this(String.Empty, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <exception cref="TraceSourceLoadException">
        /// No <see cref="TraceSource"/> instance was resolved.
        /// </exception>
        public MemberTrace(String traceSourceName)
            : this(traceSourceName, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        /// <exception cref="TraceSourceLoadException">
        /// No <see cref="TraceSource"/> instance was resolved.
        /// </exception>
        public MemberTrace(String traceSourceName, MethodBase methodToTrace)
            : this(traceSourceName, methodToTrace, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        public MemberTrace(String traceSourceName, MethodBase methodToTrace, ITraceSourceResolver resolver)
        {
            Initialize(traceSourceName, methodToTrace, resolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="source">
        /// The trace source.
        /// </param>
        public MemberTrace(TraceSource source)
            : this(source, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        public MemberTrace(TraceSource source, MethodBase methodToTrace)
        {
            Initialize(source, methodToTrace);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public MemberTrace(IActivityWriter writer)
            : this(writer, null)
        {
            Contract.Requires<ArgumentNullException>(writer != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTrace"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        public MemberTrace(IActivityWriter writer, MethodBase methodToTrace)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            Initialize(writer, methodToTrace);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;

            Debug.Assert(ActivityWriter != null, "No trace activity is available");

            // Check if the activity is disabled
            if (ActivityWriter.State == ActivityTraceState.Disabled)
            {
                return;
            }

            // Complete the activity
            ActivityWriter.StopActivity(MethodFullName);

            IDisposable disposableWriter = ActivityWriter as IDisposable;

            if (disposableWriter != null)
            {
                disposableWriter.Dispose();
            }

            // Destroy the activity
            ActivityWriter = null;
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceObjectDisposedException']/*"/>
        public void Flush()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(Resources.MemberTraceDisposedExceptionMessage);
            }

            Debug.Assert(ActivityWriter != null, "No trace activity is available");

            ActivityWriter.Flush();
        }

        /// <summary>
        /// Resumes the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceObjectDisposedException']/*"/>
        public void ResumeActivity(String message, params Object[] arguments)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(Resources.MemberTraceDisposedExceptionMessage);
            }

            Debug.Assert(ActivityWriter != null, "No trace activity is available");

            ActivityWriter.ResumeActivity(message, arguments);
        }

        /// <summary>
        /// Suspends the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceObjectDisposedException']/*"/>
        public void SuspendActivity(String message, params Object[] arguments)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(Resources.MemberTraceDisposedExceptionMessage);
            }

            Debug.Assert(ActivityWriter != null, "No trace activity is available");

            ActivityWriter.SuspendActivity(message, arguments);
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
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceObjectDisposedException']/*"/>
        public void Write(RecordType type, Exception exception)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(Resources.MemberTraceDisposedExceptionMessage);
            }

            Debug.Assert(ActivityWriter != null, "No trace activity is available");

            ActivityWriter.Write(type, exception);
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
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceObjectDisposedException']/*"/>
        public void Write(RecordType type, String message, params Object[] arguments)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(Resources.MemberTraceDisposedExceptionMessage);
            }

            Debug.Assert(ActivityWriter != null, "No trace activity is available");

            ActivityWriter.Write(type, message, arguments);
        }

        /// <summary>
        /// Builds the start message.
        /// </summary>
        /// <param name="callingMethodDetails">
        /// The calling method details.
        /// </param>
        /// <param name="methodFullName">
        /// Full name of the method.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private static String BuildStartMessage(MethodBase callingMethodDetails, String methodFullName)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(methodFullName);

            // Build up information about this activity
            builder.Append(Resources.MemberTrace_MethodNameTitle);
            builder.AppendLine(callingMethodDetails.Name);
            builder.Append(Resources.MemberTrace_DeclaringTypeTitle);
            builder.AppendLine(callingMethodDetails.DeclaringType.AssemblyQualifiedName);
            builder.Append(Resources.MemberTrace_AssemblyTitle);
            builder.AppendLine(callingMethodDetails.DeclaringType.Assembly.Location);
            builder.Append(Resources.MemberTrace_ThreadNameTitle);
            builder.AppendLine(Thread.CurrentThread.Name);
            builder.Append(Resources.MemberTrace_ThreadIdentityTitle);

            if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null)
            {
                builder.AppendLine(Thread.CurrentPrincipal.Identity.Name);
            }

            builder.Append(Resources.MemberTrace_EnvironmentIdentityTitle);
            builder.Append(Environment.UserDomainName);
            builder.Append(Path.DirectorySeparatorChar);
            builder.AppendLine(Environment.UserName);

            // Add the start message footer
            builder.Append(StartMessageFooter);

            return builder.ToString();
        }

        /// <summary>
        /// Builds the start message footer.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private static String BuildStartMessageFooter()
        {
            // Determine the start message footer    
            StringBuilder builder = new StringBuilder();

            builder.Append(Resources.MemberTrace_MachineNameTitle);
            builder.AppendLine(Environment.MachineName);
            builder.Append(Resources.MemberTrace_OperatingSystemTitle);
            builder.AppendLine(Environment.OSVersion.ToString());

            Process current = Process.GetCurrentProcess();

            // Check if there is a main module available
            if (current.MainModule != null)
            {
                builder.AppendLine(Resources.MemberTrace_MailModuleTitle);
                builder.Append(current.MainModule.FileVersionInfo.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the full name of the method.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private static String GetMethodFullName(MethodBase result)
        {
            // Determine the method name with type and signature
            String methodFullSignature = result.ToString();
            Int32 methodNameStartIndex = methodFullSignature.IndexOf(result.Name, StringComparison.Ordinal);

            if (methodNameStartIndex < 0)
            {
                throw new InvalidOperationException("Method name was not found in the signature of the method.");
            }
            else if (methodNameStartIndex > methodFullSignature.Length)
            {
                throw new InvalidOperationException("The method name was larger than the calculated method signature.");
            }

            String methodSignature = methodFullSignature.Substring(methodNameStartIndex);

            return String.Concat(result.DeclaringType.FullName, ".", methodSignature);
        }

        /// <summary>
        /// Initializes the specified trace source name.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        private void Initialize(String traceSourceName, MethodBase methodToTrace, ITraceSourceResolver resolver)
        {
            // Check if there is a resolver
            if (resolver == null)
            {
                // Default to using a configuration resolver
                resolver = TraceSourceResolverFactory.Create();
            }

            CallingMethodResolver callingMethodResolver = CallingMethodResolver.Find(resolver);

            // Assign any calling method that has been supplied
            callingMethodResolver.CallingMethod = methodToTrace;

            ActivityWriter = new ActivityTrace(traceSourceName, callingMethodResolver);

            if (ActivityWriter.State != ActivityTraceState.Disabled)
            {
                CallingMethodResult callingMethodResult = callingMethodResolver.Evaluate();

                StartTrace(callingMethodResult.CallingMethod);
            }
        }

        /// <summary>
        /// Initializes the specified source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        private void Initialize(TraceSource source, MethodBase methodToTrace)
        {
            ActivityWriter = new ActivityTrace(source);

            if (ActivityWriter.State != ActivityTraceState.Disabled)
            {
                if (methodToTrace == null)
                {
                    methodToTrace = CallingMethodResolver.DetermineCallingMethod();
                }

                StartTrace(methodToTrace);
            }
        }

        /// <summary>
        /// Initializes the specified writer.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="methodToTrace">
        /// The method to trace.
        /// </param>
        private void Initialize(IActivityWriter writer, MethodBase methodToTrace)
        {
            Debug.Assert(writer != null, "No writer instance has been provided");

            ActivityWriter = writer;

            if (ActivityWriter.State != ActivityTraceState.Disabled)
            {
                ITraceSourceResolver resolver = TraceSourceResolverFactory.Create();
                CallingMethodResolver callingMethodResolver = CallingMethodResolver.Find(resolver);

                // Assign any calling method that has been supplied
                callingMethodResolver.CallingMethod = methodToTrace;

                CallingMethodResult callingMethodResult = callingMethodResolver.Evaluate();

                StartTrace(callingMethodResult.CallingMethod);
            }
        }

        /// <summary>
        /// Starts the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceActivityBoundaryTracingNotSupportedException']/*"/>
        void IActivityWriter.StartActivity(String message, params Object[] arguments)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Starts the trace.
        /// </summary>
        /// <param name="callingMethod">
        /// The calling method.
        /// </param>
        private void StartTrace(MethodBase callingMethod)
        {
            Debug.Assert(ActivityWriter != null, "No ActivityWriter is available");
            Debug.Assert(ActivityWriter.State != ActivityTraceState.Disabled, "ActivityWriter is disabled");

            MethodFullName = GetMethodFullName(callingMethod);

            String startMessage = BuildStartMessage(callingMethod, MethodFullName);

            // Start the activity
            ActivityWriter.StartActivity(startMessage);
        }

        /// <summary>
        /// Stops the activity with the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <include file="Documentation\CommonDocumentation.xml" path="CommonDocumentation/Exceptions/Exception[@name='MemberTraceActivityBoundaryTracingNotSupportedException']/*"/>
        void IActivityWriter.StopActivity(String message, params Object[] arguments)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Gets or sets the start message footer.
        /// </summary>
        /// <value>
        ///   The start message footer.
        /// </value>
        private static String StartMessageFooter
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the state of the activity.
        /// </summary>
        /// <value>
        ///   The state of the activity.
        /// </value>
        /// <include file = "Documentation\CommonDocumentation.xml" path = "CommonDocumentation/Exceptions/Exception[@name='MemberTraceObjectDisposedException']/*" />
        public ActivityTraceState State
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException(Resources.MemberTraceDisposedExceptionMessage);
                }

                Debug.Assert(ActivityWriter != null, "No activity is available");

                return ActivityWriter.State;
            }
        }

        /// <summary>
        ///   Gets or sets the activity.
        /// </summary>
        /// <value>
        ///   The activity.
        /// </value>
        private IActivityWriter ActivityWriter
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "ActivityTrace" /> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        private Boolean Disposed
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the full name of the method.
        /// </summary>
        /// <value>
        ///   The full name of the method.
        /// </value>
        private String MethodFullName
        {
            get;
            set;
        }
    }
}