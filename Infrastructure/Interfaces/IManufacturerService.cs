using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IManufacturerService
{
    Task<AnswerOutcome<Manufacturer>> AddManufacturerToListAsync(Manufacturer manufacturer);
    AnswerOutcome<IEnumerable<Manufacturer>> GetAllManufacturersFromList();
    Task<AnswerOutcome<IEnumerable<Manufacturer>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}
