using Infrastructure.Models;
using Infrastructure.Validators;
using FluentAssertions; // <-- Add this using directive

namespace Infrastructure.Tests.Validators;
public class ProductValidator_Tests
{
    [Theory]
    [InlineData("", 10.0)]
    [InlineData("ValidName", -5.0)]
    public void ProductValidator_InvalidProduct_ReturnsValidationErrors(string name, decimal price)
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            ProductName = name,
            ProductPrice = price,
            Category = new Category { CategoryId = Guid.NewGuid(), CategoryName = "Cat" },
            Manufacturer = new Manufacturer { ManufacturerId = Guid.NewGuid(), ManufacturerName = "Man" }
        };
        var validator = new ProductValidator();

        // Act
        var nameResult = validator.ValidateName(product.ProductName);
        var priceResult = validator.ValidateDecimalPrice(product.ProductPrice.ToString());

        // Assert
        var errors = new List<string>();
        if (nameResult.Outcome != true && !string.IsNullOrWhiteSpace(nameResult.Answer))
            errors.Add(nameResult.Answer!);
        if (priceResult.Outcome != true && !string.IsNullOrWhiteSpace(priceResult.Answer))
            errors.Add(priceResult.Answer!);

        errors.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateName_EmptyOrWhitespace_ReturnsError(string input)
    {
        // Arrange
        var validator = new ProductValidator();

        // Act
        var result = validator.ValidateName(input);

        // Assert
        result.Outcome.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("-5")]
    [InlineData("notanumber")]
    public void ValidateDecimalPrice_InvalidInputs_ReturnsError(string priceInput)
    {
        // Arrange
        var validator = new ProductValidator();

        // Act
        var result = validator.ValidateDecimalPrice(priceInput);

        // Assert
        result.Outcome.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ValidateDecimalPrice_ValidInput_ReturnsTrue()
    {
        // Arrange
        var validator = new ProductValidator();

        // Act
        var result = validator.ValidateDecimalPrice("15");

        // Assert
        result.Statement.Should().BeTrue();
    }

    [Fact]
    public void ValidateGuidId_EmptyGuid_ReturnsError()
    {
        // Arrange
        var validator = new ProductValidator();
        var products = new List<Product>();

        // Act
        var result = validator.ValidateGuidId(Guid.Empty, products);

        // Assert
        result.Outcome.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ValidateGuidId_DuplicateGuid_ReturnsError_And_UniqueGuid_ReturnsTrue()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var products = new List<Product>
        {
            new Product { ProductId = existingId, ProductName = "Existing", ProductPrice = 1, Category = new Category(), Manufacturer = new Manufacturer() }
        };
        var validator = new ProductValidator();

        // Act
        var duplicateResult = validator.ValidateGuidId(existingId, products);
        var uniqueResult = validator.ValidateGuidId(Guid.NewGuid(), products);

        // Assert
        duplicateResult.Outcome.Should().BeFalse();
        duplicateResult.Answer.Should().NotBeNullOrWhiteSpace();

        uniqueResult.Statement.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateCategory_Empty_ReturnsError(string categoryInput)
    {
        var validator = new ProductValidator();
        var result = validator.ValidateCategory(categoryInput);

        result.Outcome.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ValidateCategory_Valid_ReturnsTrue()
    {
        var validator = new ProductValidator();
        var result = validator.ValidateCategory("Electronics");

        result.Statement.Should().BeTrue();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateManufacturer_Empty_ReturnsError(string manufacturerInput)
    {
        var validator = new ProductValidator();
        var result = validator.ValidateManufacturer(manufacturerInput);

        result.Outcome.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ValidateManufacturer_Valid_ReturnsTrue()
    {
        var validator = new ProductValidator();
        var result = validator.ValidateManufacturer("Acme Inc.");

        result.Statement.Should().BeTrue();
    }

    [Fact]
    public void ValidateProductUnique_DuplicateName_ReturnsError_And_UniqueName_ReturnsTrue()
    {
        // Arrange
        var list = new List<Product>
        {
            new Product { ProductId = Guid.NewGuid(), ProductName = "SameName", ProductPrice = 1, Category = new Category(), Manufacturer = new Manufacturer() }
        };
        var validator = new ProductValidator();

        // Act
        var duplicateResult = validator.ValidateProductUnique(Guid.NewGuid(), "SameName", list);
        var uniqueResult = validator.ValidateProductUnique(Guid.NewGuid(), "DifferentName", list);

        // Assert
        duplicateResult.Statement.Should().BeFalse();
        duplicateResult.Answer.Should().NotBeNullOrWhiteSpace();

        uniqueResult.Statement.Should().BeTrue();
    }

    [Fact]
    public void ProductCreateValidationControl_InvalidForm_ReturnsError()
    {
        // Arrange
        var validator = new ProductValidator();
        var form = new ProductForm
        {
            ProductName = "",             // invalid
            ProductPrice = "notdecimal",  // invalid
            CategoryName = "",            // invalid
            ManufacturerName = ""         // invalid
        };
        var productList = new List<Product>();

        // Act
        var result = validator.ProductCreateValidationControl(Guid.NewGuid(), form, productList);

        // Assert
        result.Outcome.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }
    [Fact]
    public void ProductCreateValidationControl_ValidForm_ReturnsTrue()
    {
        // Arrange
        var validator = new ProductValidator();
        var form = new ProductForm
        {
            ProductName = "NewProduct",
            ProductPrice = "19",
            CategoryName = "Cat",
            ManufacturerName = "Man",
            ManufacturerCountry = "Country",
            ManufacturerEmail = "a@b.com"
        };
        var productList = new List<Product>();

        // Act
        var result = validator.ProductCreateValidationControl(Guid.NewGuid(), form, productList);

        (result.Statement).Should().BeTrue();
    }

    // NEW TESTS FOR ProductUpdateValidationControl AND ValidateAllValidationResults

    [Fact]
    public void ProductUpdateValidationControl_InvalidProduct_ReturnsError()
    {
        // Arrange
        var existingList = new List<Product>();
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            ProductName = "",                // invalid
            ProductPrice = -10m,             // invalid
            Category = new Category { CategoryName = "" }, // invalid
            Manufacturer = new Manufacturer { ManufacturerName = " " } // invalid (whitespace)
        };
        var validator = new ProductValidator();

        // Act
        var result = validator.ProductUpdateValidationControl(product, existingList);

        // Assert
        result.Statement.Should().BeFalse();
        result.Answer.Should().NotBeNullOrWhiteSpace();
    }
}
