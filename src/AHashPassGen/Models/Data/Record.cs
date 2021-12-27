using AHashPassGen.Models.Enums;

namespace AHashPassGen.Models.Data;

public class Record
{
    public string User { get; set; } = "";
    public string Site { get; set; } = "";
    public string Comment { get; set; } = "";
    public AlphabetEnum Alphabet{ get; set; }
    public int Length { get; set; } = 16;
    public int StageCount { get; set; } = 100;


    public void Validate()
    {
        
    }
}