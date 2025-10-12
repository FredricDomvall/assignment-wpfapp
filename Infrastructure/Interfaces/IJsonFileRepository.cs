using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IJsonFileRepository
{
    Task<List<Product>> ReadFromJsonFile();
    bool WriteToJsonFile(List<Product> productList);
}