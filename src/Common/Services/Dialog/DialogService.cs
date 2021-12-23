using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.MessageBox;
using Avalonia.Platform;
using Common.Properies;
using JetBrains.Annotations;
using ReactiveUI;
using Splat;


namespace Common.Services.Dialog
{
    public class DialogService : IDialogService
    {
        private string _lastDir = "";
        
        public Task< MessageBoxResult > Question( string title, string message )
        {
            var mb = new MessageBoxView();

            var lifeTime = ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime;
            
            return mb.Show( lifeTime?.MainWindow, message, title, "", MessageBoxButtons.YesNo, MessageBoxIcons.Question );
        }
        
        public Task< MessageBoxResult > Information( string title, string message )
        {
            var mb = new MessageBoxView();

            var lifeTime = ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime;
            
            return mb.Show( lifeTime?.MainWindow, message, title, "", MessageBoxButtons.Ok, MessageBoxIcons.Info );
        }

        public Task< MessageBoxResult > Error( string title, string message, string details = "" )
        {
            var mb = new MessageBoxView();

            var lifeTime = ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime;
            
            return mb.Show( lifeTime?.MainWindow, message, title, details, MessageBoxButtons.Ok, MessageBoxIcons.Error );
        }

        [ItemCanBeNull]
        public async Task<string> OpenFile( string filters, string title, string defaultDir)
        {
            var filterList = BuildFilters( filters );


            var fileDialog = new OpenFileDialog();
            fileDialog.AllowMultiple = false;
            fileDialog.Filters = filterList;
            fileDialog.Title =  string.IsNullOrEmpty( title ) ? I18n.Open : title;
            fileDialog.Directory = string.IsNullOrEmpty( defaultDir ) ? _lastDir : defaultDir;
            
            var lifeTime = ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime;

            var files = await fileDialog.ShowAsync( lifeTime.MainWindow );

            if( files == null || files.Length == 0 )
                return null;

            _lastDir = Path.GetDirectoryName( files[0] ) + Path.DirectorySeparatorChar;
            
            return files[0];
        }

      

        [ItemCanBeNull]
        public async Task< string > SaveFile( string filters, string title = "", string defaultName = "", string defaultDir = "" )
        {
            var filterList = BuildFilters( filters );
            
            var fileDialog = new SaveFileDialog();
            fileDialog.Filters = filterList;
            fileDialog.Title =  string.IsNullOrEmpty( title ) ? I18n.Save : title;
            fileDialog.InitialFileName = defaultName;
            fileDialog.Directory = string.IsNullOrEmpty( defaultDir ) ? _lastDir : defaultDir;
            
            var lifeTime = ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime;

            var file = await fileDialog.ShowAsync( lifeTime.MainWindow );

            return file;
        }
        
        public Task< TR > Show< TVm, TR>( TVm viewModel ) where TVm : class
        {
            var viewLocator = (IViewLocator) Locator.Current.GetService( typeof( IViewLocator ) );
            var view = ( IViewFor< TVm > )viewLocator.ResolveView( viewModel );

            if( view == null )
                throw new InvalidOperationException( $"No View for ViewModel {typeof( IViewLocator )}" );

            view.ViewModel = viewModel;

            var mainWindows = ( ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime ).MainWindow; 
            var window = ( Window )view;

            return window.ShowDialog<TR>( mainWindows );

        }
        
        public Task Show< TVm >( TVm viewModel ) where TVm : class
        {
            var viewLocator = (IViewLocator) Locator.Current.GetService( typeof( IViewLocator ) );
            var view = ( IViewFor< TVm > )viewLocator.ResolveView( viewModel );

            if( view == null )
                throw new InvalidOperationException( $"No View for ViewModel {typeof( IViewLocator )}" );

            view.ViewModel = viewModel;

            var mainWindows = ( ( IClassicDesktopStyleApplicationLifetime ) Avalonia.Application.Current.ApplicationLifetime ).MainWindow; 
            var window = ( Window )view;

            return window.ShowDialog( mainWindows );

        }
        
        private static List<FileDialogFilter> BuildFilters( string filters )
        {
            var filterList = new List<FileDialogFilter>();

            var inputList = filters.Split( '|' );
            for( int i = 0; i < inputList.Length; i += 2 )
            {
                var fileFilter = new FileDialogFilter();
                fileFilter.Name = inputList[i];

                if( ( i + 1 ) < inputList.Length )
                {
                    var inputExtension = inputList[i + 1];
                    var extensionList = inputExtension.Split( ';' );

                    foreach( var extension in extensionList )
                        fileFilter.Extensions.Add( extension );
                }

                filterList.Add( fileFilter );
            }

            return filterList;
        }
        

    }
}