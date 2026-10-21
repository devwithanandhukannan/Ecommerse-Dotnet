using EcommerceApp.Application.Common.Interfaces;
using EcommerceApp.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext{ //DbContext is a class from efcore which will be used to interact with the database. IApplicationDbContext is an interface which will be used to interact with the database.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ //this constructor will be used to pass the options to the base class. The options will be used to configure the database connection.
    }

    public DbSet<Product> Products { get; set;} = null!; //Dbset<Product> 
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Cart> Carts { get; set;} = null!;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default){
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }

}