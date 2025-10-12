using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IManufacturerService
{
    Task<AnswerOutcome<Manufacturer>> AddManufacturerToListAsync(Manufacturer manufacturer);
    Task<AnswerOutcome<IEnumerable<Manufacturer>>> GetAllManufacturersFromListAsync();
    Task<AnswerOutcome<IEnumerable<Manufacturer>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}
