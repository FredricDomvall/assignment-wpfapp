using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Text.Json;

namespace Infrastructure.Repositories;
public class JsonFileRepository<T> : IJsonFileRepository<T>
{
    public async Task<AnswerOutcome<List<T>>> ReadFromJsonFileAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return new AnswerOutcome<List<T>> { Statement = false, Answer = "File doesnt exist", Outcome = new List<T>() };

            var jsonData = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(jsonData))
                return new AnswerOutcome<List<T>> { Statement = false, Answer = "File is empty", Outcome = new List<T>() };

            var productData = JsonSerializer.Deserialize<List<T>>(jsonData);

            if (productData == null)
                return new AnswerOutcome<List<T>> { Statement = false, Answer = "File is empty", Outcome = new List<T>() };

            return new AnswerOutcome<List<T>> { Statement = true, Outcome = productData };
        }
        catch (Exception ex)
        {
            return new AnswerOutcome<List<T>> { Statement = false, Answer = ex.Message, Outcome = new List<T>() };
        }
    }

    public async Task<AnswerOutcome<bool>> WriteToJsonFileAsync(string filePath, List<T> productList)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonData = JsonSerializer.Serialize(productList, options);
            await File.WriteAllTextAsync(filePath, jsonData);

            return new AnswerOutcome<bool> { Statement = true, Answer = "Successfully saved list to file"};
        }
        catch(Exception ex)
        {
            return new AnswerOutcome<bool> { Statement = false, Answer = ex.Message };
        }
    }
}