using System;
using AHashPassGen.Models.Enums;

namespace AHashPassGen.Models.Data;

public class Record
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Site { get; set; } = "";
    public string Login { get; set; } = "";
    public string Comment { get; set; } = "";
    public AlphabetEnum Alphabet { get; set; } = AlphabetEnum.A64;
    public int Length { get; set; } = 16;
    public int StageCount { get; set; } = 100;
    
    public void Validate()
    {
    }
    
    public Record Clone()
    {
        var record = new Record();

        record.Id = Id;
        record.Site = Site;
        record.Login = Login;
        record.Comment = Comment;
        record.Alphabet = Alphabet;
        record.Length = Length;
        record.StageCount = StageCount;
            
        return record;
    }
}