using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace firstDotnetProject;

public class VillaUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
    public string Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string ImageUrl { get; set; }
    public string Amenity { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}