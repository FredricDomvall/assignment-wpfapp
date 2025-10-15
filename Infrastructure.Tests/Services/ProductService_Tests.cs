using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Infrastructure.Tests.Services;
public class ProductService_Tests
{
    private readonly string _testFilePath = "jsonfile_for_testing.json";
    [Fact]
    public async Task AddProductToList_ShouldAddProductToList_WhenValidInput()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
        
        // Arrange
        var fileSources = new FileSources
        {
            ProductFileSource = _testFilePath,
            CategoryFileSource = _testFilePath,
            ManufacturerFileSource = _testFilePath
        };
        var productJsonFileRepository = new JsonFileRepository<Product>();
        var manufacturerJsonFileRepository = new JsonFileRepository<Manufacturer>();
        var categoryJsonFileRepository = new JsonFileRepository<Category>();

        var categoryService = new CategoryService(categoryJsonFileRepository, fileSources);
        var manufacturerService = new ManufacturerService(manufacturerJsonFileRepository, fileSources);

        IProductService productService = new ProductService(productJsonFileRepository, manufacturerService, categoryService, fileSources);
        var validProduct = new ProductForm
        {
            ProductName = "ValidTestProduct",
            ProductPrice = "12,34",
            CategoryName = "TestCategory",
            ManufacturerName = "TestManufacturer",
            ManufacturerCountry = "TestCountry",
            ManufacturerEmail = "test@example.com"
        };


        //Act
        var validResult = await productService.AddProductToListAsync(validProduct);
        var productExistsInList = await productService.GetAllProductsFromListAsync();

        //Assert
        Assert.True(validResult.Statement);
        Assert.Contains(productExistsInList.Outcome!, p => p.ProductName == "ValidTestProduct" && p.ProductPrice == 12.34m);
        Assert.True(File.Exists(_testFilePath));
    }

    [Fact]
    public async Task AddProductToList_ShouldReturnError_WhenInvalidInput()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
        
        // Arrange
        var fileSources = new FileSources
        {
            ProductFileSource = _testFilePath,
            CategoryFileSource = _testFilePath,
            ManufacturerFileSource = _testFilePath
        };
        var productJsonFileRepository = new JsonFileRepository<Product>();
        var manufacturerJsonFileRepository = new JsonFileRepository<Manufacturer>();
        var categoryJsonFileRepository = new JsonFileRepository<Category>();
        var categoryService = new CategoryService(categoryJsonFileRepository, fileSources);
        var manufacturerService = new ManufacturerService(manufacturerJsonFileRepository, fileSources);
        IProductService productService = new ProductService(productJsonFileRepository, manufacturerService, categoryService, fileSources);
        var invalidProduct = new ProductForm
        {
            ProductName = "",
            ProductPrice = "-5",
            CategoryName = "TestCategory",
            ManufacturerName = "TestManufacturer",
            ManufacturerCountry = "TestCountry",
            ManufacturerEmail = "invalid-email"
        };
        
        // Act
        var invalidResult = await productService.AddProductToListAsync(invalidProduct);
        var productExistsInList = await productService.GetAllProductsFromListAsync();
        // Assert
        Assert.False(invalidResult.Statement);

        // Assert
        Assert.Contains("Text-inputs can not be left empty", invalidResult.Answer);
        Assert.Contains("price", invalidResult.Answer);

        Assert.DoesNotContain(productExistsInList.Outcome!, p => p.ProductName == "");
    }
    [Fact]
    public async Task GetAllProductsFromList_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
        
        // Arrange
        var fileSources = new FileSources
        {
            ProductFileSource = _testFilePath,
            CategoryFileSource = _testFilePath,
            ManufacturerFileSource = _testFilePath
        };
        var productJsonFileRepository = new JsonFileRepository<Product>();
        var manufacturerJsonFileRepository = new JsonFileRepository<Manufacturer>();
        var categoryJsonFileRepository = new JsonFileRepository<Category>();
        var categoryService = new CategoryService(categoryJsonFileRepository, fileSources);
        var manufacturerService = new ManufacturerService(manufacturerJsonFileRepository, fileSources);
        IProductService productService = new ProductService(productJsonFileRepository, manufacturerService, categoryService, fileSources);
        
        // Act
        var result = await productService.GetAllProductsFromListAsync();
        
        // Assert
        Assert.False(result.Statement);
        Assert.Equal("No products available.", result.Answer);
        Assert.Empty(result.Outcome!);
    }

}



