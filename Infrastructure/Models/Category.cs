namespace Infrastructure.Models;
public class Category
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string CategoryPrefix { get; set; } = null!;
}
