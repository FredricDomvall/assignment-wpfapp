using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Infrastructure.Tests.Services;
public class ProductService_Tests
{
    private readonly string _testFilePath = "test_products.json";
    [Fact]
    public void AddProductToList_ShouldAddProductToList_WhenValidInput()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
        //Arrange
        var jsonFileRepository = new JsonFileRepository(_testFilePath);
        var productService = new ProductService(jsonFileRepository);
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
        var validResult = productService.AddProductToList(validProduct);
        var invalidResult = productService.AddProductToList(invalidProduct);
        var productExistsInList = productService.GetAllProductsFromListAsync();

        //Assert
        Assert.True(validResult);
        Assert.False(invalidResult);
        Assert.Contains(productExistsInList, p => p.ProductName == "ValidTestProduct" && p.ProductPrice == 12.34m);
        Assert.True(File.Exists(_testFilePath));
        Assert.Single(productExistsInList);
    }
}
