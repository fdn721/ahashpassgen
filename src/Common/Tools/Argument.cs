using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Common.Tools
{
    public static class Argument
    {
        public static T NotNull<T>( [MaybeNull]T value, [CallerArgumentExpression("value")] string name = "")
        {
            if( value is null )
                throw new ArgumentNullException( name );

            return value;
        }
        
        public static string NotNullOrEmpty( [MaybeNull]string value, [CallerArgumentExpression("value")] string name = "" )
        {
            if( string.IsNullOrEmpty( value ) )
                throw new ArgumentException( $"Argument {name} can't be null or empty" );
            
            return value;
        }
        
        public static string NotNullOrWhiteSpace( [MaybeNull]string value, [CallerArgumentExpression("value")] string name = "" )
        {
            if( string.IsNullOrWhiteSpace( value ) )
                throw new ArgumentException( $"Argument {name} can't be null, empty or whitespace" );
            
            return value;
        }
        
        public static T NotNegative< T >( T value, [CallerArgumentExpression("value")] string name = "" ) where T : IComparable
        {
            if( value.CompareTo( 0 ) < 0 )
                throw new ArgumentException( $"Argument {name} can't be less zero" );
            
            return value;
        }
    }
    
}