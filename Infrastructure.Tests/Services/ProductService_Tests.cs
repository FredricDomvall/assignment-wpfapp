using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Moq;

namespace Infrastructure.Tests.Services;
public class ProductService_Tests
{
        private readonly Mock<IJsonFileRepository<Product>> _mockRepository;
        private readonly Mock<IManufacturerService> _mockManufacturerService;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly ProductService _productService;

        public ProductService_Tests()
        {
            _mockRepository = new Mock<IJsonFileRepository<Product>>();
            _mockManufacturerService = new Mock<IManufacturerService>();
            _mockCategoryService = new Mock<ICategoryService>();

            var fileSources = new FileSources { ProductFileSource = "testProducts.json" };
            _productService = new ProductService(
                _mockRepository.Object,
                _mockManufacturerService.Object,
                _mockCategoryService.Object,
                fileSources
            );
        }

        [Fact]
        public async Task AddProductToListAsync_ValidProductForm_ShouldReturnSuccess()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.WriteToJsonFileAsync(It.IsAny<string>(), It.IsAny<List<Product>>()))
                .ReturnsAsync(new AnswerOutcome<bool> { Statement = true });

            var productForm = new ProductForm
            {
                ProductName = "Laptop",
                ProductPrice = "1200",
                CategoryName = "Electronics",
                ManufacturerName = "Dell",
                ManufacturerCountry = "USA",
                ManufacturerEmail = "support@dell.com"
            };

            // Act
            var result = await _productService.AddProductToListAsync(productForm);

            // Assert
            Assert.True(result.Statement);
            Assert.Equal("Success.", result.Answer);
            Assert.NotNull(result.Outcome);
        }

        [Fact]
        public async Task AddProductToListAsync_InvalidProductForm_ShouldReturnFalse()
        {
            // Arrange
            var productForm = new ProductForm
            {
                ProductName = "",
                ProductPrice = "abc",
                CategoryName = "Electronics",
                ManufacturerName = "Dell",
                ManufacturerCountry = "USA",
                ManufacturerEmail = "support@dell.com"
            };

            // Act
            var result = await _productService.AddProductToListAsync(productForm);

            // Assert
            Assert.False(result.Statement);
        }

        [Fact]
        public async Task DeleteProductFromListByIdAsync_IdNotFound_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _productService.DeleteProductFromListByIdAsync(nonExistingId);

            // Assert
            Assert.False(result.Statement);
            Assert.Equal("Product with the specified ID does not exist.", result.Answer);
        }

        [Fact]
        public async Task SaveListToFileAsync_EmptyList_ShouldReturnFalse()
        {
            // Arrange

            // Act
            var result = await _productService.SaveListToFileAsync();

            // Assert
            Assert.False(result.Statement);
        }

        [Fact]
        public async Task LoadListFromFileAsync_FileEmpty_ShouldReturnFalse()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.ReadFromJsonFileAsync(It.IsAny<string>()))
                .ReturnsAsync(new AnswerOutcome<List<Product>> { Outcome = new List<Product>(), Statement = false });

            // Act
            var result = await _productService.LoadListFromFileAsync();

            // Assert
            Assert.False(result.Statement);
        }
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
            ProductName = "Valid",
            ProductPrice = "12",
            CategoryName = "Valid",
            ProductCode = "VAL-00001",
            ManufacturerName = "Valid",
            ManufacturerCountry = "Valid",
            ManufacturerEmail = "Valid@valid.com"
        };


        //Act
        var validResult = await productService.AddProductToListAsync(validProduct);
        var productExistsInList = productService.GetAllProductsFromList();

        //Assert
        Assert.True(validResult.Statement);
        Assert.Contains(productExistsInList.Outcome!, p => p.ProductName == "Valid" && p.ProductPrice == 12);
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
        var productExistsInList = productService.GetAllProductsFromList();
        // Assert
        Assert.False(invalidResult.Statement);

        // Assert
        Assert.Contains("Product name can not be left empty", invalidResult.Answer);
        Assert.Contains("price", invalidResult.Answer);

        Assert.DoesNotContain(productExistsInList.Outcome!, p => p.ProductName == "");
    }
    [Fact]
    public void GetAllProductsFromList_ShouldReturnEmptyList_WhenNoProductsExist()
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
        var result = productService.GetAllProductsFromList();
        
        // Assert
        Assert.False(result.Statement);
        Assert.Equal("No products available.", result.Answer);
        Assert.Empty(result.Outcome!);
    }

}
