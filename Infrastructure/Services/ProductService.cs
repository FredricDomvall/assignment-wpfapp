using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Validators;

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
        ProductValidator productCreateValidation = new ProductValidator();
        Product newProduct = new Product();
        newProduct.Category = new Category();
        newProduct.Manufacturer = new Manufacturer();
         
        var result = new AnswerOutcome<bool> { Statement = false};
        do
        {
            newProduct.ProductId = GeneratorHelper.GenerateGuidId();
            result = productCreateValidation.ValidateGuidId(newProduct.ProductId, _productList);
        } while (!result.Statement);

        var validationResult = productCreateValidation.ProductCreateValidationControl(newProduct.ProductId, productForm, _productList);
        if (validationResult.Statement is true)
        {
            newProduct.ProductName = productForm.ProductName!;
            newProduct.ProductPrice = decimal.Parse(productForm.ProductPrice!);
            newProduct.Category.CategoryName = productForm.CategoryName!;
            newProduct.Category.CategoryPrefix = GeneratorHelper.GenerateCategoryPrefix(productForm.CategoryName!);
            newProduct.ProductCode = GeneratorHelper.GenerateArticleNumber(newProduct.Category.CategoryPrefix, _productList);
            newProduct.Manufacturer.ManufacturerName = productForm.ManufacturerName!;
            newProduct.Manufacturer.ManufacturerCountry = productForm.ManufacturerCountry!;
            newProduct.Manufacturer.ManufacturerEmail = productForm.ManufacturerEmail!;
            newProduct.ProductDescription = productForm.ProductDescription ?? "No description available.";
            _productList.Add(newProduct);
            await SaveListToFileAsync();
            return new AnswerOutcome<Product> { Statement = true, Answer = "Success.", Outcome = newProduct };
        }
        
        return new AnswerOutcome<Product> { Statement = false, Answer = validationResult.Answer };  
    }
    public AnswerOutcome<IEnumerable<Product>> GetAllProductsFromList()
    {   
        if (!_productList.Any())
            return new AnswerOutcome<IEnumerable<Product>> { Statement = false, Answer = "No products available.", Outcome = _productList };

        return new AnswerOutcome<IEnumerable<Product>> { Statement = true, Answer = "Success.", Outcome = _productList };
    }
    public async Task<AnswerOutcome<Product>> UpdateProductInListByIdAsync(Product product)
    {
        ProductValidator productUpdateValidation = new ProductValidator();

        var productToUpdate = _productList.FirstOrDefault(p => p.ProductId == product.ProductId);
        if (productToUpdate == null)
            return new AnswerOutcome<Product> { Statement = false, Answer = "Product with the specified ID does not exist." };

        var originalProduct = new Product();
        originalProduct.Category = new Category();
        originalProduct.Manufacturer = new Manufacturer();
        originalProduct.ProductId = productToUpdate.ProductId;
        originalProduct.ProductName = productToUpdate.ProductName;
        originalProduct.ProductPrice = productToUpdate.ProductPrice;
        originalProduct.Category.CategoryName = productToUpdate.Category.CategoryName;
        originalProduct.Manufacturer.ManufacturerName = productToUpdate.Manufacturer.ManufacturerName;
        originalProduct.Manufacturer.ManufacturerCountry = productToUpdate.Manufacturer.ManufacturerCountry;
        originalProduct.Manufacturer.ManufacturerEmail = productToUpdate.Manufacturer.ManufacturerEmail;

        var validationResult = productUpdateValidation.ProductUpdateValidationControl(product, _productList);
        
        if (validationResult.Statement is true)
        {
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.ProductPrice = product.ProductPrice;
            productToUpdate.Category.CategoryName = product.Category.CategoryName;
            productToUpdate.Manufacturer.ManufacturerName = product.Manufacturer.ManufacturerName;
            productToUpdate.Manufacturer.ManufacturerCountry = product.Manufacturer.ManufacturerCountry;
            productToUpdate.Manufacturer.ManufacturerEmail = product.Manufacturer.ManufacturerEmail;

            await SaveListToFileAsync();
            return new AnswerOutcome<Product> { Statement = true, Answer = "Product updated successfully.", Outcome = productToUpdate };
        }

        return new AnswerOutcome<Product> { Statement = false, Answer = validationResult.Answer, Outcome = originalProduct };

    }
    public async Task<AnswerOutcome<bool>> DeleteProductFromListByIdAsync(Guid productId)
    {
        if (!_productList.Any(p => p.ProductId == productId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product with the specified ID does not exist." };
       
        _productList.RemoveAll(p => p.ProductId == productId);
        await SaveListToFileAsync();
        return new AnswerOutcome<bool> { Statement = true, Answer = "Product deleted successfully." };
    }
    public async Task<AnswerOutcome<IEnumerable<Product>>> LoadListFromFileAsync()
    {
        var productsFromFile = await _jsonFileRepository.ReadFromJsonFileAsync(_filePath);
        if (productsFromFile.Outcome == null || productsFromFile.Outcome.Count == 0)
            return new AnswerOutcome<IEnumerable<Product>> { Statement = false, Outcome = productsFromFile.Outcome };

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
