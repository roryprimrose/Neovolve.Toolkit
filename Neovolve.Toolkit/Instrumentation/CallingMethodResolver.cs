namespace Neovolve.Toolkit.Instrumentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The <see cref="CallingMethodResolver"/>
    ///   class is used to resolve the name of a <see cref="TraceSource"/>
    ///   based on the <see cref="Type"/> in user code that directly or indirectly invokes the resolver.
    /// </summary>
    internal class CallingMethodResolver : ITraceSourceResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallingMethodResolver"/> class.
        /// </summary>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        public CallingMethodResolver(ITraceSourceResolver resolver)
            : this(resolver, null)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallingMethodResolver"/> class.
        /// </summary>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <param name="callingMethod">
        /// The calling method.
        /// </param>
        public CallingMethodResolver(ITraceSourceResolver resolver, MethodBase callingMethod)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);

            ChildResolver = resolver;
            CallingMethod = callingMethod;
            Result = default(CallingMethodResult);
        }

        /// <summary>
        /// Determines the calling method.
        /// </summary>
        /// <returns>
        /// A <see cref="MethodBase"/> instance.
        /// </returns>
        public static MethodBase DetermineCallingMethod()
        {
            StackTrace trace = new StackTrace(false);
            MethodBase methodFound = null;

            for (Int32 index = 0; index < trace.FrameCount; index++)
            {
                StackFrame frame = trace.GetFrame(index);
                MethodBase frameMethod = frame.GetMethod();
                Type frameType = frameMethod.DeclaringType;
                Boolean isTraceableType = GetIsTraceableType(frameType);

                if (isTraceableType == false)
                {
                    continue;
                }

                Boolean isTraceableModule = GetIsTraceableModule(frameType);

                if (isTraceableModule == false)
                {
                    continue;
                }

                Boolean isTraceableBaseType = GetIsTraceableBaseType(frameType);

                if (isTraceableBaseType)
                {
                    continue;
                }

                methodFound = frameMethod;

                break;
            }

            Debug.Assert(methodFound != null, "Failed to resolve the calling method");

            return methodFound;
        }

        /// <summary>
        /// Evaluates the calling method.
        /// </summary>
        /// <returns>
        /// A <see cref="CallingMethodResult"/> value.
        /// </returns>
        public CallingMethodResult Evaluate()
        {
            if (Result.Equals(default(CallingMethodResult)) == false)
            {
                return Result;
            }

            // Check if a calling method was defined with the resolver
            if (CallingMethod == null)
            {
                // Determine the calling method by walking the callstack
                CallingMethod = DetermineCallingMethod();
            }

            CallingMethodResult result = new CallingMethodResult
                                         {
                                             CallingMethod = CallingMethod
                                         };

            Collection<String> names = ChildResolver.ResolveNames();

            result.TraceSourceName = DetectTraceSourceName(result.CallingType, names);

            // Store and the result
            Result = result;

            return Result;
        }

        /// <summary>
        /// Reloads the <see cref="TraceSource"/> names and instances available from the resolver.
        /// </summary>
        public void Reload()
        {
            ChildResolver.Reload();
        }

        /// <summary>
        /// Resolves a <see cref="TraceSource"/> using the specified name and string comparison.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="TraceSource"/>.
        /// </param>
        /// <param name="comparison">
        /// The string comparison to apply.
        /// </param>
        /// <returns>
        /// A <see cref="TraceSource"/> instance or <c>null</c> if the name is not found.
        /// </returns>
        public TraceSource Resolve(String name, StringComparison comparison)
        {
            return ChildResolver.Resolve(name, comparison);
        }

        /// <summary>
        /// Resolves the available <see cref="TraceSource"/> names.
        /// </summary>
        /// <returns>
        /// A <see cref="Collection{T}"/> instance.
        /// </returns>
        public Collection<String> ResolveNames()
        {
            return ChildResolver.ResolveNames();
        }

        /// <summary>
        /// Finds a <see cref="CallingMethodResolver"/> resolver in a chain of resolvers.
        /// </summary>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <returns>
        /// A <see cref="CallingMethodResolver"/> instance.
        /// </returns>
        /// <remarks>
        /// If no <see cref="CallingMethodResolver"/> instance is found in the resolver chain,
        ///   a new instance is returned that wraps the <paramref name="resolver"/> instance.
        /// </remarks>
        internal static CallingMethodResolver Find(ITraceSourceResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);

            CallingMethodResolver callingMethodResolver = resolver as CallingMethodResolver;

            // Check if the resolver being passed in is the correct type
            if (callingMethodResolver != null)
            {
                return callingMethodResolver;
            }

            ITraceSourceResolver resolverToCheck = resolver;

            // Loop while there is a child resolver to check
            while (resolverToCheck.ChildResolver != null)
            {
                callingMethodResolver = resolverToCheck.ChildResolver as CallingMethodResolver;

                // Check if the resolver being passed in is the correct type
                if (callingMethodResolver != null)
                {
                    return callingMethodResolver;
                }

                // Move to the next child resolver
                resolverToCheck = resolverToCheck.ChildResolver;
            }

            // Return a new CallingMethodResolver that wraps the resolver supplied
            return new CallingMethodResolver(resolver);
        }

        /// <summary>
        /// Checks if the provided name is a valid trace source name.
        /// </summary>
        /// <param name="traceSourceName">
        /// Name of the trace source.
        /// </param>
        /// <param name="names">
        /// The names.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified name is valid; otherwise <c>false</c>.
        /// </returns>
        private static Boolean CheckNameIsValid(String traceSourceName, IEnumerable<String> names)
        {
            if (String.IsNullOrWhiteSpace(traceSourceName))
            {
                return false;
            }

            // Loop through each of the names);
            if (names != null)
            {
                return names.Any(name => name.Equals(traceSourceName, StringComparison.OrdinalIgnoreCase));
            }

            // We didn't find the name
            return false;
        }

        /// <summary>
        /// Detects the name of the trace source.
        /// </summary>
        /// <param name="callingType">
        /// Type of the calling.
        /// </param>
        /// <param name="names">
        /// The names.
        /// </param>
        /// <returns>
        /// The name of a <see cref="TraceSource"/> or <see cref="String.Empty">String.Empty</see> if no appropriate name is available.
        /// </returns>
        private static String DetectTraceSourceName(Type callingType, IEnumerable<String> names)
        {
            String assemblyPath = callingType.Assembly.Location;

            if (File.Exists(assemblyPath) == false)
            {
                return String.Empty;
            }

            String assemblyName = Path.GetFileName(assemblyPath);

            // Check if the assembly name is a valid source name
            if (CheckNameIsValid(assemblyName, names))
            {
                return assemblyName;
            }

            // Strip the extension from the assembly name
            assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);

            // Check if the assembly name is a valid source name
            if (CheckNameIsValid(assemblyName, names))
            {
                return assemblyName;
            }

            // Attempt to resolve from the method declaring type, down through its namespace names
            String typeName = callingType.FullName;

            // Loop while there is a value to check
            while (String.IsNullOrEmpty(typeName) == false)
            {
                // Check if this name is valid
                if (CheckNameIsValid(typeName, names))
                {
                    return typeName;
                }

                // Check if there is a . in the name
                Int32 lastIndex = typeName.LastIndexOf('.');

                if (lastIndex > -1)
                {
                    // There is another namespace value to check
                    typeName = typeName.Substring(0, lastIndex);
                }
                else
                {
                    // We have exhausted all options using the namespace
                    break;
                }
            }

            // No trace source was found
            return String.Empty;
        }

        /// <summary>
        /// Gets the type of the is traceable base.
        /// </summary>
        /// <param name="frameType">
        /// Type of the frame.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private static Boolean GetIsTraceableBaseType(Type frameType)
        {
            Boolean isTraceableBaseType = false;
            Type baseType = frameType.BaseType;

            while (baseType != null)
            {
                if (baseType.FullName != null)
                {
                    // Skip MSTest framework proxy classes
                    if (baseType.FullName.Equals("Microsoft.VisualStudio.TestTools.UnitTesting.BaseShadow", StringComparison.OrdinalIgnoreCase))
                    {
                        isTraceableBaseType = true;

                        break;
                    }   
                }

                baseType = baseType.BaseType;
            }

            return isTraceableBaseType;
        }

        /// <summary>
        /// Gets the is traceable module.
        /// </summary>
        /// <param name="frameType">
        /// Type of the frame.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private static Boolean GetIsTraceableModule(Type frameType)
        {
            Boolean isTraceableModule = true;

            if (frameType.Module.ScopeName == "CommonLanguageRuntimeLibrary")
            {
                isTraceableModule = false;
            }

            // Skip over Microsoft names
            if (frameType.Module.ScopeName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase))
            {
                isTraceableModule = false;
            }

            // Skip over System names
            if (frameType.Module.ScopeName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
            {
                isTraceableModule = false;
            }

            return isTraceableModule;
        }

        /// <summary>
        /// Gets the type of the is traceable.
        /// </summary>
        /// <param name="frameType">
        /// Type of the frame.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private static Boolean GetIsTraceableType(Type frameType)
        {
            Boolean isTraceableType = true;

            if (typeof(ITraceSourceResolver).IsAssignableFrom(frameType))
            {
                // Skip this interface
                isTraceableType = false;
            }

            if (typeof(IRecordWriter).IsAssignableFrom(frameType))
            {
                // Skip this interface
                isTraceableType = false;
            }

            return isTraceableType;
        }

        /// <summary>
        ///   Gets or sets the calling method.
        /// </summary>
        /// <value>
        ///   The calling method.
        /// </value>
        public MethodBase CallingMethod
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the child resolver.
        /// </summary>
        /// <value>
        ///   The child resolver or <c>null</c> if there is no child resolver.
        /// </value>
        public ITraceSourceResolver ChildResolver
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the result.
        /// </summary>
        /// <value>
        ///   The result.
        /// </value>
        private CallingMethodResult Result
        {
            get;
            set;
        }
    }
}