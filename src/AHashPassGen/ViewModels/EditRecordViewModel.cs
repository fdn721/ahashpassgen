using System;
using System.Net.Mime;
using System.Reactive;
using AHashPassGen.Models.Data;
using AHashPassGen.Models.Enums;
using Common.Services.Dialog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AHashPassGen.ViewModels;

public class EditRecordViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    
    [Reactive] public bool EditMode { get; set; }
    [Reactive] public string Site { get; set; }
    [Reactive] public string Login { get; set; }
    [Reactive] public string Comment { get; set; }
    [Reactive] public AlphabetEnum Alphabet{ get; set; }
    [Reactive] public int Length { get; set; }
    [Reactive] public int StageCount { get; set; }

    public event Action<Record?>? CloseEvent;

    private Record _record { set; get; }
    private readonly IDialogService _dialogService;
    
    public EditRecordViewModel( Record? record, IDialogService? dialogService = null )
    {
        EditMode = record != null;
        _record = record == null ? new Record() : record.Clone();

        _dialogService = dialogService ?? Locator.Current.GetService<IDialogService>() ??
            throw new ArgumentNullException( nameof( dialogService ) );
        
        Site = _record.Site;
        Login = _record.Login;
        Comment = _record.Comment;
        Alphabet = _record.Alphabet;
        Length = _record.Length;
        StageCount = _record.StageCount;

        ApplyCommand = ReactiveCommand.Create( ApplyHandler,
            this.WhenAnyValue( x => x.Site, y => y.Login,
                ( text, translate ) => !string.IsNullOrEmpty( text ) && !string.IsNullOrEmpty( translate ) ) );

        CloseCommand = ReactiveCommand.Create( CloseHandler );
    }
    
    private void ApplyHandler()
    {
        _record.Site = Site;
        _record.Login = Login;
        _record.Comment = Comment;
        _record.Alphabet = Alphabet;
        _record.Length = Length;
        _record.StageCount = StageCount;
            
        CloseEvent?.Invoke( _record );
    }
        
    private void CloseHandler()
    {
        CloseEvent?.Invoke( null );
    }
}