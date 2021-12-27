using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Text;
using AHashPassGen.Models.Data;
using AHashPassGen.Models.Settings;
using AHashPassGen.Properties;
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
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global
        
        public ReadOnlyObservableCollection<Record> RecordList => _filteredRecordList;
        [Reactive] public String Filter { get; set; } = "";
        [Reactive] public Record? SelectedRecord { get; set; }
        
        private bool _forceClose;
        private readonly IDialogService _dialogService;
        private readonly ISettingsService< AppSettings > _settingsService;
        private readonly ObservableCollectionExtended<Record> _recordList = new ObservableCollectionExtended<Record>();
        private readonly ReadOnlyObservableCollection<Record> _filteredRecordList;
        
         public MainWindowViewModel( IDialogService? dialogService = null,
                                    ISettingsService< AppSettings >? settingsService = null )
        {
            _dialogService = dialogService ?? Locator.Current.GetService< IDialogService >() ?? throw new ArgumentNullException( nameof( dialogService ) );
            _settingsService = settingsService ?? Locator.Current.GetService< ISettingsService< AppSettings > >() ?? throw new ArgumentNullException( nameof( settingsService ) );

            _settingsService.Load();
            
        
            ExitCommand = ReactiveCommand.Create< CancelEventArgs >( ExitHandler );
            AboutCommand = ReactiveCommand.Create( AboutHandler );
            
            AddCommand = ReactiveCommand.Create( AddHandler );
            EditCommand = ReactiveCommand.Create< Record >( EditHandler );
            RemoveCommand = ReactiveCommand.Create< Record >( RemoveHandler );
            UpCommand = ReactiveCommand.Create< Record >( UpHandler );
            DownCommand = ReactiveCommand.Create< Record >( DownHandler );
            
            _recordList.ToObservableChangeSet()
                .AutoRefreshOnObservable( _ => this.WhenAnyValue( x => x.Filter ) )
                .Filter( x => true )
                .Bind( out _filteredRecordList )
                .Subscribe( /*_ => WordCount = $"({_wordListItems.Count})" */);
        }

         private void AddHandler()
         {
          //   throw new NotImplementedException();
         }
         
         private void EditHandler( Record? record )
         {
             throw new NotImplementedException();
         }
         
         private void RemoveHandler( Record? record )
         {
             throw new NotImplementedException();
         }
         
         private void UpHandler( Record? record )
         {
             throw new NotImplementedException();
         }
         
         private void DownHandler( Record? record )
         {
             throw new NotImplementedException();
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
