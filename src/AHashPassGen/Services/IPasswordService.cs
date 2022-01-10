using AHashPassGen.Models.Data;

namespace AHashPassGen.Services;

public interface IPasswordService
{
    string MasterPassword { get; set; }
    string Generate( Record record );
}