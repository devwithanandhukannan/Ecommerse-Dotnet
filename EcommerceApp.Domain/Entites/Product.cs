namespace EcommerceApp.Domain.Entites;

public class Product{
    public Guid id { get; set;} = Guid.NewGuid();
    public string Name { get; set;} = string.Empty;
    public string Description { get; set;} = string.Empty;
    public decimal Price { get; set;} = 0;
    public int Quantity { get; set;} = 0;
    public DateTime CreatedAt { get; set;} = DateTime.UtcNow;
}