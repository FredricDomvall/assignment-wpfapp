using Infrastructure.Models;

namespace Infrastructure.Validators;

public class CategoryValidator
{
    public AnswerOutcome<bool> ValidateGuidId(Guid categoryId, List<Category> categoryList)
    {
        if (categoryId == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id was not set properly." };

        if (categoryList.Any(c => c.CategoryId == categoryId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Id already exists in the list." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for id passed successfully" };
    }
    public AnswerOutcome<bool> ValidateCategoryName(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Name can not be left empty" };

        if (categoryName.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Name be at least 3 characters long." };

        if (categoryName.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Name can not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for name passed successfully" };
    }
    public AnswerOutcome<bool> ValidateCategoryPrefix(string prefix, List<Category> categoryList)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Prefix can not be left empty." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public AnswerOutcome<bool> ValidateCategoryUnique(Category checkCategory, List<Category> categoryList)
    {
        var otherCategories = categoryList.Where(c => c.CategoryId != checkCategory.CategoryId);

        if (otherCategories.Any(c => c.CategoryName == checkCategory.CategoryName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Category name must be unique." };

        if (otherCategories.Any(c => c.CategoryPrefix == checkCategory.CategoryPrefix))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Prefix must be unique." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for unique passed succesfully" };
    }
    public AnswerOutcome<bool> CategoryCreateValidationControl(Category checkCategory, List<Category> categoryList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();

        validationResults.Add(ValidateGuidId(checkCategory.CategoryId, categoryList));
        validationResults.Add(ValidateCategoryName(checkCategory.CategoryName!));
        validationResults.Add(ValidateCategoryPrefix(checkCategory.CategoryPrefix, categoryList));
        validationResults.Add(ValidateCategoryUnique(checkCategory, categoryList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for create passed successfully." };

        else return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public AnswerOutcome<bool> CategoryUpdateValidationControl(Category checkCategory, List<Category> categoryList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();

        validationResults.Add(ValidateCategoryName(checkCategory.CategoryName!));
        validationResults.Add(ValidateCategoryPrefix(checkCategory.CategoryPrefix, categoryList));
        validationResults.Add(ValidateCategoryUnique(checkCategory, categoryList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All validation update passed successfully." };

        else return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public AnswerOutcome<bool> ValidateAllValidationResults(List<AnswerOutcome<bool>> categoryServiceListResult)
    {
        string errorMessages = "";

        bool allValid = categoryServiceListResult.All(r => r.Statement is true);
        if (allValid is not true)
        {
            foreach (var item in categoryServiceListResult)
                if (item.Statement is false)
                    errorMessages += item.Answer + "\n";

            return new AnswerOutcome<bool> { Statement = false, Answer = errorMessages };
        }

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validationcontrols passed successfully." };
    }
}
