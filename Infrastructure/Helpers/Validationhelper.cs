using Infrastructure.Models;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers;
public static class ValidationHelper
{
    public static AnswerOutcome<bool> ValidateString(string stringInputs, List<Product> productList)
    {
        if (string.IsNullOrWhiteSpace(stringInputs))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tText-inputs can not be left empty" };

        if (stringInputs.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tText-inputs be at least 3 characters long." };
        
        if (stringInputs.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tText-inputs not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateDecimalPrice(string decimalPrice)
    {
        var result = decimal.TryParse(decimalPrice, out decimal parsedPrice);
        if (!result)
        {
            if (string.IsNullOrWhiteSpace(decimalPrice))
                return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice can not be left empty." };

            if (decimalPrice is null)
                return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice can not be null." };

            return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice must be a decimal value." };
        }
        else if (parsedPrice <= 0)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice must be greater than zero." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateGuidId<T>(Guid Id)
    {
        if (Id == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tId was not set properly." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Email cannot be empty." };

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, pattern))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Email format is invalid." };

        var parts = email.Split('@');
        if (parts.Length != 2)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Email must contain exactly one '@'." };

        if (!parts[1].Contains('.'))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Email must contain a '.' after '@'." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    /*
            Below validation for unique values in lists can be changed to generic handlig, Improve later
     */
    public static AnswerOutcome<bool> ValidateProductUnique(Product checkProduct, List<Product> productList)
    {
        if (productList.Any(product => product.ProductName == checkProduct.ProductName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tProduct name must be unique." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateCategoryUnique(Category checkCategory, List<Category> categoryList)
    {
        if (categoryList.Any(category => category.CategoryName == checkCategory.CategoryName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tCategory name must be unique." };

        if (categoryList.Any(category => category.CategoryId == checkCategory.CategoryId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tCategory Id must be unique." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateManufacturerUnique(Manufacturer checkManufacturer, List<Manufacturer> manufacturerList)
    {
        if (manufacturerList.Any(m => m.ManufacturerName == checkManufacturer.ManufacturerName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tManufacturer name must be unique." };

        if (manufacturerList.Any(m => m.ManufacturerId == checkManufacturer.ManufacturerId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tManufacturer Id must be unique." };

        if (manufacturerList.Any(m => m.ManufacturerEmail == checkManufacturer.ManufacturerEmail))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tManufacturer email must be unique." };

        return new AnswerOutcome<bool> { Statement = true };
    }
}
