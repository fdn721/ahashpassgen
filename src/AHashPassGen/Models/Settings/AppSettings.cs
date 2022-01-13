using System;
using Avalonia;
using Common.Services.Settings;

namespace AHashPassGen.Models.Settings;

public class AppSettings : ISettings
{
    public int Version { get; set; } = 1;
    
    public Size WindowSize { get; set; } = new Size( 0, 0 );
    public double FontSize { get; set; } = 0;
    
    public void Validate()
    {
        if( Version != 1 )
            throw new FormatException( $"Unsupported settings version: {Version}." );
        
    }
}