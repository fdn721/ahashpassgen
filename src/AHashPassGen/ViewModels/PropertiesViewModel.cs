using System;
using System.Reactive;
using AHashPassGen.Models.Settings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AHashPassGen.ViewModels;

public class PropertiesViewModel : ReactiveObject
{
    public event Action< AppSettings? >? CloseEvent;
    
    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    [Reactive] public int FontSize  { get; set; }

    private AppSettings _settings;
    
    public PropertiesViewModel( AppSettings settings )
    {
        _settings = settings.Clone();

        FontSize = ( int )_settings.FontSize;
        
        ApplyCommand = ReactiveCommand.Create( ApplyHandler/*,
            this.WhenAnyValue( x => x.Site, y => y.Login,
                ( text, translate ) => !string.IsNullOrEmpty( text ) && !string.IsNullOrEmpty( translate ) )*/ );

        CloseCommand = ReactiveCommand.Create( CloseHandler );
    }

    private void ApplyHandler()
    {
        _settings.FontSize = FontSize;
        CloseEvent?.Invoke( _settings );
    }
    
    private void CloseHandler()
    {
        CloseEvent?.Invoke( null );
    }
}