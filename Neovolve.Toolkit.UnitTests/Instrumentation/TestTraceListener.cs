namespace Neovolve.Toolkit.UnitTests.Instrumentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// The <see cref="TestTraceListener"/>
    ///   class is used to test the instrumentation toolkit.
    /// </summary>
    public class TestTraceListener : TraceListener
    {
        /// <summary>
        ///   Initializes static members of the <see cref = "TestTraceListener" /> class.
        /// </summary>
        static TestTraceListener()
        {
            Events = new Collection<TestTraceEvent>();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            Events.Clear();
            FlushCalled = false;
        }

        /// <summary>
        /// Flushes the output buffer.
        /// </summary>
        public override void Flush()
        {
            FlushCalled = true;

            base.Flush();
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">
        /// A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        /// </param>
        /// <param name="source">
        /// A name used to identify the output, typically the name of the application that generated the trace event.
        /// </param>
        /// <param name="eventType">
        /// One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        /// </param>
        /// <param name="id">
        /// A numeric identifier for the event.
        /// </param>
        /// <param name="message">
        /// A message to write.
        /// </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, Int32 id, String message)
        {
            TestTraceEvent newEvent = new TestTraceEvent
                                      {
                                          EventType = eventType, 
                                          Message = message
                                      };

            Events.Add(newEvent);

            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">
        /// A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        /// </param>
        /// <param name="source">
        /// A name used to identify the output, typically the name of the application that generated the trace event.
        /// </param>
        /// <param name="eventType">
        /// One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        /// </param>
        /// <param name="id">
        /// A numeric identifier for the event.
        /// </param>
        /// <param name="format">
        /// A format string that contains zero or more format items, which correspond to objects in the <paramref name="args"/> array.
        /// </param>
        /// <param name="args">
        /// An object array containing zero or more objects to format.
        /// </param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceEvent(
            TraceEventCache eventCache, String source, TraceEventType eventType, Int32 id, String format, params Object[] args)
        {
            TestTraceEvent newEvent = new TestTraceEvent
                                      {
                                          EventType = eventType, 
                                          Message = String.Format(CultureInfo.CurrentCulture, format, args)
                                      };

            Events.Add(newEvent);

            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        /// <summary>
        /// Writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">
        /// A message to write.
        /// </param>
        public override void Write(String message)
        {
            Debug.Write(message);
        }

        /// <summary>
        /// Writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">
        /// A message to write.
        /// </param>
        public override void WriteLine(String message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        ///   Gets the events.
        /// </summary>
        /// <value>
        ///   The events.
        /// </value>
        public static Collection<TestTraceEvent> Events
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [flush called].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [flush called]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean FlushCalled
        {
            get;
            set;
        }
    }
}