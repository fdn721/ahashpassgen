using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace Common.Converters;

public class EnumToIndexConverter : IValueConverter
{
    public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
    {
        if( value == null )
            return null;

        if( targetType.IsEnum )
        {
            int index = System.Convert.ToInt32( value ); 
            var values = Enum.GetValues( targetType );
            return values.GetValue( index );
        }

        if( value.GetType().IsEnum )
        {
            var values = Enum.GetValues( value.GetType() );
            for( var i = 0; i < values.Length; i++ )
            {
                var enumValue = values.GetValue( i );
                if( enumValue == null )
                    continue;
                
                if( enumValue.Equals( value ) )
                    return i;
            }
        }

        return null;
    }

    public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
    {
        return Convert( value, targetType, parameter, culture );
    }
}
