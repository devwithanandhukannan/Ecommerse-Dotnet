using MediatR;

namespace EcommerceApp.Application.Product.Command;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Quantity
) : IRequest<Guid>;
