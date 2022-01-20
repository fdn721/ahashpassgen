using AHashPassGen.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Svg.Skia;
using ReactiveUI;
using SkiaSharp;
using Svg.Skia;

namespace AHashPassGen.Views
{
    public partial class MainWindow : ReactiveWindow< MainWindowViewModel >
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        
        public void CloseWindow()
        {
            Close( false );
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated(disposables => {
                if( ViewModel != null )
                {
                    ViewModel.CloseEvent += Close;
                    ViewModel.Init();
                }

            });
        }
    }
}