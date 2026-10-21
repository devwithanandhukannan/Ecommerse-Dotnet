using EcommerseApp.Application.Common.Interfaces;
using EcommerseApp.Domain.Entites;
using MediatR;

namespace EcommerceApp.Application.Product.Command;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>{
    
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context){
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken){
        var product = new Product{
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity
        };
        _context.Products.Add(product); //only just mark is need to add to the database, it will not be added until we call SaveChangesAsync
        await _context.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}