using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface ICategoryService
{
    Task<AnswerOutcome<Category>> AddCategoryToListAsync(Category Category);
    AnswerOutcome<IEnumerable<Category>> GetAllCategoriesFromList();
    Task<AnswerOutcome<Category>> UpdateCategoryInListByIdAsync(Guid categoryId, Category category);
    Task<AnswerOutcome<bool>> DeleteCategoryFromListByIdAsync(Guid categoryId);
    Task<AnswerOutcome<IEnumerable<Category>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}
