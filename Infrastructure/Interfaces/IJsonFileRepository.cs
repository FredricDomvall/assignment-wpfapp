using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IJsonFileRepository
{
    Task<List<Product>> ReadFromJsonFile();
    Task<bool> WriteToJsonFile(List<Product> productList);
}