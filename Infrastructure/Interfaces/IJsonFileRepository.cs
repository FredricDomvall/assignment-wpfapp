using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IJsonFileRepository<T>
{
    Task<AnswerOutcome<List<T>>> ReadFromJsonFileAsync();
    Task<bool> WriteToJsonFileAsync(List<T> productList);
}