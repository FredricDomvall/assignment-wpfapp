using Infrastructure.Models;

namespace Infrastructure.Interfaces;
public interface IManufacturerService
{
    Task<AnswerOutcome<Manufacturer>> AddManufacturerToListAsync(Manufacturer manufacturer);
    AnswerOutcome<IEnumerable<Manufacturer>> GetAllManufacturersFromList();
    Task<AnswerOutcome<Manufacturer>> UpdateManufacturerInListByIdAsync(Guid manufacturerId, Manufacturer manufacturer);
    Task<AnswerOutcome<bool>> DeleteManufacturerFromListByIdAsync(Guid manufacturerId);
    Task<AnswerOutcome<IEnumerable<Manufacturer>>> LoadListFromFileAsync();
    Task<AnswerOutcome<bool>> SaveListToFileAsync();
}
