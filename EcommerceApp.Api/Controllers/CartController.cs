using System.Security.Claims;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Aspcore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Api.Controllers;

public record AddToCartRequest(Guid ProductId, int Quantity);

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase{
    private readonly IApplicationDbContext _context;

    public CartController(IApplicationDbContext dbcontext){
        _context = dbcontext;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
            return Unauthorized();
        
        var product = await _context.Products.FindAsync(request.ProductId);
        if(product == null){
            return NotFound("Product not found.");
        }

        var existingCartItem = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == request.ProductId);
        if(existingCartItem != null){
            existingCartItem.Quantity += request.Quantity;
        } else {
            _context.Carts.Add(new Cart{
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Product added to cart successfully." });    
    }
}