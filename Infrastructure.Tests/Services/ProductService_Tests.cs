using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Infrastructure.Tests.Services;
public class ProductService_Tests
{
    private readonly string _testFilePath = "test_products.json";
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
        var categoryJsonFileRepository = new JsonFileRepository<Category>();
        var categoryService = new CategoryService(categoryJsonFileRepository, fileSources);

        var manufacturerJsonFileRepository = new JsonFileRepository<Manufacturer>();
        var manufacturerService = new ManufacturerService(manufacturerJsonFileRepository, fileSources);
        var jsonFileRepository = new JsonFileRepository<Product>();
        var productService = new ProductService(jsonFileRepository, manufacturerService, categoryService, fileSources);
        var validProduct = new ProductForm
        {
            ProductName = "ValidTestProduct",
            ProductPrice = "12,34"
        };
        var invalidProduct = new ProductForm
        {
            ProductName = " ",
            ProductPrice = "abc"
        };


        //Act
        var validResult = await productService.AddProductToListAsync(validProduct);
        var invalidResult = await productService.AddProductToListAsync(invalidProduct);
        var productExistsInList = await productService.GetAllProductsFromListAsync();

        //Assert
        Assert.True(validResult.Statement);
        Assert.False(invalidResult.Statement);
        Assert.Contains(productExistsInList.Outcome!, p => p.ProductName == "ValidTestProduct" && p.ProductPrice == 12.34m);
        Assert.True(File.Exists(_testFilePath));
    }
}
