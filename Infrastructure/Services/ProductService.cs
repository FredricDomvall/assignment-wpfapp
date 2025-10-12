using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services;
public class ProductService : IProductService
{
    private readonly IJsonFileRepository _jsonFileRepository;
    public ProductService(IJsonFileRepository jsonFileRepository)
    {
        _jsonFileRepository = jsonFileRepository;
    }
    public bool AddProductToList(ProductForm productForm)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetAllProductsFromList()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> LoadListFromFile()
    {
        throw new NotImplementedException();
    }

    public bool SaveListToFile()
    {
        throw new NotImplementedException();
    }
}
