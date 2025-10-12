using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Text.Json;

namespace Infrastructure.Repositories;
public class JsonFileRepository<T> : IJsonFileRepository
{
    private readonly string _filePath;
    public JsonFileRepository(string filePath)
    {
        _filePath = filePath;
    }
    public async Task<List<Product>> ReadFromJsonFile()
    {
        try
        {
            if(!File.Exists(_filePath))
                return new List<Product>();

            var jsonData = await File.ReadAllTextAsync(_filePath);
            var productData = JsonSerializer.Deserialize<List<Product>>(jsonData);

            if (productData == null)
                return new List<Product>();

            return productData;
        }
        catch
        {
            return new List<Product>();
        }
    }

    public async Task<bool> WriteToJsonFile(List<Product> productList)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonData = JsonSerializer.Serialize(productList, options);
            await File.WriteAllTextAsync(_filePath, jsonData);

            return true;
        }
        catch
        {
            return false;
        }
    }
}