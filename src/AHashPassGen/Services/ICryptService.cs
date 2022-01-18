namespace AHashPassGen.Services;

public interface ICryptService
{
    byte[] Encrypt( string key, string text );
    string Decrypt( string key, byte[] data );
    byte[] Hash( byte[] data, int offset, int count );
}