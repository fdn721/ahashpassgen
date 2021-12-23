using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.MessageBox.Properties;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using Svg.Skia;

namespace Avalonia.MessageBox
{
    public class MessageBoxView : Window
    {
        private readonly StackPanel _buttons;
        private readonly StackPanel _infos;
        private readonly TextBlock _text;
        private readonly Image _icon;
        private readonly TextBox _details;
        private Window? _parent;
        private bool _once = true;

        
        public MessageBoxView()
        {
            InitializeComponent();
            
            _buttons = this.FindControl< StackPanel >( "Buttons" );
            _infos = this.FindControl< StackPanel >( "Infos" );
            _text = this.FindControl< TextBlock >( "Text" );
            _icon = this.FindControl< Image >( "Icon" );
            _details = this.FindControl< TextBox >( "Details" );

            Activated += ( sender, args ) =>
            {
                SizeToContent = SizeToContent.Height;

                // TODO workaround to fix startup position
                if( _once && OperatingSystem.IsLinux() )
                {
                    _once = false;
                    SetWindowStartupLocation( _parent?.PlatformImpl );
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public Task< MessageBoxResult > Show( Window parent, string text, string title, string details, MessageBoxButtons buttons, MessageBoxIcons icons = MessageBoxIcons.None )
        {
            _parent = parent;
            
            Title = title;
            _text.Text = text;
            _details.Text = details;
            AddIcons( icons );
            
            if( !string.IsNullOrEmpty( details ) )
                AddDetailsButton();
            
            AddButtons( buttons );

            return ShowDialog< MessageBoxResult >(parent);
        }

        private void AddIcons( MessageBoxIcons icons )
        {
            if( icons == MessageBoxIcons.Error )
            {
                _icon.Source = new SvgImage
                {
                    Source = SvgSource.Load< SvgSource >( "avares://Avalonia.MessageBox/Assets/error.svg", null )
                };
            }
            else if( icons == MessageBoxIcons.Warning )
            {
                _icon.Source = new SvgImage
                {
                    Source = SvgSource.Load< SvgSource> ( "avares://Avalonia.MessageBox/Assets/warning.svg", null )
                };
            }
            else if( icons == MessageBoxIcons.Info )
            {
                _icon.Source = new SvgImage
                {
                    Source = SvgSource.Load< SvgSource>( "avares://Avalonia.MessageBox/Assets/info.svg", null )
                };
            }  
            else if( icons == MessageBoxIcons.Question )
            {
                _icon.Source = new SvgImage
                {
                    Source = SvgSource.Load< SvgSource> ( "avares://Avalonia.MessageBox/Assets/question.svg", null )
                };
            }
            else
            {
              //  _infos.Children.Remove( _icon );
            }

        }
        private void AddButtons( MessageBoxButtons buttons )
        {
            if( buttons == MessageBoxButtons.Ok )
            {
                AddButton( I18n.OK, MessageBoxResult.Ok, true );
            }
            else if( buttons == MessageBoxButtons.OkCancel )
            {
                AddButton( I18n.OK, MessageBoxResult.Ok, true );
                AddButton( I18n.Cancel, MessageBoxResult.Cancel, false );
            }
            else if( buttons == MessageBoxButtons.YesNo )
            {
                AddButton( I18n.Yes, MessageBoxResult.Yes, true );
                AddButton( I18n.No, MessageBoxResult.No, false );
            }
            else
            {
                throw new NotImplementedException( $"Unknown MessageBoxButtons {buttons}" );
            }
        }
        
        private void AddButton( string text, MessageBoxResult result, bool defButton = false )
        {
            var btn = new Button {Content = text};
            
            btn.Click += ( _, __ ) =>
            {
                Close( result );
            };
            
            _buttons.Children.Add( btn );
        }

        private void AddDetailsButton()
        {
            var btn = new Button {Content = I18n.ShowDetails };
            
            btn.Click += ( _, __ ) =>
            {
                if( _details.IsVisible )
                {
                    _details.IsVisible = false;
                    btn.Content = I18n.ShowDetails;
                }
                else
                {
                    _details.IsVisible = true;
                    btn.Content = I18n.HideDetails;
                }
            };
            
            _buttons.Children.Add( btn );
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
                Screen screen = this.Screens.ScreenFromPoint(owner?.Position ?? Position);
                if (screen == null)
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
