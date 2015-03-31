namespace Neovolve.Toolkit.Server.Unity.WebIntegrationTests
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The <see cref="TestService"/>
    ///   class is used to implement the test service contract.
    /// </summary>
    public class TestService : ITestService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestService"/> class.
        /// </summary>
        /// <param name="hashCalculator">
        /// The hash calculator.
        /// </param>
        public TestService(HashAlgorithm hashCalculator)
        {
            Calculator = hashCalculator;
        }

        /// <summary>
        /// Gets some value.
        /// </summary>
        /// <param name="data">
        /// The data for the call.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public String GetSomeValue(String data)
        {
            Byte[] originalData = Encoding.UTF8.GetBytes(data);
            Byte[] hashedData = Calculator.ComputeHash(originalData);
            String hashedValue = Convert.ToBase64String(hashedData);

            return hashedValue + " is the hashed value of " + data;
        }

        /// <summary>
        ///   Gets or sets the calculator.
        /// </summary>
        /// <value>
        ///   The calculator.
        /// </value>
        protected HashAlgorithm Calculator
        {
            get;
            set;
        }
    }
}