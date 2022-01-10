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
    
    public event Action<string?>? CloseEvent;

    private readonly string _masterHash;
    private readonly IPasswordService _passwordService;
    private readonly IDialogService _dialogService;

    public MasterPasswordViewModel( string masterHash, IPasswordService? passwordService = null, IDialogService? dialogService = null )
    {
        _masterHash = masterHash;
        _passwordService = passwordService ?? Locator.Current.GetService< IPasswordService >() ?? throw new ArgumentNullException( nameof( passwordService ) );
        _dialogService = dialogService ?? Locator.Current.GetService< IDialogService >() ?? throw new ArgumentNullException( nameof( dialogService ) );

        CreateMode = string.IsNullOrEmpty( _masterHash );

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


        var masterHash = _passwordService.CalcHash( Password );
        if( masterHash == _masterHash )
        {
            CloseEvent?.Invoke( Password );
            return;
        }

        _dialogService.Error( I18n.Error, $"{I18n.ThePasswordIsDifferentFromTheOneUsedPreviously}!" );
    }
    
    private void CancelHandler()
    {
        CloseEvent?.Invoke( null );
    }
}