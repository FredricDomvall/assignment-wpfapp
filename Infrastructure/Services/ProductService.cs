using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services;
public class ProductService : IProductService
{
    private List<Product> _productList = new List<Product>();
    private readonly IJsonFileRepository<Product> _jsonFileRepository;
    private readonly IManufacturerService _manuFacturerService;
    private readonly ICategoryService _categoryService;
    private readonly string _filePath;

    public ProductService(
        IJsonFileRepository<Product> jsonFileRepository,
        IManufacturerService manuFacturerService, 
        ICategoryService categoryService, FileSources filePath)
    {
        _jsonFileRepository = jsonFileRepository;
        _manuFacturerService = manuFacturerService;
        _categoryService = categoryService;
        _filePath = filePath.ProductFileSource;
    }

    public async Task<AnswerOutcome<Product>> AddProductToListAsync(ProductForm productForm)
    {
        Product newProduct = new Product();
        newProduct.Category = new Category();
        newProduct.Manufacturer = new Manufacturer();
        newProduct.ProductId = GeneratorHelper.GenerateGuidId();
        newProduct.Category.CategoryName = productForm.CategoryName!; 
        newProduct.Category.CategoryPrefix = GeneratorHelper.GenerateCategoryPrefix(productForm.CategoryName!);
        newProduct.ProductCode = GeneratorHelper.GenerateArticleNumber(newProduct.Category.CategoryPrefix, _productList);

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
        var result = await LoadListFromFileAsync();
        if (result.Statement is false || !_productList.Any())
            return new AnswerOutcome<IEnumerable<Product>> { Statement = false, Answer = "No products available.", Outcome = _productList };

        return new AnswerOutcome<IEnumerable<Product>> { Statement = true, Answer = "Success.", Outcome = _productList };
    }

    public async Task<AnswerOutcome<IEnumerable<Product>>> LoadListFromFileAsync()
    {
        var productsFromFile = await _jsonFileRepository.ReadFromJsonFileAsync(_filePath);
        if (productsFromFile.Outcome == null || productsFromFile.Outcome.Count == 0)
            return new AnswerOutcome<IEnumerable<Product>> { Statement = false, Outcome = productsFromFile.Outcome};

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
