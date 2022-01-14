using AHashPassGen.Models.Data;

namespace AHashPassGen.Services;

public interface IStorageService
{
    bool Exist { get; }
    RecordsFile Load( string password );
    void Save( RecordsFile file, string password );
}