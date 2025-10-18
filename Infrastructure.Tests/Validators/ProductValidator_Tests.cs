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
            ProductName = name,
            ProductPrice = price,
            Category = new Category(),
            Manufacturer = new Manufacturer()
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
}
