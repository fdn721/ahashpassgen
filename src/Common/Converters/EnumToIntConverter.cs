using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Common.Converters
{
    public class EnumToIntConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null )
                return null;

            if( targetType.IsEnum )
                return Enum.ToObject( targetType, value );

            if( value.GetType().IsEnum )
                return System.Convert.ChangeType( value, Enum.GetUnderlyingType( value.GetType() ) );
            
            return null;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return Convert( value, targetType, parameter, culture );
        }
    }

}