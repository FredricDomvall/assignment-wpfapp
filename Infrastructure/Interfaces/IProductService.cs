using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IProductService
{
    Task<AnswerOutcome<Product>> AddProductToListAsync(ProductForm productForm);
    Task<AnswerOutcome<IEnumerable<Product>>> GetAllProductsFromListAsync();
    Task<AnswerOutcome<Product>> UpdateProductInListByIdAsync(Guid productId, ProductForm productForm);
    Task<AnswerOutcome<bool>> DeleteProductFromListByIdAsync(Guid productId);
    Task<AnswerOutcome<IEnumerable<Product>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}