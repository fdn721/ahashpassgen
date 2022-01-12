using AHashPassGen.Models.Data;

namespace AHashPassGen.Services;

public interface IStorageService
{
    RecordsFile Load();
    void Save( RecordsFile file );
}