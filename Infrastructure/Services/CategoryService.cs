using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;
public class CategoryService : ICategoryService
{
    private List<Category> _categoryList = new List<Category>();

    private readonly IJsonFileRepository<Category> _jsonFileRepository;
    public CategoryService(IJsonFileRepository<Category> jsonFileRepository, FileSources fileSource)
    {
        _jsonFileRepository = new JsonFileRepository<Category>(fileSource.CategoryFileSource);
    }
    public async Task<AnswerOutcome<Category>> AddCategoryToListAsync(Category categoryForm)
    {
        Category newCategory = new Category();
        categoryForm.CategoryId = GeneratorHelper.GenerateGuidId();
        

        var nameValidationResult = ValidationHelper.ValidateString(categoryForm.CategoryName!);
        if (nameValidationResult.Statement is true)
            categoryForm.CategoryPrefix = GeneratorHelper.GenerateCategoryPrefix(categoryForm.CategoryName!); 
        
        var prefixValidationResult = ValidationHelper.ValidateCategoryPrefix(categoryForm.CategoryPrefix!, _categoryList);
        var guidValidationResult = ValidationHelper.ValidateGuidId<Category>(newCategory.CategoryId);
        var uniqueValidationResult = ValidationHelper.ValidateCategoryUnique(newCategory, _categoryList);

        if (nameValidationResult.Statement is true && prefixValidationResult.Statement is true
         && guidValidationResult.Statement is true && uniqueValidationResult.Statement is true)
        {
            newCategory.CategoryId = categoryForm.CategoryId;
            newCategory.CategoryName = categoryForm.CategoryName!;
            newCategory.CategoryPrefix = categoryForm.CategoryPrefix!;

            await LoadListFromFileAsync();
            _categoryList.Add(newCategory);
            await SaveListToFileAsync();
            return new AnswerOutcome<Category> { Statement = true, Answer = "Success.", Outcome = newCategory };
        }
        else
        {
            string errorMessages = "";
            if (nameValidationResult.Statement is false)
                errorMessages += nameValidationResult.Answer + "\n";
            if (prefixValidationResult.Statement is false)
                errorMessages += prefixValidationResult.Answer + "\n";
            if (guidValidationResult.Statement is false)
                errorMessages += guidValidationResult.Answer + "\n";
            if (uniqueValidationResult.Statement is false)
                errorMessages += uniqueValidationResult.Answer + "\n";
            return new AnswerOutcome<Category> { Statement = false, Answer = errorMessages.Trim() };
        }
    }

    public async Task<AnswerOutcome<IEnumerable<Category>>> GetAllCategoriesFromListAsync()
    {
        await LoadListFromFileAsync();
        return new AnswerOutcome<IEnumerable<Category>> { Statement = true, Answer = "Success.", Outcome = _categoryList };
    }

    public async Task<AnswerOutcome<IEnumerable<Category>>> LoadListFromFileAsync()
    {
        var categoriesFromFile = await _jsonFileRepository.ReadFromJsonFileAsync();
        if (categoriesFromFile.Outcome == null || categoriesFromFile.Outcome.Count == 0)
            return new AnswerOutcome<IEnumerable<Category>> { Statement = false };

        _categoryList = categoriesFromFile.Outcome;
        return new AnswerOutcome<IEnumerable<Category>> { Statement = true };
    }

    public async Task<AnswerOutcome<bool>> SaveListToFileAsync()
    {
        if (!_categoryList.Any())
            return new AnswerOutcome<bool> { Statement = false };

        await _jsonFileRepository.WriteToJsonFileAsync(_categoryList);
        return new AnswerOutcome<bool> { Statement = true };
    }
}
