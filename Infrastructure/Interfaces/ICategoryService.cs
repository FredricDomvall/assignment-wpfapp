using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface ICategoryService
{
    Task<AnswerOutcome<Category>> AddCategoryToListAsync(Category Category);
    Task<AnswerOutcome<IEnumerable<Category>>> GetAllCategoriesFromListAsync();
    Task<AnswerOutcome<IEnumerable<Category>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}
