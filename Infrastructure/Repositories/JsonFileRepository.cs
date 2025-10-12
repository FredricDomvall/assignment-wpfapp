using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Repositories;
public class JsonFileRepository : IJsonFileRepository
{
    private readonly string _filePath;
    public JsonFileRepository(string filePath)
    {
        _filePath = filePath;
    }
    public List<Product> ReadFromJsonFile()
    {
        throw new NotImplementedException();
    }

    public bool WriteToJsonFile(List<Product> productList)
    {
        throw new NotImplementedException();
    }
}