using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Validators;

namespace Infrastructure.Services;
public class CategoryService : ICategoryService
{
    private List<Category> _categoryList = new List<Category>();
    private readonly IJsonFileRepository<Category> _jsonFileRepository;
    private readonly string _filePath;
    public CategoryService(IJsonFileRepository<Category> jsonFileRepository, FileSources filePath)
    {
        _jsonFileRepository = jsonFileRepository;
        _filePath = filePath.CategoryFileSource;
    }
    public async Task<AnswerOutcome<Category>> AddCategoryToListAsync(Category categoryForm)
    {
        CategoryValidator categoryCreateValidator = new CategoryValidator();

        Category newCategory = new Category();
        var result = new AnswerOutcome<bool> { Statement = false };
        do
        {
            categoryForm.CategoryId = GeneratorHelper.GenerateGuidId();
            result = categoryCreateValidator.ValidateGuidId(categoryForm.CategoryId, _categoryList);    
        } while (!result.Statement);

        categoryForm.CategoryPrefix = GeneratorHelper.GenerateCategoryPrefix(categoryForm.CategoryName!);
        
        var validationResult = categoryCreateValidator.CategoryCreateValidationControl(categoryForm, _categoryList);

        if (validationResult.Statement is true)
        {
            newCategory.CategoryId = categoryForm.CategoryId;
            newCategory.CategoryName = categoryForm.CategoryName!;
            newCategory.CategoryPrefix = categoryForm.CategoryPrefix!;

            _categoryList.Add(newCategory);
            await SaveListToFileAsync();

            return new AnswerOutcome<Category> { Statement = true, Answer = "Success.", Outcome = newCategory };
        }
        
        return new AnswerOutcome<Category> { Statement = false, Answer = validationResult.Answer };      
    }

    public AnswerOutcome<IEnumerable<Category>> GetAllCategoriesFromList()
    {
        if (!_categoryList.Any())
            return new AnswerOutcome<IEnumerable<Category>> { Statement = false, Answer = "No categories available.", Outcome = _categoryList };
 
        return new AnswerOutcome<IEnumerable<Category>> { Statement = true, Answer = "Success.", Outcome = _categoryList };
    }
    public async Task<AnswerOutcome<Category>> UpdateCategoryInListByIdAsync(Category category)
    {
        CategoryValidator categoryUpdateValidator = new CategoryValidator();

        var categoryToUpdate = _categoryList.FirstOrDefault(c => c.CategoryId == category.CategoryId);
        if (categoryToUpdate == null)
            return new AnswerOutcome<Category> { Statement = false, Answer = "Category with the specified ID does not exist." };

        var originalCategory = new Category
        {
            CategoryId = categoryToUpdate.CategoryId,
            CategoryPrefix = categoryToUpdate.CategoryPrefix,
            CategoryName = categoryToUpdate.CategoryName
        };

        var validationResult = categoryUpdateValidator.CategoryUpdateValidationControl(category, _categoryList);

        if (validationResult.Statement is true)
        {
            categoryToUpdate.CategoryName = category.CategoryName!;
            categoryToUpdate.CategoryPrefix = category.CategoryPrefix!;

            await SaveListToFileAsync();
            return new AnswerOutcome<Category> { Statement = true, Answer = $"Successfully changed Categoryname to: {categoryToUpdate.CategoryName}" };
        }
        
        return new AnswerOutcome<Category> { Statement = false, Answer = validationResult.Answer, Outcome = originalCategory };
       
    }

    public async Task<AnswerOutcome<bool>> DeleteCategoryFromListByIdAsync(Guid categoryId)
    {
        if (!_categoryList.Any(c => c.CategoryId == categoryId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Category with the specified ID does not exist." };

        _categoryList.RemoveAll(c => c.CategoryId == categoryId);
        await SaveListToFileAsync();
        return new AnswerOutcome<bool> { Statement = true, Answer = "Category deleted successfully." };
    }
    public async Task<AnswerOutcome<IEnumerable<Category>>> LoadListFromFileAsync()
    {
        var categoriesFromFile = await _jsonFileRepository.ReadFromJsonFileAsync(_filePath);
        if (categoriesFromFile.Outcome == null || categoriesFromFile.Outcome.Count == 0)
            return new AnswerOutcome<IEnumerable<Category>> { Statement = false };

        _categoryList = categoriesFromFile.Outcome;
        return new AnswerOutcome<IEnumerable<Category>> { Statement = true };
    }

    public async Task<AnswerOutcome<bool>> SaveListToFileAsync()
    {
        if (!_categoryList.Any())
            return new AnswerOutcome<bool> { Statement = false };

        await _jsonFileRepository.WriteToJsonFileAsync(_filePath, _categoryList);
        return new AnswerOutcome<bool> { Statement = true };
    }
}
