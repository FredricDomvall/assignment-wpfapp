namespace Infrastructure.Models;
public class Product
{
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    public Category Category { get; set; } = null!;
    public Manufacturer Manufacturer { get; set; } = null!;
}
