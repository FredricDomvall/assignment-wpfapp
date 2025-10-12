using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IProductService
{
    Task<AnswerOutcome<Product>> AddProductToListAsync(ProductForm productForm);
    Task<AnswerOutcome<IEnumerable<Product>>> GetAllProductsFromListAsync();
    Task<AnswerOutcome<IEnumerable<Product>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}