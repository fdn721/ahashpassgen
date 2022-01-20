using System;
using System.Reactive;
using AHashPassGen.Services;
using AHashPassGen.Properties;
using Common.Services.Dialog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AHashPassGen.ViewModels;

public class MasterPasswordViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> OkCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    
    [Reactive] public bool CreateMode { get; set; }
    [Reactive] public string Password { get; set; } = "";
    [Reactive] public string PasswordConfirm { get; set; } = "";
    public string PasswordWatermark => CreateMode ? I18n.Minimum8Сharacters : "";
    public event Action<string?>? CloseEvent;
    
    private readonly IDialogService _dialogService;

    public MasterPasswordViewModel( string password, bool createMode, IDialogService? dialogService = null )
    {
        Password = password;
        PasswordConfirm = password;
        CreateMode = createMode;
        _dialogService = dialogService ?? Locator.Current.GetService< IDialogService >() ?? throw new ArgumentNullException( nameof( dialogService ) );
        
        if( CreateMode )
        {
            OkCommand = ReactiveCommand.Create( OkHandler, this.WhenAnyValue( x => x.Password, y => y.PasswordConfirm,
                ( pass, passConfirm ) => pass == passConfirm && pass.Length >= 1 ) );
        }
        else
        {
            OkCommand = ReactiveCommand.Create( OkHandler, this.WhenAny( x => x.Password, _ => !string.IsNullOrEmpty( Password ) ) );
        }

        CancelCommand = ReactiveCommand.Create( CancelHandler );
    }

    private void OkHandler()
    {
        if( CreateMode )
        {
            CloseEvent?.Invoke( Password );
            return;
        }
        
        CloseEvent?.Invoke( Password );
    }
    
    private void CancelHandler()
    {
        CloseEvent?.Invoke( null );
    }
}