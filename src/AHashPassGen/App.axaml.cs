using AHashPassGen.Models.Settings;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AHashPassGen.ViewModels;
using AHashPassGen.Views;
using Avalonia.Controls;
using Common.Services.Dialog;
using Common.Services.Settings;
using ReactiveUI;
using Splat;

namespace AHashPassGen
{
    public class App : Application
    {
        public static double FontSize  =>  GetFontSize();
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            
            Locator.CurrentMutable.Register(() => new EditRecordView(), typeof(IViewFor<EditRecordViewModel>));
            Locator.CurrentMutable.Register(() => new AboutView(), typeof(IViewFor<AboutViewModel>));
            Locator.CurrentMutable.RegisterConstant( new SettingsService< AppSettings >(), typeof( ISettingsService< AppSettings > ) );
            Locator.CurrentMutable.RegisterConstant( new DialogService(), typeof( IDialogService ) );
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
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