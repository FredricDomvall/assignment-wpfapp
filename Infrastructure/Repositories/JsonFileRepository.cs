using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Text.Json;

namespace Infrastructure.Repositories;
public class JsonFileRepository<T> : IJsonFileRepository<T>
{
    private readonly string _filePath;
    public JsonFileRepository(string filePath)
    {
        _filePath = filePath;
    }
    public async Task<List<T>> ReadFromJsonFileAsync()
    {
        try
        {
            if(!File.Exists(_filePath))
                return new List<T>();

            var jsonData = await File.ReadAllTextAsync(_filePath);
            var productData = JsonSerializer.Deserialize<List<T>>(jsonData);

            if (productData == null)
                return new List<T>();

            return productData;
        }
        catch
        {
            return new List<T>();
        }
    }

    public async Task<bool> WriteToJsonFileAsync(List<T> productList)
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