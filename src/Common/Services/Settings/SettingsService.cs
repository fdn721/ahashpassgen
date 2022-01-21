using System;
using System.IO;
using System.Diagnostics;
using System.Text.Json;
using Common.Properies;

namespace Common.Services.Settings;

public class SettingsService< T > : ISettingsService<T> where T : ISettings, new()
{
    public T Current { get; protected set; } = new T();
    
    public object Sync => _sync;
    public string ConfigName { get; set; } = "settings";
    public string AppName { get; set; } = Process.GetCurrentProcess().MainModule?.ModuleName ?? "app";

    private readonly object _sync = new object();

    public SettingsService()
    {
    }

    public bool Load()
    {
        var filePath = BuildFilePath();

        try
        {
            lock( _sync )
            {
                if( !File.Exists( filePath ) )
                    return false;
                
                using StreamReader r = new StreamReader( filePath );
                var json = r.ReadToEnd();

                var settings = JsonSerializer.Deserialize<T>( json );
                if( settings == null )
                    throw new Exception( $"{I18n.CantDeserializeSettingsFile}: {filePath}" );

                settings.Validate();

                Current = settings;
                return true;
            }
        }
        catch( Exception err )
        {
            throw new Exception( $"{I18n.CantLoadSettingsFile}: {filePath}", err );
        }
    }

    public void Save()
    {
        var filePath = BuildFilePath();
        
        try
        {
            lock( _sync )
            {
                var dirPath = Path.GetDirectoryName( filePath );
                
                if( dirPath != null && !Directory.Exists( dirPath ) )
                    Directory.CreateDirectory( dirPath );
                
                var json = JsonSerializer.Serialize( Current,  new JsonSerializerOptions{ WriteIndented = true } );

                using Stream stream = File.Open( filePath, FileMode.Create, FileAccess.Write, FileShare.None );
                using TextWriter textWriter = new StreamWriter(stream);
                textWriter.Write( json );
                stream.Flush();
            }
        }
        catch( Exception err )
        {
            throw new Exception( $"{I18n.CantSaveSettingsFile}: {filePath}", err );
        }
    }

    private string BuildFilePath()
    {
        return Path.Join( Environment.GetFolderPath( Environment.SpecialFolder.Personal ), ".config", 
            AppName, ConfigName + ".json" );
    }
}

