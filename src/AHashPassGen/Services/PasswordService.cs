using System;
using System.Security.Cryptography;
using System.Text;
using AHashPassGen.Models.Data;

namespace AHashPassGen.Services;

public class PasswordService : IPasswordService
{
    public string MasterPassword { get; set; } = "";

    private readonly byte[] _alphabet = new byte[74];

    public PasswordService()
    {
        BuildAlphabet();
    }

    public string CalcHash( string value )
    {
        var hashAlgorithm = SHA512.Create();
        var inputData = Encoding.UTF8.GetBytes( value );
        var outputData = hashAlgorithm.ComputeHash( inputData );

        return BitConverter.ToString( outputData ).Replace( "-", "" );
    }

    public string Generate( Record record )
    {
        int charsCount = Convert.ToInt32( record.Alphabet );
        
        var inputData = Encoding.UTF8.GetBytes( $"{MasterPassword}{record.Site}{record.Login}" );

        var hashAlgorithm = SHA512.Create(); // new SHA512Managed();

        for( var i = 0; i < record.StageCount; i++ )
            inputData = hashAlgorithm.ComputeHash( inputData );


        for( var i = 0; i < inputData.Length; i++ )
        {
            var value = Convert.ToDouble( inputData[i] );
            value = value * ( charsCount - 1 ) / 255;
            inputData[i] = Convert.ToByte( value );
        }

        for( int i = 0; i < inputData.Length; i++ )            
            inputData[i] = _alphabet[ inputData[i] ];
            
        string outputString = Encoding.ASCII.GetString( inputData );

        return outputString.Substring( 0, record.Length );
    }

    private void BuildAlphabet()
    {
        for( var i = 0; i < 10; i++ )
            _alphabet[i] = ( byte )( Encoding.ASCII.GetBytes( "0" )[0] + i );

        for( var i = 0; i < 26; i++ )
            _alphabet[i + 10] = ( byte )( Encoding.ASCII.GetBytes( "a" )[0] + i );

        for( var i = 0; i < 26; i++ )
            _alphabet[i + 36] = ( byte )( Encoding.ASCII.GetBytes( "A" )[0] + i );

        _alphabet[62] = Encoding.ASCII.GetBytes( "-" )[0];
        _alphabet[63] = Encoding.ASCII.GetBytes( "=" )[0];
        _alphabet[64] = Encoding.ASCII.GetBytes( "!" )[0];
        _alphabet[65] = Encoding.ASCII.GetBytes( "@" )[0];
        _alphabet[66] = Encoding.ASCII.GetBytes( "#" )[0];
        _alphabet[67] = Encoding.ASCII.GetBytes( "$" )[0];
        _alphabet[68] = Encoding.ASCII.GetBytes( "%" )[0];
        _alphabet[69] = Encoding.ASCII.GetBytes( "^" )[0];
        _alphabet[70] = Encoding.ASCII.GetBytes( "&" )[0];
        _alphabet[71] = Encoding.ASCII.GetBytes( "*" )[0];
        _alphabet[72] = Encoding.ASCII.GetBytes( "(" )[0];
        _alphabet[73] = Encoding.ASCII.GetBytes( ")" )[0];
    }
}