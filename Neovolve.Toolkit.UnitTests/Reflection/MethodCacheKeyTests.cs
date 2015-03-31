namespace Neovolve.Toolkit.UnitTests.Reflection
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Reflection;

    /// <summary>
    /// The <see cref="MethodCacheKeyTests"/>
    ///   class contains unit tests for the <see cref="MethodCacheKey"/> struct.
    /// </summary>
    [TestClass]
    public class MethodCacheKeyTests
    {
        /// <summary>
        /// Runs test for creating method cache key with declaring type stores declaring type correctly.
        /// </summary>
        [TestMethod]
        public void CreatingMethodCacheKeyWithDeclaringTypeStoresDeclaringTypeCorrectlyTest()
        {
            MethodCacheKey target = new MethodCacheKey(GetType(), Guid.NewGuid().ToString(), null);

            Assert.AreEqual(GetType(), target.DeclaringType, "DeclaringType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating method cache key with method name stores method name correctly.
        /// </summary>
        [TestMethod]
        public void CreatingMethodCacheKeyWithMethodNameStoresMethodNameCorrectlyTest()
        {
            String methodName = Guid.NewGuid().ToString();

            MethodCacheKey target = new MethodCacheKey(GetType(), methodName, null);

            Assert.AreEqual(methodName, target.MethodName, "MethodName returned an incorrect value");
        }

        /// <summary>
        /// Runs test for creating method cache key with parameter types stores parameter types correctly.
        /// </summary>
        [TestMethod]
        public void CreatingMethodCacheKeyWithParameterTypesStoresParameterTypesCorrectlyTest()
        {
            Type[] parameterTypes = new[]
                                    {
                                        GetType()
                                    };

            MethodCacheKey target = new MethodCacheKey(GetType(), Guid.NewGuid().ToString(), parameterTypes);

            Assert.AreEqual(parameterTypes.Length, target.ParameterTypes.Count, "ParameterTypes returned an incorrect count");

            for (Int32 index = 0; index < parameterTypes.Length; index++)
            {
                Assert.AreEqual(parameterTypes[index], target.ParameterTypes[index], "ParameterType is an incorrect value");
            }
        }

        /// <summary>
        /// Runs test for equality evaluation returns not equal with different declaring type.
        /// </summary>
        [TestMethod]
        public void EqualityEvaluationReturnsNotEqualWithDifferentDeclaringTypeTest()
        {
            Type[] parameterTypes = new[]
                                    {
                                        GetType()
                                    };

            MethodCacheKey first = new MethodCacheKey(GetType(), Guid.NewGuid().ToString(), parameterTypes);
            MethodCacheKey second = new MethodCacheKey(typeof(ArgumentNullException), first.MethodName, first.ParameterTypes.ToArray());

            Assert.AreNotEqual(first, second, "Equality test returned an incorrect value");
        }

        /// <summary>
        /// Runs test for method cache key throws exception when created with empty method name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodCacheKeyThrowsExceptionWhenCreatedWithEmptyMethodNameTest()
        {
            new MethodCacheKey(GetType(), String.Empty);
        }

        /// <summary>
        /// Runs test for method cache key throws exception when created with null method name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodCacheKeyThrowsExceptionWhenCreatedWithNullMethodNameTest()
        {
            new MethodCacheKey(GetType(), null);
        }

        /// <summary>
        /// Runs test for method cache key throws exception when created with null type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodCacheKeyThrowsExceptionWhenCreatedWithNullTypeTest()
        {
            new MethodCacheKey(null, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Runs test for method cache key throws exception when created with white space method name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodCacheKeyThrowsExceptionWhenCreatedWithWhiteSpaceMethodNameTest()
        {
            new MethodCacheKey(GetType(), " ");
        }

        /// <summary>
        /// Runs test for to string returns unique value for same method and parameter list on different types.
        /// </summary>
        [TestMethod]
        public void ToStringReturnsUniqueValueForSameMethodAndParameterListOnDifferentTypesTest()
        {
            MethodCacheKey first = new MethodCacheKey(typeof(String), "ToString", null);
            MethodCacheKey second = new MethodCacheKey(typeof(Boolean), "ToString", null);

            Assert.AreNotEqual(first.ToString(), second.ToString(), "ToString failed to return a unique value");
        }

        /// <summary>
        /// Runs test for to string returns value.
        /// </summary>
        [TestMethod]
        public void ToStringReturnsValueTest()
        {
            MethodCacheKey target = new MethodCacheKey(GetType(), "Method", typeof(ArgumentNullException), typeof(String));

            String actual = target.ToString();

            Assert.AreEqual("MethodCacheKeyTests.Method(ArgumentNullException, String)", actual, "ToString returned an incorrect value");
        }

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///   The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}