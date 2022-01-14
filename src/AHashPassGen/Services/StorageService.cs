using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using AHashPassGen.Models.Data;
using Newtonsoft.Json;

namespace AHashPassGen.Services;

public class StorageService : IStorageService
{
    public bool Exist => IsExist();

   
    private readonly string _fileName = "records.json";
    public StorageService()
    {
    }

    public RecordsFile Load( string password )
    {
        try
        {
            if( !File.Exists( _fileName ) )
                return new RecordsFile();

            using var fileStream = File.Open( _fileName, FileMode.Open, FileAccess.Read, FileShare.None );
            using var dataStream = new MemoryStream();
            fileStream.CopyTo( dataStream );
            fileStream.Close();

            var encryptedData = dataStream.ToArray();

            using var decoder = Aes.Create();
            decoder.Key = CalcHash( password );
            var decryptedData = decoder.DecryptEcb( encryptedData, PaddingMode.Zeros );
            var json = Encoding.UTF8.GetString( decryptedData );

            var recordsFile = JsonConvert.DeserializeObject<RecordsFile>( json );
            if( recordsFile == null )
                throw new Exception( "Can't deserialize json." );

            return recordsFile;

        }
        catch( JsonSerializationException err )
        {
            throw;
        }
        catch( Exception err )
        {
            throw new Exception( $"Can't load file: {_fileName}", err );
        }
    }
    
    public void Save( RecordsFile file, string password )
    {
        try
        {
            var json = JsonConvert.SerializeObject( file, Formatting.Indented );
            var decryptedData = Encoding.UTF8.GetBytes( json );
                
            using var decoder = Aes.Create();
            decoder.Key = CalcHash( password );
            var encryptedData = decoder.EncryptEcb( decryptedData, PaddingMode.Zeros );

            using var fileStream = File.Open( _fileName, FileMode.Create, FileAccess.Write, FileShare.None );
            fileStream.Write( encryptedData );
            fileStream.Close();
        }
        catch( Exception err )
        {
            throw new Exception( $"Can't save file: {_fileName}", err );
        }
    }
    
    private byte[] CalcHash( string value )
    {
        var hashAlgorithm = SHA512.Create();
        var inputData = Encoding.UTF8.GetBytes( value );
        var outputData = hashAlgorithm.ComputeHash( inputData );

        return outputData;
    }
    
    private bool IsExist()
    {
        try
        {
            return File.Exists( _fileName );
        }
        catch( Exception err )
        {
            return false;
        }
    }

}

/*
 
 
  var json = JsonConvert.SerializeObject( file, Formatting.Indented );
            File.WriteAllText( _fileName, json );
   using StreamReader r = new StreamReader( _fileName );
            var json = r

            var recordsFile = JsonConvert.DeserializeObject< RecordsFile >( json );
            if( recordsFile == null )
                throw new Exception( "Can't deserialize json." );

            return recordsFile;
 */