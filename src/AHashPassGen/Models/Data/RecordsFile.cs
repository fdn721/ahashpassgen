using System.Collections.Generic;

namespace AHashPassGen.Models.Data;

public class RecordsFile
{
    public int Version { get; set; } = 1;
    public List< Record > Records { get; set; } = new();
    
    
}