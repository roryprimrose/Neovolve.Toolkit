namespace Neovolve.Toolkit.TestHarness
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The <see cref="Program"/>
    ///   class is the entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PerformanceTester());
        }
    }
}