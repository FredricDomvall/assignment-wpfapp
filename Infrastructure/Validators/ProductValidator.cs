using Infrastructure.Models;

namespace Infrastructure.Validators;
public class ProductValidator
{
    public AnswerOutcome<bool> ValidateGuidId(Guid productId, List<Product> productsList)
    {
        if (productId == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id was not set properly." };

        if (productsList.Any(p => p.ProductId == productId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id already exists in the list." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for id passed successfully" };
    }
    public AnswerOutcome<bool> ValidateName(string stringInputs)
    {
        if (string.IsNullOrWhiteSpace(stringInputs))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name can not be left empty" };

        if (stringInputs.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name must be at least 3 characters long." };
        if (stringInputs.Length > 10)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name cannot be longer than 10 characters." };

        if (stringInputs.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for price passed successfully" };
    }
    public AnswerOutcome<bool> ValidateDecimalPrice(string decimalPrice)
    {
        if (decimalPrice is null)
            return new AnswerOutcome<bool> { Statement = false, Answer = "price can not be null." };

        if (string.IsNullOrWhiteSpace(decimalPrice))
            return new AnswerOutcome<bool> { Statement = false, Answer = "price can not be left empty." };

        if (!decimal.TryParse(decimalPrice, out decimal parsedPrice))
            return new AnswerOutcome<bool> { Statement = false, Answer = "price must be a decimal value." };

        if (parsedPrice >= 10000000)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Price cant be greater than: 9999999,99." };

        if (parsedPrice < 0)
            return new AnswerOutcome<bool> { Statement = false, Answer = "price must be greater than zero." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for price passed successfully" };
    }
    public AnswerOutcome<bool> ValidateCategory(string category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Category is not set.", Outcome = false };

        return new AnswerOutcome<bool> { Statement = true, Answer = "Category is set properly." };
    }
    public AnswerOutcome<bool> ValidateManufacturer(string manufacturer)
    {
        if (manufacturer == null || string.IsNullOrWhiteSpace(manufacturer))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Manufacturer is not set." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "Manufacturer is set properly." };
    }
    public AnswerOutcome<bool> ValidateProductUnique(Guid productId, string productName, List<Product> productList)
    {
        //I need to consider taking in whole object of product instead of just name and id so i can validate ProductCode later if needed

        var otherProducts = productList.Where(p => p.ProductId != productId);

        if (otherProducts.Any(p => p.ProductName == productName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product with the specified name already exists." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for unique passed successfully" };
    }
    public AnswerOutcome<bool> ProductCreateValidationControl(Guid Id, ProductForm checkProduct, List<Product> productList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();
        validationResults.Add(ValidateName(checkProduct.ProductName!));
        validationResults.Add(ValidateDecimalPrice(checkProduct.ProductPrice!));
        validationResults.Add(ValidateCategory(checkProduct.CategoryName!));
        validationResults.Add(ValidateManufacturer(checkProduct.ManufacturerName!));
        validationResults.Add(ValidateProductUnique(Id, checkProduct.ProductName!, productList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for create passed successfully." };

        else
            return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public AnswerOutcome<bool> ProductUpdateValidationControl(Product product, List<Product> productList)
    {
        ProductForm checkProduct = new ProductForm
        {
            ProductName = product.ProductName,
            ProductPrice = product.ProductPrice.ToString(),
            CategoryName = product.Category?.CategoryName,
            ManufacturerName = product.Manufacturer?.ManufacturerName
        };

        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();
        validationResults.Add(ValidateName(checkProduct.ProductName!));
        validationResults.Add(ValidateDecimalPrice(checkProduct.ProductPrice!));
        validationResults.Add(ValidateCategory(checkProduct.CategoryName!));
        validationResults.Add(ValidateManufacturer(checkProduct.ManufacturerName!));
        validationResults.Add(ValidateProductUnique(product.ProductId, checkProduct.ProductName, productList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for update passed successfully." };

        else
            return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public AnswerOutcome<bool> ValidateAllValidationResults(List<AnswerOutcome<bool>> productserviceListResult)
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

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validationcontrols passed successfully." };
    }
}
