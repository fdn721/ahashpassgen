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
        public static double FontSize  =>  GetFontSize();
        public static double FontSizeH3  =>  GetFontSize() + 1;
        public static double FontSizeH2  =>  GetFontSize() + 2;
        public static double FontSizeH1  =>  GetFontSize() + 3;
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            
            
        }

        public override void OnFrameworkInitializationCompleted()
        {
            
            Locator.CurrentMutable.Register( () => new EditRecordView(), typeof( IViewFor< EditRecordViewModel > ) );
            Locator.CurrentMutable.Register( () => new AboutView(), typeof( IViewFor< AboutViewModel > ) );
            Locator.CurrentMutable.Register( () => new PasswordView(), typeof( IViewFor< PasswordViewModel > ) );
            Locator.CurrentMutable.Register( () => new MasterPasswordView(), typeof( IViewFor< MasterPasswordViewModel > ) );
            Locator.CurrentMutable.Register( () => new PropertiesView(), typeof( IViewFor< PropertiesViewModel > ) );
            Locator.CurrentMutable.RegisterConstant( new PasswordService(), typeof( IPasswordService ) );
            Locator.CurrentMutable.RegisterConstant( new SettingsService< AppSettings >(), typeof( ISettingsService< AppSettings > ) );
            Locator.CurrentMutable.RegisterConstant( new DialogService(), typeof( IDialogService ) );
            Locator.CurrentMutable.RegisterConstant( new StorageService(), typeof( IStorageService ) );
            
            if( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        private static double GetFontSize()
        {
            var textBlock = new TextBlock();

            if( System.OperatingSystem.IsLinux() )
                return 16;

            if( System.OperatingSystem.IsWindows() )
                return textBlock.FontSize;
            
            return textBlock.FontSize;
        }

    }
}