using EcommerceApp.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Common.Interfaces;

public interface IApplicationDbContext{
    DbSet<Product> Products { get; set; } // product is my entitiy class. Dbset<Product> query and add product to db using Efcore
    DbSet<Cart> Carts { get; set;}
    DbSet<User> Users { get; set;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); //this method will be used to save the changes to the database
}