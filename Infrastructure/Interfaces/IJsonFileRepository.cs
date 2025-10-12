using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IJsonFileRepository
{
    List<Product> ReadFromJsonFile();
    bool WriteToJsonFile(List<Product> productList);
}