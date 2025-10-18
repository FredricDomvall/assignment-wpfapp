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

}
