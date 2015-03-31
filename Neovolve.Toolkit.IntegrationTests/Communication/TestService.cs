namespace Neovolve.Toolkit.IntegrationTests.Communication
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Neovolve.Toolkit.Communication;

    /// <summary>
    /// The <see cref="TestService"/>
    ///   class is a service implementation for testing.
    /// </summary>
    [CLSCompliant(false)]
    [ErrorHandler(typeof(KnownErrorHandler), typeof(UnknownErrorHandler))]
    public class TestService : ITestService
    {
        /// <summary>
        /// Does something.
        /// </summary>
        public void DoSomething()
        {
            Debug.WriteLine("DoSomething was called");
        }

        /// <summary>
        /// Does something else.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// If set to <c>true</c> [second].
        /// </param>
        /// <param name="third">
        /// The third.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public String DoSomethingElse(String first, Boolean second, Int32 third)
        {
            Debug.WriteLine("DoSomethingElse was called with " + first + ", " + second + ", " + third);

            return first + second + third;
        }

        /// <summary>
        /// Gets the current identity.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public String GetCurrentIdentity()
        {
            if (Thread.CurrentPrincipal == null)
            {
                return String.Empty;
            }

            if (Thread.CurrentPrincipal.Identity == null)
            {
                return String.Empty;
            }

            if (Thread.CurrentPrincipal.Identity.Name == null)
            {
                return String.Empty;
            }

            return Thread.CurrentPrincipal.Identity.Name;
        }

        /// <summary>
        /// Throws the exception.
        /// </summary>
        public void ThrowException()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws the known fault.
        /// </summary>
        public void ThrowKnownFault()
        {
            throw new KnownException();
        }

        /// <summary>
        /// Throws the timeout exception.
        /// </summary>
        public void ThrowTimeoutException()
        {
            throw new TimeoutException();
        }

        /// <summary>
        /// Throws the unknown fault.
        /// </summary>
        public void ThrowUnknownFault()
        {
            throw new UnknownException();
        }
    }
}