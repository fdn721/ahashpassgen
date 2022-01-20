using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using AHashPassGen.Models.Data;
using Newtonsoft.Json;
using Splat;

namespace AHashPassGen.Services;

public class StorageService : IStorageService
{
    public bool Exist => IsExist();

   
    private readonly ICryptService _cryptService;
    private readonly string _fileName = "records.json";
    private readonly string _oldFileName = "records.json.old";
    public StorageService( ICryptService? cryptService = null )
    {
        _cryptService =  cryptService ?? Locator.Current.GetService<ICryptService>() ?? throw new ArgumentNullException( nameof( cryptService ) );
    }

    public RecordsFile Load( string password )
    {
        try
        {
            if( !File.Exists( _fileName ) )
                return new RecordsFile();
            
            try
            {
                var encryptedData = LoadFile( _fileName );
                return Decode( password, encryptedData );
            }
            catch( Exception err )
            {
                if( !File.Exists( _oldFileName ) )
                    throw;
               
                var encryptedData = LoadFile( _oldFileName );
                return Decode( password, encryptedData );
            }
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
    
    public void Save( RecordsFile recordsFile, string password )
    {
        try
        {
            var encryptedData = Encode( password, recordsFile );
            
            if( File.Exists( _oldFileName ) )
                File.Delete( _oldFileName );
            
            if( File.Exists( _fileName ) )
                File.Move( _fileName, _oldFileName );

            SaveFile( _fileName,  encryptedData );
        }
        catch( Exception err )
        {
            throw new Exception( $"Can't save file: {_fileName}", err );
        }
    }

    private byte[] LoadFile( string fileName )
    {
        using var fileStream = File.Open( _fileName, FileMode.Open, FileAccess.Read, FileShare.None );
        using var dataStream = new MemoryStream();
        fileStream.CopyTo( dataStream );
        fileStream.Close();
            
       return dataStream.ToArray();
    }
    
    private void SaveFile( string fileName, byte[] encryptedData )
    {
        using var fileStream = File.Open( _fileName, FileMode.Create, FileAccess.Write, FileShare.None );
        fileStream.Write( encryptedData );
        fileStream.Close();
    }

    private RecordsFile Decode( string password,  byte[] encryptedData )
    {
        var json = _cryptService.Decrypt( password, encryptedData );
        
        var recordsFile = JsonConvert.DeserializeObject<RecordsFile>( json );
        if( recordsFile == null )
            throw new Exception( "Can't deserialize json." );

        return recordsFile;
    }
    
    private byte[] Encode( string password,  RecordsFile  recordsFile )
    {
        var json = JsonConvert.SerializeObject( recordsFile, Formatting.Indented );
        return _cryptService.Encrypt( password, json );
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