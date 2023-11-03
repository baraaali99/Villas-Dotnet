using Microsoft.EntityFrameworkCore;

namespace firstDotnetProject.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Villa> Villas { get; set; }
    
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "on the sea",
                ImageUrl = "",
                Occupancy = 5,
                Rate = 200,
                Sqft = 550,
                Amenity = ""
            }
        );
    }
}