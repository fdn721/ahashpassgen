using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Text;
using AHashPassGen.Models.Settings;
using AHashPassGen.Properties;
using Avalonia.MessageBox;
using Common.Services.Dialog;
using Common.Services.Settings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AHashPassGen.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public event Action? CloseEvent;
       
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        [Reactive] public ReactiveCommand< CancelEventArgs, Unit > ExitCommand { get; set; }
        [Reactive] public ReactiveCommand< Unit, Unit > AboutCommand { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

        public string Greeting => "Welcome to Avalonia!";
        
        private bool _forceClose;
        private readonly IDialogService _dialogService;
        private readonly ISettingsService< AppSettings > _settingsService;
        
         public MainWindowViewModel( IDialogService? dialogService = null,
                                    ISettingsService< AppSettings >? settingsService = null )
        {
            _dialogService = dialogService ?? Locator.Current.GetService< IDialogService >() ?? throw new ArgumentNullException( nameof( dialogService ) );
            _settingsService = settingsService ?? Locator.Current.GetService< ISettingsService< AppSettings > >() ?? throw new ArgumentNullException( nameof( settingsService ) );

            _settingsService.Load();
            
        
            ExitCommand = ReactiveCommand.Create< CancelEventArgs >( ExitHandler );
            AboutCommand = ReactiveCommand.Create( AboutHandler );
        }
         
         private async void ExitHandler( CancelEventArgs arg)
         {
             if( _forceClose )
                 return;
            
             arg.Cancel = true;
            
            
             var result = await _dialogService.Question( I18n.Closing, I18n.CloseApplication );

             if( result != MessageBoxResult.Yes )
                 return;
             

             _forceClose = true;
             CloseEvent?.Invoke();
         }
        
         private async void AboutHandler()
         {
             await _dialogService.Show( new AboutViewModel() );
         }
    }
}
