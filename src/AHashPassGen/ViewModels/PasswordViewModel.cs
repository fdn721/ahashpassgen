using System;
using System.Reactive;
using AHashPassGen.Properties;
using Avalonia;
using Avalonia.Input.Platform;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AHashPassGen.ViewModels;

public class PasswordViewModel : ReactiveObject
{
    public event Action? CloseEvent;
    
    public ReactiveCommand<Unit, Unit> ShowCommand { get; }
    public ReactiveCommand<Unit, Unit> CopySiteCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyLoginCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyPasswordCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }


    public string SiteName => $"{I18n.Site}:";
    public string LoginName => $"{I18n.Login}:";
    public string PasswordName => $"{I18n.Password}:";
    
    [Reactive] public string Site { get; set; }
    [Reactive] public string Login { get; set; }
    [Reactive] public string Password { get; set; }
    [Reactive] public bool VisibleShowButton { get; set; } = true;

    private readonly string _password;

    public PasswordViewModel( string site, string login, string password )
    {
        Site = site;
        Login = login;
        _password = password;
        Password = "".PadLeft( _password.Length, '*' );

        ShowCommand = ReactiveCommand.Create( ShowHandler );
        CopySiteCommand = ReactiveCommand.Create( CopySiteHandler );
        CopyLoginCommand = ReactiveCommand.Create( CopyLoginHandler );
        CopyPasswordCommand = ReactiveCommand.Create( CopyPasswordHandler );
        CloseCommand = ReactiveCommand.Create( CloseHandler );
    }

    private void ShowHandler()
    {
        Password = _password;
        VisibleShowButton = false;
    }
    private void CopySiteHandler()
    {
        Application.Current?.Clipboard?.SetTextAsync( Site );
    }
    
    private void CopyLoginHandler()
    {
        Application.Current?.Clipboard?.SetTextAsync( Login );
    }
    private void CopyPasswordHandler()
    {
        Application.Current?.Clipboard?.SetTextAsync( _password );
    }
    private void CloseHandler()
    {
        CloseEvent?.Invoke();
    }
}