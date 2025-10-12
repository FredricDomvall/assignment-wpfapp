using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services;
public class ProductService : IProductService
{
    private List<Product> _productList = new List<Product>();

    private readonly IJsonFileRepository _jsonFileRepository;
    public ProductService(IJsonFileRepository jsonFileRepository)
    {
        _jsonFileRepository = jsonFileRepository;
        LoadListFromFile();
    }
    public bool AddProductToList(ProductForm productForm)
    {
        if (productForm == null || string.IsNullOrEmpty(productForm.ProductName) || string.IsNullOrEmpty(productForm.ProductPrice))
            return false;
        if (!decimal.TryParse(productForm.ProductPrice, out decimal price))
            return false;
        
        Product newProduct = new Product
        {
            ProductId = Guid.NewGuid(),
            ProductName = productForm.ProductName,
            ProductPrice = price
        };

        _productList.Add(newProduct);
        return SaveListToFile();
    }

    public IEnumerable<Product> GetAllProductsFromList()
    {
        LoadListFromFile();
        return _productList;
    }

    public IEnumerable<Product> LoadListFromFile()
    {
        var productsFromFile = _jsonFileRepository.ReadFromJsonFile();
        if (productsFromFile == null || !productsFromFile.Any())
            return Enumerable.Empty<Product>();
        
        _productList = productsFromFile;
        return _productList;
    }

    public bool SaveListToFile()
    {
        if (!_productList.Any())
            return false;

        return _jsonFileRepository.WriteToJsonFile(_productList); 
    }
}
