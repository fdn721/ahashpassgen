using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Common.Converters
{
    public class BoolToIntConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null )
                return null;

            if( targetType == typeof( bool ) )
                return ( int )value != 0 ;

            if( value is bool boolValue )
                return boolValue ? 1 : 0;
            
            return null;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return Convert( value, targetType, parameter, culture );
        }
    }

}