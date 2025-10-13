using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IJsonFileRepository<T>
{
    Task<AnswerOutcome<List<T>>> ReadFromJsonFileAsync(string filePath);
    Task<AnswerOutcome<bool>> WriteToJsonFileAsync(string filePath, List<T> productList);
}