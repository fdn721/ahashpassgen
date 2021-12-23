using System.Threading.Tasks;
using Avalonia.MessageBox;


namespace Common.Services.Dialog
{
    public interface IDialogService
    {
        Task< MessageBoxResult > Question( string title, string message );
        Task< MessageBoxResult > Information( string title, string message );
        Task< MessageBoxResult > Error( string title, string message, string details = "" );
        Task< string > OpenFile( string filters, string title = "", string defaultDir = "" );
        Task< string > SaveFile( string filters, string title = "", string defaultName = "", string defaultDir = "" );

        Task<TR> Show<TVm, TR>( TVm viewModel ) where TVm : class;
        Task Show<TVm>( TVm viewModel ) where TVm : class;
    }
}