using AHashPassGen.Models.Data;

namespace AHashPassGen.Services;

public interface IPasswordService
{
    string MasterPassword { get; set; }
    string CalcHash( string value );
    string Generate( Record record );
}