using System;
using System.Drawing;
using AHashPassGen.Models.Settings;
using AHashPassGen.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AHashPassGen.ViewModels;
using AHashPassGen.Views;
using Avalonia.Controls;
using Common.Services.Dialog;
using Common.Services.Settings;
using HarfBuzzSharp;
using ReactiveUI;
using Splat;

namespace AHashPassGen
{
    public class App : Application
    {
        public static double FontSize { get; set; }
        public static double FontSizeH3  => FontSize + 1;
        public static double FontSizeH2  => FontSize + 2;
        public static double FontSizeH1  => FontSize + 3;
        
        
        public override void Initialize()
        {
            InitDI();
            InitSettings();

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
         

            if( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void InitDI()
        {
            Locator.CurrentMutable.Register( () => new EditRecordView(), typeof( IViewFor< EditRecordViewModel > ) );
            Locator.CurrentMutable.Register( () => new AboutView(), typeof( IViewFor< AboutViewModel > ) );
            Locator.CurrentMutable.Register( () => new PasswordView(), typeof( IViewFor< PasswordViewModel > ) );
            Locator.CurrentMutable.Register( () => new MasterPasswordView(), typeof( IViewFor< MasterPasswordViewModel > ) );
            Locator.CurrentMutable.Register( () => new PropertiesView(), typeof( IViewFor< PropertiesViewModel > ) );
            Locator.CurrentMutable.RegisterConstant( new DialogService(), typeof( IDialogService ) );
            Locator.CurrentMutable.RegisterConstant( new SettingsService< AppSettings >(), typeof( ISettingsService< AppSettings > ) );
            Locator.CurrentMutable.RegisterConstant( new CryptService(), typeof( ICryptService ) );
            Locator.CurrentMutable.RegisterConstant( new PasswordService(), typeof( IPasswordService ) );
            Locator.CurrentMutable.RegisterConstant( new StorageService(), typeof( IStorageService ) );
        }

        private void InitSettings()
        {
            var settingsService = Locator.Current.GetService< ISettingsService< AppSettings > >();
            
            try
            {
                if( settingsService != null )
                    settingsService.Load();
            }
            catch( Exception )
            { }
            
            if( settingsService != null )
                ApplyFontSize( settingsService.Current.FontSize );
        }
        
        private void ApplyFontSize( double fontSize )
        {
            if( fontSize != 0 )
            {
                FontSize = fontSize;
                return;
            }
            
            var textBlock = new TextBlock();
            FontSize = textBlock.FontSize;
        }

    }
}