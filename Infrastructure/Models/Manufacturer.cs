namespace Infrastructure.Models;
public class Manufacturer
{
    public Guid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; } = null!;
    public string ManufacturerCountry { get; set; } = null!;
    public string ManufacturerEmail { get; set; } = null!;
}
