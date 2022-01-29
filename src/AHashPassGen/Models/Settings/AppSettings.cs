using System;
using Avalonia;
using Common.Services.Settings;

namespace AHashPassGen.Models.Settings;

public class AppSettings : ISettings
{
    public int Version { get; set; } = 1;
    
    public double WindowWidth { get; set; } = 0;
    public double WindowHeight { get; set; } = 0;

    public double FontSize { get; set; } = 0;
    
    public void Validate()
    {
        if( Version != 1 )
            throw new FormatException( $"Unsupported settings version: {Version}." );
        
    }

    public void Update( AppSettings sett )
    {
        if( sett == null )
            throw new ArgumentNullException( nameof( sett ) );
        
        WindowWidth = sett.WindowWidth;
        WindowHeight = sett.WindowHeight;
        FontSize = sett.FontSize;
    }
    
    public AppSettings Clone()
    {
        var sett = new AppSettings();

        sett.WindowWidth = WindowWidth;
        sett.WindowHeight = WindowHeight;
        sett.FontSize = FontSize;

        return sett;
    }
    
    public bool Equals( AppSettings value)
    {
        return value.WindowWidth == WindowWidth &&
               value.WindowHeight == WindowHeight &&
               value.FontSize == FontSize;
    }
}
