namespace EcommerceApp.Domain.Entites;

public enum UserRole{
    Admin = 1,
    Customer = 0
}

public class User{
    public Guid id { get; set;} = Guid.NewGuid();
    public string Email { get; set;} = string.Empty;
    public string Password { get; set;} = string.Empty;
    public UserRole Role { get; set;} = UserRole.Customer;
    public dateTime CreatedAt { get; set;} = DateTime.UtcNow;
}