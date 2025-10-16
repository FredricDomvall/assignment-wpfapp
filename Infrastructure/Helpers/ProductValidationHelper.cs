using Infrastructure.Models;

namespace Infrastructure.Helpers;

public static class ProductValidationHelper
{
    public static AnswerOutcome<bool> ValidateGuidId<T>(Guid Id)
    {
        if (Id == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tId was not set properly." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateName(string stringInputs)
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
                return new AnswerOutcome<bool> { Statement = false, Answer = "price can not be left empty." };

            if (decimalPrice is null)
                return new AnswerOutcome<bool> { Statement = false, Answer = "price can not be null." };

            return new AnswerOutcome<bool> { Statement = false, Answer = "price must be a decimal value." };
        }
        else if (parsedPrice <= 0)
            return new AnswerOutcome<bool> { Statement = false, Answer = "price must be greater than zero." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateCategory(string category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Category is not set.", Outcome = false };

        return new AnswerOutcome<bool> { Statement = true, Answer = "Category is set.", Outcome = true };
    }
    public static AnswerOutcome<bool> ValidateManufacturer(string manufacturer)
    {
        if (manufacturer == null || string.IsNullOrWhiteSpace(manufacturer))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Manufacturer is not set.", Outcome = false };

        return new AnswerOutcome<bool> { Statement = true, Answer = "Manufacturer is set properly.", Outcome = true };
    }
    public static AnswerOutcome<bool> ValidateProductAlreadyExists(Guid productId, string productName, List<Product> productList)
    {
        bool existingProductId = productList.Any(p => p.ProductId == productId);
        if (existingProductId is true)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product with the specified ID already exists (odds minimal so press create again)"};

        bool existingProductName = productList.Any(p => p.ProductName == productName);
        if(existingProductName is true)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product with the specified name already exists." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "Product with the specified ID and name do not exist."};
    }
    public static AnswerOutcome<bool> ValidateProductUnique(Product checkProduct, List<Product> productList)
    {
        if (productList.Any(product => product.ProductName == checkProduct.ProductName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tProduct name must be unique." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<string> ValidateAllValidationResults(List<AnswerOutcome<bool>> productserviceListResult)
    {
        string errorMessages = "";

        bool allValid = productserviceListResult.All(r => r.Statement is true);
        if (allValid is not true)
        {
            foreach (var item in productserviceListResult)
                if (item.Statement is false)
                    errorMessages += item.Answer + "\n";
            return new AnswerOutcome<string> { Statement = false, Answer = "One or more Validationcontrols failed.", Outcome = errorMessages };
        }

        return new AnswerOutcome<string> { Statement = true, Answer = "All Validationcontrols passed successfully." };
    }
}
