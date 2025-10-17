using Infrastructure.Models;

namespace Infrastructure.Helpers;
public static class CategoryValidationHelper
{
    public static AnswerOutcome<bool> ValidateGuidId(Guid Id, List<Category> categoryList)
    {
        if (Id == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id was not set properly." };
        if (categoryList.Any(c => c.CategoryId == Id))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id already exists in the list." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateCategoryName(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Text-inputs can not be left empty" };

        if (categoryName.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Text-inputs be at least 3 characters long." };

        if (categoryName.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Text-inputs not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true };
    } 
    public static AnswerOutcome<bool> ValidateCategoryPrefix(string prefix, List<Category> categoryList)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return new AnswerOutcome<bool> { Statement = false, Answer = "prefix can not be left empty." };

        if (categoryList.Any(c => c.CategoryPrefix == prefix))
            return new AnswerOutcome<bool> { Statement = false, Answer = "prefix must be unique." };
        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateCategoryUnique(Category checkCategory, List<Category> categoryList)
    {
        if (categoryList.Any(c => c.CategoryName == checkCategory.CategoryName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Product name must be unique." };

        if (categoryList.Any(c => c.CategoryId == checkCategory.CategoryId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id already exists in the list." };

        if (categoryList.Any(c => c.CategoryPrefix == checkCategory.CategoryPrefix))
            return new AnswerOutcome<bool> { Statement = false, Answer = "prefix must be unique." };
        return new AnswerOutcome<bool> { Statement = true };

    }
    public static AnswerOutcome<bool> CategoryCreateValidationControl(Category checkCategory, List<Category> categoryList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();

        validationResults.Add(ValidateCategoryName(checkCategory.CategoryName!));
        validationResults.Add(ValidateCategoryPrefix(checkCategory.CategoryPrefix, categoryList));
        validationResults.Add(ValidateCategoryUnique(checkCategory, categoryList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All Validationcontrols passed successfully." };

        else return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
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
