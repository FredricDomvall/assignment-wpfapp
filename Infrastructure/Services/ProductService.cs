using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;
public class ProductService : IProductService
{
    private List<Product> _productList = new List<Product>();
    private readonly IJsonFileRepository<Product> _jsonFileRepository;
    private readonly string _filePath;
    public ProductService(IJsonFileRepository<Product> jsonFileRepository, FileSources filePath)
    {
        _jsonFileRepository = jsonFileRepository;
        _filePath = filePath.ProductFileSource;

    }
    public async Task<AnswerOutcome<Product>> AddProductToListAsync(ProductForm productForm)
    {
        Product newProduct = new Product();
        newProduct.ProductId = GeneratorHelper.GenerateGuidId();

        var nameValidationResult = ValidationHelper.ValidateString(productForm.ProductName!);
        var priceValidationResult = ValidationHelper.ValidateDecimalPrice(productForm.ProductPrice!);
        var guidValidationResult = ValidationHelper.ValidateGuidId<Product>(newProduct.ProductId);
        var uniqueValidationResult = ValidationHelper.ValidateProductUnique(newProduct, _productList);

        if (nameValidationResult.Statement is true && priceValidationResult.Statement is true 
         && guidValidationResult.Statement is true && uniqueValidationResult.Statement is true)
        {
            newProduct.ProductName = productForm.ProductName!;
            newProduct.ProductPrice = decimal.Parse(productForm.ProductPrice!);

            await LoadListFromFileAsync();
            _productList.Add(newProduct);
            await SaveListToFileAsync();
            return new AnswerOutcome<Product> { Statement = true, Answer = "Success.", Outcome = newProduct };
        }
        else
        {
            string errorMessages = "";
            if (nameValidationResult.Statement is false)
                errorMessages += nameValidationResult.Answer + "\n";
            if (priceValidationResult.Statement is false)
                errorMessages += priceValidationResult.Answer + "\n";
            if (guidValidationResult.Statement is false)
                errorMessages += guidValidationResult.Answer + "\n";
            if (uniqueValidationResult.Statement is false)
                errorMessages += uniqueValidationResult.Answer + "\n";
            return new AnswerOutcome<Product> { Statement = false, Answer = errorMessages.Trim() };
        }
    }

    public async Task<AnswerOutcome<IEnumerable<Product>>> GetAllProductsFromListAsync()
    {
        await LoadListFromFileAsync();
        return new AnswerOutcome<IEnumerable<Product>> { Statement = true, Answer = "Success.", Outcome = _productList };
    }

    public async Task<AnswerOutcome<IEnumerable<Product>>> LoadListFromFileAsync()
    {
        var productsFromFile = await _jsonFileRepository.ReadFromJsonFileAsync(_filePath);
        if (productsFromFile.Outcome == null || productsFromFile.Outcome.Count == 0)
            return new AnswerOutcome<IEnumerable<Product>> { Statement = false };

        _productList = productsFromFile.Outcome;
        return new AnswerOutcome<IEnumerable<Product>> { Statement = true };
    }

    public async Task<AnswerOutcome<bool>> SaveListToFileAsync()
    {
        if (!_productList.Any())
            return new AnswerOutcome<bool> { Statement = false };

        await _jsonFileRepository.WriteToJsonFileAsync(_filePath, _productList);
        return new AnswerOutcome<bool> { Statement = true };
    }
}
