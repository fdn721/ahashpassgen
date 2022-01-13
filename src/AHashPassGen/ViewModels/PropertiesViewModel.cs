using System;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AHashPassGen.ViewModels;

public class PropertiesViewModel : ReactiveObject
{
    public event Action? CloseEvent;
    
    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    [Reactive] public bool EncryptRecords { get; set; }

    public PropertiesViewModel()
    {
        ApplyCommand = ReactiveCommand.Create( ApplyHandler/*,
            this.WhenAnyValue( x => x.Site, y => y.Login,
                ( text, translate ) => !string.IsNullOrEmpty( text ) && !string.IsNullOrEmpty( translate ) )*/ );

        CloseCommand = ReactiveCommand.Create( CloseHandler );
    }

    private void ApplyHandler()
    {
    }
    
    private void CloseHandler()
    {
        CloseEvent?.Invoke();
    }
}