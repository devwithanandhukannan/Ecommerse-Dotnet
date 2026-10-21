using MediatR;

namespace EcommerceApp.Application.Product.Command;

public class CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Quantity
) : IRequest<Guid> ;
