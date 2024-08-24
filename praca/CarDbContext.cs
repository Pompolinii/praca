using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using praca.Entities;

public class CarDbContext : IdentityDbContext<ApplicationUser>
{
    public CarDbContext(DbContextOptions<CarDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

       // modelBuilder.Entity<Car>()
                //.Ignore(c => c.ImagePath);
                // Ignorowanie właściwości IFormFile

        base.OnModelCreating(modelBuilder);

        // Configure ASP.NET Identity tables

    }
}
