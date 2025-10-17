using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IProductService
{
    Task<AnswerOutcome<Product>> AddProductToListAsync(ProductForm productForm);
    AnswerOutcome<IEnumerable<Product>> GetAllProductsFromList();
    Task<AnswerOutcome<Product>> UpdateProductInListByIdAsync(Product product);
    Task<AnswerOutcome<bool>> DeleteProductFromListByIdAsync(Guid productId);
    Task<AnswerOutcome<IEnumerable<Product>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}