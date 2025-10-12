using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IProductService
{
    Task<AnswerOutcome<bool>> AddProductToListAsync(ProductForm productForm);
    Task<IEnumerable<Product>> GetAllProductsFromListAsync();
    Task<IEnumerable<Product>> LoadListFromFileAsync();
    Task<bool> SaveListToFileAsync();
}