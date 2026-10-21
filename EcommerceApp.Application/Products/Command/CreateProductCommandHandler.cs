using EcommerceApp.Application.Common.Interfaces;
using EcommerceApp.Domain.Entites;
using MediatR;

namespace EcommerceApp.Application.Products.Command;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>{
    
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context){
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken){
        var product = new EcommerceApp.Domain.Entites.Product{
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