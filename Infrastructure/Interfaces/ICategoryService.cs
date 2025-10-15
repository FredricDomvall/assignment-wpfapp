using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface ICategoryService
{
    Task<AnswerOutcome<Category>> AddCategoryToListAsync(Category Category);
    AnswerOutcome<IEnumerable<Category>> GetAllCategoriesFromList();
    Task<AnswerOutcome<IEnumerable<Category>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}
