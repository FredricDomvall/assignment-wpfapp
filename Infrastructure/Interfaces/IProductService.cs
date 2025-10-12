using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IProductService
{
    bool AddProductToList(ProductForm productForm);
    IEnumerable<Product> GetAllProductsFromList();
    Task<IEnumerable<Product>> LoadListFromFileAsync();
    Task<bool> SaveListToFileAsync();
}