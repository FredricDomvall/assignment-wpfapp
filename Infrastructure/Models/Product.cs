namespace Infrastructure.Models;
public class Product
{
    public Guid ProductId { get; set; }
    public string? ProductCode { get; set; } // has to be nullable until i can find a solution
    public string ProductName { get; set; } = null!;
    public string? ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public Category Category { get; set; } = null!;
    public Manufacturer Manufacturer { get; set; } = null!;
}
