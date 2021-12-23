using System;
using Common.Services.Settings;

namespace AHashPassGen.Models.Settings;

public class AppSettings : ISettings
{
    public int Version { get; set; } = 1;

    public void Validate()
    {
        if( Version != 1 )
            throw new FormatException( $"Unsupported settings version: {Version}." );
        
    }
}