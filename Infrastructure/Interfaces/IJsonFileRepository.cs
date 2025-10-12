using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IJsonFileRepository
{
    Task<List<Product>> ReadFromJsonFileAsync();
    Task<bool> WriteToJsonFileAsync(List<Product> productList);
}