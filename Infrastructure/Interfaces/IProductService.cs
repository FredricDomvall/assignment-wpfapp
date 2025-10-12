using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IProductService
{
    bool AddProductToList(ProductForm productForm);
    IEnumerable<Product> GetAllProductsFromList();
    IEnumerable<Product> LoadListFromFile();
    bool SaveListToFile();


}
