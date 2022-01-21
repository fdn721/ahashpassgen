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

        FontSize = _settings.FontSize < 8 || _settings.FontSize > 20  ? 0 : ( int ) _settings.FontSize - 7;
        
        ApplyCommand = ReactiveCommand.Create( ApplyHandler/*,
            this.WhenAnyValue( x => x.Site, y => y.Login,
                ( text, translate ) => !string.IsNullOrEmpty( text ) && !string.IsNullOrEmpty( translate ) )*/ );

        CloseCommand = ReactiveCommand.Create( CloseHandler );
    }

    private void ApplyHandler()
    {
        _settings.FontSize = FontSize == 0 ? 0 : FontSize + 7;
        CloseEvent?.Invoke( _settings );
    }
    
    private void CloseHandler()
    {
        CloseEvent?.Invoke( null );
    }
}