using AHashPassGen.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

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
            this.WhenActivated(disposables => {   
                if( ViewModel != null ) 
                    ViewModel.CloseEvent += Close;
            });
            
            AvaloniaXamlLoader.Load(this);
        }
    }
}