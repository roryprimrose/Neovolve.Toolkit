namespace Neovolve.Toolkit.UnitTests
{
    using System;
    using System.Globalization;
    using System.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="ExtensionsTests"/>
    ///   class is used to test the <see cref="Extensions"/> class.
    /// </summary>
    [TestClass]
    public class ExtensionsTests
    {
        /// <summary>
        /// Runs test for extensions convert to secure string returns an instance for empty string.
        /// </summary>
        [TestMethod]
        public void ExtensionsConvertToSecureStringReturnsAnInstanceForEmptyStringTest()
        {
            String target = String.Empty;

            SecureString actual = target.ConvertToSecureString();

            Assert.IsNotNull(actual, "ConvertToSecureString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions convert to secure string returns an instance for string with value.
        /// </summary>
        [TestMethod]
        public void ExtensionsConvertToSecureStringReturnsAnInstanceForStringWithValueTest()
        {
            String target = Guid.NewGuid().ToString();

            SecureString actual = target.ConvertToSecureString();

            Assert.IsNotNull(actual, "ConvertToSecureString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions convert to secure string returns an instance for white space string.
        /// </summary>
        [TestMethod]
        public void ExtensionsConvertToSecureStringReturnsAnInstanceForWhiteSpaceStringTest()
        {
            const String Target = " ";

            SecureString actual = Target.ConvertToSecureString();

            Assert.IsNotNull(actual, "ConvertToSecureString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions convert to secure string returns null for null string.
        /// </summary>
        [TestMethod]
        public void ExtensionsConvertToSecureStringReturnsNullForNullStringTest()
        {
            String target = null;

            SecureString actual = target.ConvertToSecureString();

            Assert.IsNull(actual, "ConvertToSecureString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions convert to string returns null for null secure string.
        /// </summary>
        [TestMethod]
        public void ExtensionsConvertToStringReturnsNullForNullSecureStringTest()
        {
            SecureString target = null;

            String actual = target.ConvertToString();

            Assert.IsNull(actual, "ConvertToString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks fills null mask for values not provided.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksFillsNullMaskForValuesNotProvidedTest()
        {
            const String Format = "Pre{0}Post{1}asdf{4}";
            const String Expected = "Pre1Post{null}asdf{null}";

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture, 1);

            Assert.AreEqual(Expected, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks ignores values without defined mask.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksIgnoresValuesWithoutDefinedMaskTest()
        {
            const String Format = "Pre{0}Post{1}";
            const String Expected = "Pre1PostA";

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture, 1, "A", true, Guid.NewGuid());

            Assert.AreEqual(Expected, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks returns empty for empty format.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksReturnsEmptyForEmptyFormatTest()
        {
            String format = String.Empty;

            String actual = format.FormatNullMasks(CultureInfo.InvariantCulture, 1, "A", true, Guid.NewGuid());

            Assert.AreEqual(String.Empty, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks returns empty for null format.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksReturnsEmptyForNullFormatTest()
        {
            const String Format = null;

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture, 1, "A", true, Guid.NewGuid());

            Assert.AreEqual(String.Empty, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks returns empty for white space format.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksReturnsEmptyForWhiteSpaceFormatTest()
        {
            const String Format = " ";

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture, 1, "A", true, Guid.NewGuid());

            Assert.AreEqual(String.Empty, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks returns formatted value for matching masks.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksReturnsFormattedValueForMatchingMasksTest()
        {
            const String Format = "Pre{0}Post{1}";
            const String Expected = "Pre1PostA";

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture, 1, "A");

            Assert.AreEqual(Expected, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks returns null mask for no value provided.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksReturnsNullMaskForNoValueProvidedTest()
        {
            const String Format = "Pre{0}Post";
            const String Expected = "Pre{null}Post";

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture);

            Assert.AreEqual(Expected, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions format null masks returns null mask for null value.
        /// </summary>
        [TestMethod]
        public void ExtensionsFormatNullMasksReturnsNullMaskForNullValueTest()
        {
            const String Format = "Pre{0}Post";
            const String Expected = "Pre{null}Post";

            String actual = Format.FormatNullMasks(CultureInfo.InvariantCulture, null);

            Assert.AreEqual(Expected, actual, "FormatNullMasks returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for empty format mask.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForEmptyFormatMaskTest()
        {
            String format = String.Empty;
            Int32 actual = format.GetFormatMaskCount();

            Assert.AreEqual(0, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for escaped end format mask.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForEscapedEndFormatMaskTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22,123:AS1234DF}}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(1, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for escaped format mask.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForEscapedFormatMaskTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{{22,123:AS1234DF}}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(1, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for escaped format mask.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForEscapedStartFormatMaskTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{{22,123:AS1234DF}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(1, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for fragmented mask with format component defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForFragmentedMaskWithFormatComponentDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22:AS1234DF}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(23, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for fragmented mask with negative alignment and format component defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForFragmentedMaskWithNegativeAlignmentAndFormatComponentDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22,-123:AS1243DF}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(23, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for fragmented mask with negative alignment defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForFragmentedMaskWithNegativeAlignmentDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22,-123}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(23, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for fragmented mask with positive alignment and format component defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForFragmentedMaskWithPositiveAlignmentAndFormatComponentDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22,123:AS1234DF}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(23, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for fragmented mask with positive alignment defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForFragmentedMaskWithPositiveAlignmentDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22,123}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(23, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for null format mask.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForNullFormatMaskTest()
        {
            const String Format = null;
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(0, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for single mask defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForSingleMaskDefinedTest()
        {
            const String Format = "SomeFormat{0} value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(1, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for two masks defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForTwoFragmentedMasksDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{22}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(23, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns correct value for two masks defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsCorrectValueForTwoMasksDefinedTest()
        {
            const String Format = "SomeFormat{0} asdfasdf{1}value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(2, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions get format mask count returns zero for no masks defined.
        /// </summary>
        [TestMethod]
        public void ExtensionsGetFormatMaskCountReturnsZeroForNoMasksDefinedTest()
        {
            const String Format = "SomeFormat value";
            Int32 actual = Format.GetFormatMaskCount();

            Assert.AreEqual(0, actual, "GetFormatMaskCount returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions secure string conversions correctly handle empty string.
        /// </summary>
        [TestMethod]
        public void ExtensionsSecureStringConversionsCorrectlyHandleEmptyStringTest()
        {
            String expected = String.Empty;
            SecureString secureValue = expected.ConvertToSecureString();
            String actual = secureValue.ConvertToString();

            Assert.AreEqual(expected, actual, "ConvertToString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions secure string conversions correctly handle empty string.
        /// </summary>
        [TestMethod]
        public void ExtensionsSecureStringConversionsCorrectlyHandleStringValueTest()
        {
            String expected = Guid.NewGuid().ToString();
            SecureString secureValue = expected.ConvertToSecureString();
            String actual = secureValue.ConvertToString();

            Assert.AreEqual(expected, actual, "ConvertToString returned an incorrect value");
        }

        /// <summary>
        /// Runs test for extensions secure string conversions correctly handle white space string.
        /// </summary>
        [TestMethod]
        public void ExtensionsSecureStringConversionsCorrectlyHandleWhiteSpaceStringTest()
        {
            const String Expected = " ";
            SecureString secureValue = Expected.ConvertToSecureString();
            String actual = secureValue.ConvertToString();

            Assert.AreEqual(Expected, actual, "ConvertToString returned an incorrect value");
        }
    }
}