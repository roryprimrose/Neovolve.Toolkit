namespace Neovolve.Toolkit.Workflow.Design.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverItemVisibilityConverter"/>
    ///   class is used to convert to the <see cref="Visibility"/> type for an
    ///   <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> activity.
    /// </summary>
    public class InstanceResolverItemVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">
        /// The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            Int32 argumentCount = (Int32)value;
            Int32 propertyItem = Int32.Parse((String)parameter, CultureInfo.CurrentCulture);

            // Property item is 1 based, not zero based
            if (propertyItem <= argumentCount)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">
        /// The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}