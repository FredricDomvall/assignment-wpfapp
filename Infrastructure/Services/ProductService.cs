using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services;
public class ProductService : IProductService
{
    private List<Product> _productList = new List<Product>();

    private readonly IJsonFileRepository<Product> _jsonFileRepository;
    public ProductService(IJsonFileRepository<Product> jsonFileRepository)
    {
        _jsonFileRepository = jsonFileRepository;
    }
    public async Task<AnswerOutcome<bool>> AddProductToListAsync(ProductForm productForm)
    {
        if (productForm == null || string.IsNullOrEmpty(productForm.ProductName) || string.IsNullOrEmpty(productForm.ProductPrice))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Invalid input." };
        if (!decimal.TryParse(productForm.ProductPrice, out decimal price))
            return new AnswerOutcome<bool> { Statement = false, Answer = "price not of type decimal." };
        
        Product newProduct = new Product
        {
            ProductId = Guid.NewGuid(),
            ProductName = productForm.ProductName,
            ProductPrice = price
        };
        await LoadListFromFileAsync();
        _productList.Add(newProduct);
        await SaveListToFileAsync();
        return new AnswerOutcome<bool> { Statement = true, Answer = "Success.", Outcome = newProduct };
    }

    public async Task<IEnumerable<Product>> GetAllProductsFromListAsync()
    {
        await LoadListFromFileAsync();
        return _productList;
    }

    public async Task<IEnumerable<Product>> LoadListFromFileAsync()
    {
        var productsFromFile = await _jsonFileRepository.ReadFromJsonFileAsync();
        if (productsFromFile == null || !productsFromFile.Any())
            return Enumerable.Empty<Product>();
        
        _productList = productsFromFile;
        return _productList;
    }

    public async Task<bool> SaveListToFileAsync()
    {
        if (!_productList.Any())
            return false;

        await _jsonFileRepository.WriteToJsonFileAsync(_productList);
        return true;
    }
}
