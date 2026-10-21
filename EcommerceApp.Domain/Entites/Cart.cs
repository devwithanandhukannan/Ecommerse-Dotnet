namespace EcommerceApp.Domain.Entites;

public class Cart{
    public Guid id { get; set;} = Guid.NewGuid();
    public Guid userId { get; set;}
    public Guid ProductId { get; set;} 
    public int Quantity { get; set;} = 0;

    public Product? Product { get; set; }

}