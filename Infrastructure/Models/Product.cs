namespace Infrastructure.Models;
public class Product
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}
