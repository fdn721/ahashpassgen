using System;
using AHashPassGen.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AHashPassGen.Views
{
    public class AboutView : ReactiveWindow< AboutViewModel >
    {
        private bool _once = true;
        public AboutView()
        {
            InitializeComponent();
            
            Activated += ( sender, args ) =>
            {
                // TODO workaround to fix startup position
                if( _once && OperatingSystem.IsLinux() )
                {
                    _once = false;
                    var mainWindows = ( ( IClassicDesktopStyleApplicationLifetime ) Application.Current.ApplicationLifetime ).MainWindow; 
                    SetWindowStartupLocation( mainWindows.PlatformImpl );
                }
            };
        }
        private void InitializeComponent()
        {
            this.WhenActivated( disposables =>
            {
                if( ViewModel != null )
                    ViewModel.CloseEvent += Close;
            });
            
            AvaloniaXamlLoader.Load(this);
        }
        
        private void SetWindowStartupLocation( IWindowBaseImpl? owner = null )
        {
            double num;
            if (owner == null)
            {
                IWindowImpl platformImpl = this.PlatformImpl;
                num = platformImpl?.DesktopScaling ?? 1.0;
            }
            else
                num = owner.DesktopScaling;
            
            double scale = num;
            PixelRect rect = new PixelRect( PixelPoint.Origin, PixelSize.FromSize( ClientSize, scale ) );
            if( WindowStartupLocation == WindowStartupLocation.CenterScreen )
            {
                Screen screen = Screens.ScreenFromPoint( owner?.Position ?? Position );
                if( screen == null )
                    return;
                Position = screen.WorkingArea.CenterRect(rect).Position;
            }
            else
            {
                if( WindowStartupLocation != WindowStartupLocation.CenterOwner || owner == null )
                    return;
                Position = new PixelRect( owner.Position, PixelSize.FromSize( owner.ClientSize, scale ) ).CenterRect( rect ).Position;
            }
        }
    }
}