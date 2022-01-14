using System;
using Avalonia;
using Common.Services.Settings;
using Newtonsoft.Json;

namespace AHashPassGen.Models.Settings;

public class AppSettings : ISettings
{
    public int Version { get; set; } = 1;
    
    public Size WindowSize { get; set; } = new ( 0, 0 );
    
    public bool EncryptRecords { get; set; } = false;

    [JsonIgnore]
    public double FontSize
    {
        get => GetFontSize(); 
        set => SetFontSize( value );
    }
    
    public double FontSizeWindows { get; set; } = 0;
    public double FontSizeLinux { get; set; } = 0;
    public double FontSizeMacOS { get; set; } = 0;
    public double FontSizeOther { get; set; } = 0;
    
    
    public void Validate()
    {
        if( Version != 1 )
            throw new FormatException( $"Unsupported settings version: {Version}." );
        
    }

    public AppSettings Clone()
    {
        var sett = new AppSettings();

        sett.WindowSize = new Size( WindowSize.Width, WindowSize.Height );
        sett.EncryptRecords = EncryptRecords;
        sett.FontSizeLinux = FontSizeLinux;
        sett.FontSizeWindows = FontSizeWindows;
        sett.FontSizeMacOS = FontSizeMacOS;
        sett.FontSizeOther = FontSizeOther;
        
        return sett;
    }

    private double GetFontSize()
    {
        if( OperatingSystem.IsLinux() )
            return FontSizeLinux;
        
        if( OperatingSystem.IsWindows() )
            return FontSizeWindows;
        
        if( OperatingSystem.IsMacOS() )
            return FontSizeMacOS;
        
        return FontSizeOther;
    }
    
    private void SetFontSize( double value )
    {
        if( OperatingSystem.IsLinux() )
            FontSizeLinux = value;
        else if( OperatingSystem.IsWindows() )
            FontSizeWindows = value;
        else if( OperatingSystem.IsMacOS() )
            FontSizeMacOS = value;
        else
            FontSizeOther = value;
    }
}
