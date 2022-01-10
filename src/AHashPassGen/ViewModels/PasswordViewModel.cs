using System;
using System.Reactive;
using Avalonia;
using Avalonia.Input.Platform;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AHashPassGen.ViewModels;

public class PasswordViewModel : ReactiveObject
{
    public event Action? CloseEvent;
    
    public ReactiveCommand<Unit, Unit> ShowCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    [Reactive] public string Password { get; set; }
    [Reactive] public bool VisibleShowButton { get; set; } = true;

    private readonly string _password;

    public PasswordViewModel( string password )
    {
        _password = password;
        Password = "".PadLeft( _password.Length, '*' );

        ShowCommand = ReactiveCommand.Create( ShowHandler );
        CopyCommand = ReactiveCommand.Create( CopyHandler );
        CloseCommand = ReactiveCommand.Create( CloseHandler );
    }

    private void ShowHandler()
    {
        Password = _password;
        VisibleShowButton = false;
    }
    private void CopyHandler()
    {
        Application.Current?.Clipboard?.SetTextAsync( _password );
    }
    private void CloseHandler()
    {
        CloseEvent?.Invoke();
    }
}