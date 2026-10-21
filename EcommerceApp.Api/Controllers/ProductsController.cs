using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase{
    private readonly ISender _mediator;

    public AuthController(ISender mediator){
        _mediator = mediator;
    }

    [HttpPost("login")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Login([FromBody] CreateProductCommand command){
        var productId = await _mediator.Send(command);
        return Ok(new { product_id = productId, message = "Login successful" });
    }
}