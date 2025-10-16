namespace Infrastructure.Configurations;
public class FileSources
{
    public FileSources()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));

        ProductFileSource = Path.Combine(projectRoot, "jsonfiles", "products.json");
        CategoryFileSource = Path.Combine(projectRoot, "jsonfiles", "categories.json");
        ManufacturerFileSource = Path.Combine(projectRoot, "jsonfiles", "manufacturers.json");
    }
    
    public string ProductFileSource { get; set; }
    public string CategoryFileSource { get; set; }
    public string ManufacturerFileSource { get; set; }
}
