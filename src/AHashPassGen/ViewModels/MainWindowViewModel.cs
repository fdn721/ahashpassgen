using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Text;
using AHashPassGen.Models.Data;
using AHashPassGen.Models.Settings;
using AHashPassGen.Properties;
using AHashPassGen.Services;
using Avalonia.MessageBox;
using Common.Services.Dialog;
using Common.Services.Settings;
using DynamicData;
using DynamicData.Binding;
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
        public ReactiveCommand< CancelEventArgs, Unit > ExitCommand { get; }
        public ReactiveCommand< Unit, Unit > AboutCommand { get; }
        public ReactiveCommand< Unit, Unit > AddCommand { get; }
        public ReactiveCommand< Record, Unit > EditCommand { get; }
        public ReactiveCommand< Record, Unit > RemoveCommand { get; }
        public ReactiveCommand< Record, Unit > UpCommand { get; }
        public ReactiveCommand< Record, Unit > DownCommand { get; }
        public ReactiveCommand< Record, Unit > GenerateCommand { get; }
        
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global
        
        public ReadOnlyObservableCollection<Record> RecordList => _filteredRecordList;
        [Reactive] public String Filter { get; set; } = "";
        [Reactive] public Record? SelectedRecord { get; set; }
        
        private bool _forceClose;
        private readonly IPasswordService _passwordService;
        private readonly IDialogService _dialogService;
        private readonly ISettingsService< AppSettings > _settingsService;
        private readonly IStorageService _storageService;
        private readonly ObservableCollectionExtended<Record> _recordList = new ObservableCollectionExtended<Record>();
        private readonly ReadOnlyObservableCollection<Record> _filteredRecordList;
        private RecordsFile _recordsFile;
        
         public MainWindowViewModel( IDialogService? dialogService = null, 
                                     IPasswordService? passwordService = null,
                                     IStorageService? storageService = null,
                                     ISettingsService< AppSettings >? settingsService = null )
        {
            _dialogService = dialogService ?? Locator.Current.GetService< IDialogService >() ?? throw new ArgumentNullException( nameof( dialogService ) );
            _passwordService = passwordService ?? Locator.Current.GetService< IPasswordService >() ?? throw new ArgumentNullException( nameof( passwordService ) );
            _storageService = storageService ?? Locator.Current.GetService< IStorageService >() ?? throw new ArgumentNullException( nameof( storageService ) );
            _settingsService = settingsService ?? Locator.Current.GetService< ISettingsService< AppSettings > >() ?? throw new ArgumentNullException( nameof( settingsService ) );

            _settingsService.Load();
            
            ExitCommand = ReactiveCommand.Create< CancelEventArgs >( ExitHandler );
            AboutCommand = ReactiveCommand.Create( AboutHandler );
            
            AddCommand = ReactiveCommand.Create( AddHandler );
            EditCommand = ReactiveCommand.Create< Record >( EditHandler, this.WhenAny( x => x.SelectedRecord, _ => SelectedRecord != null ) );
            RemoveCommand = ReactiveCommand.Create< Record >( RemoveHandler, this.WhenAny( x => x.SelectedRecord, _ => SelectedRecord != null ) );
            UpCommand = ReactiveCommand.Create< Record >( UpHandler, this.WhenAny( x => x.SelectedRecord, _ => SelectedRecord != null ) );
            DownCommand = ReactiveCommand.Create< Record >( DownHandler, this.WhenAny( x => x.SelectedRecord, _ => SelectedRecord != null ) );
            GenerateCommand = ReactiveCommand.Create< Record >( GenerateHandler, this.WhenAny( x => x.SelectedRecord, _ => SelectedRecord != null ) );
            
            _recordList.ToObservableChangeSet()
                //.AutoRefreshOnObservable( _ => this.WhenAnyValue( x => x.Filter ) 
                //.Filter( x => true )
                .Bind( out _filteredRecordList )
                .Subscribe( /*_ => WordCount = $"({_wordListItems.Count})" */);
        }

         public async void Init()
         {
             try
             {
                 _recordsFile = _storageService.Load();
             }
             catch( Exception err )
             {
                 await _dialogService.Error( I18n.Error, $"{I18n.LoadError}!", err.ToString() );
             }
             
             var vm = new MasterPasswordViewModel( _recordsFile.MasterPasswordHash );
             var password = await _dialogService.Show< MasterPasswordViewModel, string >( vm );

             if( string.IsNullOrEmpty( password ) )
             {
                 _forceClose = true;
                 CloseEvent?.Invoke();
                 return;
             }

             if( string.IsNullOrEmpty( _recordsFile.MasterPasswordHash ) )
             {
                 try
                 {
                     _recordsFile.MasterPasswordHash = _passwordService.CalcHash( password );
                     _storageService.Save( _recordsFile );
                 }
                 catch( Exception err )
                 {
                     await _dialogService.Error( I18n.Error, $"{I18n.SaveError}!", err.ToString() );
                 }
             }

             _passwordService.MasterPassword = password;
             _recordList.AddRange( _recordsFile.Records );
         }
         
         private async void AddHandler()
         {
             var vm = new EditRecordViewModel( null );
             var record = await _dialogService.Show<EditRecordViewModel, Record >( vm );

             if( record == null )
                 return;
             
             _recordList.Add( record );
         }
        
        
         private async void EditHandler( Record? record )
         {
             if( record == null )
                 return;
             
             var vm = new EditRecordViewModel( record );
             var newRecord = await _dialogService.Show<EditRecordViewModel, Record >( vm );

             if( newRecord == null )
                 return;
             
             _recordList.Replace( record, newRecord );
         }
         
         private async void RemoveHandler( Record? record )
         {
             if( record == null )
                 return;
            
             var result = await _dialogService.Question( I18n.Remove, $"{I18n.RemoveRecord} {record.Site}/{record.Login}?" );

             if( result != MessageBoxResult.Yes )
                 return;
            
             _recordList.Remove( record );
         }
         
         private void UpHandler( Record? record )
         {
             if( record == null )
                 return;
             
             var index = _recordList.IndexOf( record );

             if( index <= 0 )
                 return;
                
             _recordList.Move( index, index - 1 );
         }
         
         private void DownHandler( Record? record )
         {
             if( record == null )
                 return;
             
             var index = _recordList.IndexOf( record );

             if( index >=  ( _recordList.Count - 1 ) )
                 return;
                
             _recordList.Move( index, index + 1 );
         }
         
         private void GenerateHandler( Record? record )
         {
             if( record == null )
                 return;
             
             var password = _passwordService.Generate( record );

             var vm = new PasswordViewModel( record.Site, record.Login, password );

             _dialogService.Show( vm );

         }

         private async void ExitHandler( CancelEventArgs arg)
         {
             if( _forceClose )
                 return;
            
             arg.Cancel = true;
            
            
             var result = await _dialogService.Question( I18n.Closing, I18n.CloseApplication );

             if( result != MessageBoxResult.Yes )
                 return;
             
             SaveRecords();
             
             _forceClose = true;
             CloseEvent?.Invoke();
         }
        
         private async void AboutHandler()
         {
             await _dialogService.Show( new AboutViewModel() );
         }
         
         private void LoadRecords()
         {
            
         }
         
         private void SaveRecords()
         {
             try
             {

                 _recordsFile.Records.Clear();
                 _recordsFile.Records.AddRange( _recordList );
                 _storageService.Save( _recordsFile );
             }
             catch( Exception err )
             {
                 _dialogService.Error( I18n.Error, $"{I18n.SaveError}!", err.ToString() );
             }
         }
    }
}
