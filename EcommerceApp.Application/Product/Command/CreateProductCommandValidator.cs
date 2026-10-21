using FluentValidation;

namespace EcommerceApp.Application.Product.Command;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>{
    public CreateProductCommandValidator(){
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Product quantity cannot be negative.");
    }
}

// AbstractValidator
// This is a class provided by the FluentValidation library. It gives you methods to define validation rules, such as NotEmpty() and GreaterThan().


// CreateProductCommand
// This is the type of object you want to validate. It probably contains properties such as Name, Price, and StockQuantity.


// The < > symbols
// In C#, these indicate a generic type. Here, you're telling AbstractValidator exactly which type of object it should validate.