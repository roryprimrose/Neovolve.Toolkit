namespace Neovolve.Toolkit
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The <see cref="Extensions"/>
    ///   class is used to expose common extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///   Defines the regular expression value for format mask values.
        /// </summary>
        private const String FormatMaskIndexExpressionValue = @"(?<!\{)\{(?<FormatMaskIndex>\d+)(,-?\d+)?(:[\w\d]+)?\}(?!\})";

        /// <summary>
        ///   Stores the regular expression used to identify format mask values.
        /// </summary>
        private static readonly Regex _formatMaskIndexExpression = new Regex(
            FormatMaskIndexExpressionValue, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Converts a <see cref="String"/> to a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="insecureValue">
        /// The insecure value.
        /// </param>
        /// <returns>
        /// A <see cref="SecureString"/> value.
        /// </returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", 
            Justification = "The instance cannot be disposed as it is the return value.")]
        public static SecureString ConvertToSecureString(this String insecureValue)
        {
            if (insecureValue == null)
            {
                return null;
            }

            unsafe
            {
                fixed (Char* passwordChars = insecureValue)
                {
                    SecureString securePassword = new SecureString(passwordChars, insecureValue.Length);

                    securePassword.MakeReadOnly();

                    return securePassword;
                }
            }
        }

        /// <summary>
        /// Converts a <see cref="SecureString"/> to <see cref="String"/>.
        /// </summary>
        /// <param name="secureValue">
        /// The secure value.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        public static String ConvertToString(this SecureString secureValue)
        {
            if (secureValue == null)
            {
                return null;
            }

            IntPtr unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureValue);

                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Formats the string using the value of <c>{null}</c> for any arguments that are either <c>null</c> or not provided.
        /// </summary>
        /// <param name="format">
        /// The format string.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <param name="args">
        /// The arguments used to format the string.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        /// <remarks>
        /// This method does not require that the number of arguments is the same or greater than the maximum format mask index.
        ///   Any format argument not provided, or provided as a <c>null</c> value will be injected into the result string as
        ///   the value <c>{null}</c>.
        /// </remarks>
        public static String FormatNullMasks(this String format, IFormatProvider formatProvider, params Object[] args)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return String.Empty;
            }

            Int32 maskCount = format.GetFormatMaskCount();

            return FormatNullMasksInternal(format, formatProvider, maskCount, args);
        }

        /// <summary>
        /// Gets the count of format mask arguments supported by the specified string.
        /// </summary>
        /// <param name="value">
        /// The format value.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method determines the maximum number of format arguments supported in a format string.
        ///     The calculation of format mask index values supports the 
        ///     <a href="http://msdn.microsoft.com/en-us/library/txafckwd.aspx" target="_blank">composite format string syntax</a> 
        ///     described in the MSDN documentation.
        ///   </para>
        /// <para>
        /// The logic in this method will skip any format masks that are missing in order to determine the maximum count of arguments supported by the string.
        ///     A value of <c>"Pre{0}Post"</c> will return 1 whereas <c>"Pre{0} {5}Post"</c> will return 6.
        ///   </para>
        /// </remarks>
        public static Int32 GetFormatMaskCount(this String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            Int32 maxMaskIndex = 0;

            MatchCollection matches = _formatMaskIndexExpression.Matches(value);

            foreach (Match match in matches)
            {
                Group maskIndexGroup = match.Groups["FormatMaskIndex"];
                String maskIndexValue = maskIndexGroup.Value;
                Int32 maskIndex;

                if (Int32.TryParse(maskIndexValue, out maskIndex) == false)
                {
                    continue;
                }

                // Increment to convert from zero based index to 1 based count
                maskIndex++;

                maxMaskIndex = Math.Max(maxMaskIndex, maskIndex);
            }

            return maxMaskIndex;
        }

        /// <summary>
        /// Formats the null masks.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <param name="maskCount">
        /// The mask count.
        /// </param>
        /// <param name="args">
        /// The arguments used to format the string.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        internal static String FormatNullMasksInternal(this String format, IFormatProvider formatProvider, Int32 maskCount, params Object[] args)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return String.Empty;
            }

            Int32 argumentCount = 0;
            Object[] formatArguments;

            if (args != null)
            {
                argumentCount = args.Length;
            }
            else
            {
                args = new Object[0];
            }

            if (maskCount > argumentCount)
            {
                formatArguments = new Object[maskCount];

                args.CopyTo(formatArguments, 0);
            }
            else
            {
                formatArguments = args;
            }

            for (Int32 index = 0; index < formatArguments.Length; index++)
            {
                if (formatArguments[index] == null)
                {
                    formatArguments[index] = "{null}";
                }
            }

            Contract.Assume(String.IsNullOrWhiteSpace(format) == false);

            return String.Format(formatProvider, format, formatArguments);
        }
    }
}