using EcommerceApp.Application.Product.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator){
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command){
        var productId = await _mediator.Send(command);
        return Ok(new { product_id = productId, message = "Product created successfully" });
    }
}