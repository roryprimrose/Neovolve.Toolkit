namespace Neovolve.Toolkit.Unity.IntegrationTests
{
    using System;
    using System.Configuration;

    /// <summary>
    /// The <see cref="ConnectionTest"/>
    ///   class is used to test the <see cref="ConnectionStringParameterValueElement"/>.
    /// </summary>
    public class ConnectionTest : IDoSomething
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionTest"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public ConnectionTest(String connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionTest"/> class.
        /// </summary>
        /// <param name="connectionSetting">
        /// The connection setting.
        /// </param>
        public ConnectionTest(ConnectionStringSettings connectionSetting)
        {
            ConnectionSetting = connectionSetting;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public String Execute()
        {
            return ConnectionString;
        }

        /// <summary>
        ///   Gets the connection setting.
        /// </summary>
        /// <value>
        ///   The connection setting.
        /// </value>
        public ConnectionStringSettings ConnectionSetting
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the connection string.
        /// </summary>
        /// <value>
        ///   The connection string.
        /// </value>
        public String ConnectionString
        {
            get;
            private set;
        }
    }
}