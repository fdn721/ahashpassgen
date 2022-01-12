using System;
using System.Collections.Generic;
using System.IO;
using AHashPassGen.Models.Data;
using Newtonsoft.Json;

namespace AHashPassGen.Services;

public class StorageService : IStorageService
{
    private readonly string _fileName = "records.json";
    public StorageService()
    {
    }

    public RecordsFile Load()
    {
        try
        {
            if( !File.Exists( _fileName ) )
                return new RecordsFile();
            
            using StreamReader r = new StreamReader( _fileName );
            var json = r.ReadToEnd();

            var recordsFile = JsonConvert.DeserializeObject< RecordsFile >( json );
            if( recordsFile == null )
                throw new Exception( "Can't deserialize json." );

            return recordsFile;
        }
        catch( Exception err )
        {
            throw new Exception( $"Can't load file: {_fileName}", err );
        }
    }
    
    public void Save( RecordsFile file )
    {
        try
        {
            var json = JsonConvert.SerializeObject( file, Formatting.Indented );
            File.WriteAllText( _fileName, json );
        }
        catch( Exception err )
        {
            throw new Exception( $"Can't save file: {_fileName}", err );
        }

    }
}