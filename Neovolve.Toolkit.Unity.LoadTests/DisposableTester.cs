namespace Neovolve.Toolkit.Unity.LoadTests
{
    using System;

    /// <summary>
    /// The <see cref="DisposableTester"/>
    ///   class is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public abstract class DisposableTester : IDisposable
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        private void Dispose(Boolean disposing)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("The object has already been disposed");
            }

            // Check to see if Dispose has already been called.
            if (IsDisposed == false)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.
            }

            IsDisposed = true;
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsDisposed
        {
            get;
            private set;
        }
    }
}