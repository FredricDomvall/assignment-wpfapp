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
            newProduct.Category.CategoryName = productForm.CategoryName!;
            newProduct.Manufacturer.ManufacturerName = productForm.ManufacturerName!;
            newProduct.Manufacturer.ManufacturerCountry = productForm.ManufacturerCountry!;
            newProduct.Manufacturer.ManufacturerEmail = productForm.ManufacturerEmail!;

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

    public async Task<AnswerOutcome<Product>> UpdateProductInListByIdAsync(Guid productId, ProductForm productForm)
    {
        await LoadListFromFileAsync();
        var productToUpdate = _productList.FirstOrDefault(p => p.ProductId == productId);
        if (productToUpdate == null)
            return new AnswerOutcome<Product> { Statement = false, Answer = "Product with the specified ID does not exist." };

        var nameValidationResult = ValidationHelper.ValidateString(productForm.ProductName!);
        var priceValidationResult = ValidationHelper.ValidateDecimalPrice(productForm.ProductPrice!);
        var uniqueValidationResult = ValidationHelper.ValidateProductUnique(productToUpdate, _productList);

        if (nameValidationResult.Statement is true && priceValidationResult.Statement is true
         && uniqueValidationResult.Statement is true)
        {
            productToUpdate.ProductName = productForm.ProductName!;
            productToUpdate.ProductPrice = decimal.Parse(productForm.ProductPrice!);
            productToUpdate.Category.CategoryName = productForm.CategoryName!;
            productToUpdate.Manufacturer.ManufacturerName = productForm.ManufacturerName!;
            productToUpdate.Manufacturer.ManufacturerCountry = productForm.ManufacturerCountry!;
            productToUpdate.Manufacturer.ManufacturerEmail = productForm.ManufacturerEmail!;

            await LoadListFromFileAsync();
            _productList.Add(productToUpdate);
            await SaveListToFileAsync();
            return new AnswerOutcome<Product> { Statement = true, Answer = "Success.", Outcome = productToUpdate };
        }
        else
        {
            string errorMessages = "";
            if (nameValidationResult.Statement is false)
                errorMessages += nameValidationResult.Answer + "\n";
            if (priceValidationResult.Statement is false)
                errorMessages += priceValidationResult.Answer + "\n";
            if (uniqueValidationResult.Statement is false)
                errorMessages += uniqueValidationResult.Answer + "\n";
            return new AnswerOutcome<Product> { Statement = false, Answer = errorMessages.Trim() };
        }


    }

    public async Task<AnswerOutcome<bool>> DeleteProductFromListByIdAsync(Guid productId)
    {
        await LoadListFromFileAsync();

        if (!_productList.Any(p => p.ProductId == productId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product with the specified ID does not exist." };
       
        _productList.RemoveAll(p => p.ProductId == productId);
        await SaveListToFileAsync();
        return new AnswerOutcome<bool> { Statement = true, Answer = "Product deleted successfully." };
    }
}
