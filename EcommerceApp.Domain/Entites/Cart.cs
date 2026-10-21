namespace EcommerceApp.Domain.Entites;

public class Cart{
    public Guid Id { get; set;} = Guid.NewGuid();
    public Guid UserId { get; set;}
    public Guid ProductId { get; set;} 
    public int Quantity { get; set;} = 0;

    public Product? Product { get; set; }

}