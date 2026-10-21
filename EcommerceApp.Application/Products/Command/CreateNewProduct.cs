using MediatR;

namespace EcommerceApp.Application.Products.Command;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Quantity
) : IRequest<Guid>;
