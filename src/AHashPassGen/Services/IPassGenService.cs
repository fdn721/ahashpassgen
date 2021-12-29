using AHashPassGen.Models.Data;

namespace AHashPassGen.Services;

public interface IPassGenService
{
    string MasterPassword { get; set; }
    string Generate( Record record );
}