using Infrastructure.Models;

namespace Infrastructure.Helpers;

public static class ProductValidationHelper
{
    public static AnswerOutcome<bool> ValidateGuidId(Guid Id, List<Product> productsList)
    {
        if (Id == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tId was not set properly." };
        if (productsList.Any(p => p.ProductId == Id))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tId already exists in the list." };


        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateName(string stringInputs)
    {
        if (string.IsNullOrWhiteSpace(stringInputs))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name can not be left empty" };

        if (stringInputs.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name be at least 3 characters long." };
        if (stringInputs.Length > 10)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name can not be longer than 10 characters." };

        if (stringInputs.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name not contain numbers." };

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
        else if (parsedPrice >= 5)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Price cant be greater than: 9999999,99." };
        else if (parsedPrice < 0)
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
    public static AnswerOutcome<bool> ValidateProductUnique(ProductForm checkProduct, List<Product> productList)
    {
        if (productList.Any(product => product.ProductName == checkProduct.ProductName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tProduct name must be unique." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ProductCreateValidationControl(ProductForm checkProduct, List<Product> productList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();
        validationResults.Add(ValidateName(checkProduct.ProductName!));
        validationResults.Add(ValidateDecimalPrice(checkProduct.ProductPrice!));
        validationResults.Add(ValidateCategory(checkProduct.CategoryName!));
        validationResults.Add(ValidateManufacturer(checkProduct.ManufacturerName!));
        validationResults.Add(ValidateProductUnique(checkProduct, productList));
        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All Validationcontrols passed successfully." };
        else
            return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public static AnswerOutcome<bool> ProductUpdateValidationControl(ProductForm checkProduct, List<Product> productList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();
        validationResults.Add(ValidateName(checkProduct.ProductName!));
        validationResults.Add(ValidateDecimalPrice(checkProduct.ProductPrice!));
        validationResults.Add(ValidateCategory(checkProduct.CategoryName!));
        validationResults.Add(ValidateManufacturer(checkProduct.ManufacturerName!));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All Validationcontrols passed successfully." };
        else
            return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }

    public static AnswerOutcome<bool> ValidateAllValidationResults(List<AnswerOutcome<bool>> productserviceListResult)
    {
        string errorMessages = "";

        bool allValid = productserviceListResult.All(r => r.Statement is true);
        if (allValid is not true)
        {
            foreach (var item in productserviceListResult)
                if (item.Statement is false)
                    errorMessages += item.Answer + "\n";
            return new AnswerOutcome<bool> { Statement = false, Answer = errorMessages };
        }

        return new AnswerOutcome<bool> { Statement = true, Answer = "All Validationcontrols passed successfully." };
    }

}
