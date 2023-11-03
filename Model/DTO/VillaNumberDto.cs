using System.ComponentModel.DataAnnotations;

namespace firstDotnetProject;

public class VillaNumberDto
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaID { get; set; }
    public string SpecialDetails { get; set; }
}