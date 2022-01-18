using System;
using System.Security.Cryptography;
using System.Text;

namespace AHashPassGen.Services;

public class CryptService : ICryptService
{
    private readonly SHA512 _hash = SHA512.Create();
    private readonly Aes _encoder = Aes.Create();
    private readonly object _sync = new object();
    public CryptService()
    {
    }
    
    public byte[] Encrypt( string key, string text )
    {
        lock( _sync )
        {
            var aesKey = CreateAesKey( key );
            _encoder.Key = aesKey;

            var textBytes = Encoding.UTF8.GetBytes( text );
            var encryptedBytes = _encoder.EncryptCbc( textBytes, CreateAesVi( key ) );

            return encryptedBytes;
        }
    }
    
    public string Decrypt( string key, byte[] data )
    {
        lock( _sync )
        {
            var aesKey = CreateAesKey( key );
            _encoder.Key = aesKey;

            var decryptedBytes = _encoder.DecryptCbc( data, CreateAesVi( key ) );
            var text = Encoding.UTF8.GetString( decryptedBytes );

            return text;
        }
    }

    public byte[] Hash( byte[] data, int offset, int count )
    {
        lock( _sync )
        {
            return _hash.ComputeHash( data, offset, count );
        }
    }

    private byte[] CreateAesKey( string key )
    {
        var keyBytes = Encoding.UTF8.GetBytes( key );
        var keyHash = _hash.ComputeHash( keyBytes );

        var keySize = _encoder.KeySize / 8;

        if( keyHash.Length < keySize )
            throw new InvalidOperationException( "Key size too small for encoding algorithm" );
        
        Array.Resize( ref keyHash, keySize );

        return keyHash;
    }
    
    private byte[] CreateAesVi( string key )
    {
        var keyBytes = Encoding.UTF8.GetBytes( key );
        var keyHash = _hash.ComputeHash( keyBytes );

        var viSize = _encoder.BlockSize / 8;

        if( keyHash.Length < viSize )
            throw new InvalidOperationException( "Key size too small for encoding algorithm" );
        
        Array.Resize( ref keyHash, viSize );

        return keyHash;
    }
}