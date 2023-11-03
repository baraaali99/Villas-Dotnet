using System.ComponentModel.DataAnnotations;

namespace firstDotnetProject;

public class VillaNumberUpdateDto
{
    [Required]
    public int VillaNo { get; set; }
    
    public string SpecialDetails { get; set; }
}