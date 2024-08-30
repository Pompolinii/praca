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

       

        base.OnModelCreating(modelBuilder);

       

    }
}
