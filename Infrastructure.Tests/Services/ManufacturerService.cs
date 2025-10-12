using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Tests.Services;
public class ManufacturerService : IManufacturerService
{
    public Task<AnswerOutcome<Manufacturer>> AddManufacturerToListAsync(Manufacturer manufacturer)
    {
        throw new NotImplementedException();
    }

    public Task<AnswerOutcome<IEnumerable<Manufacturer>>> GetAllManufacturersFromListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AnswerOutcome<IEnumerable<Manufacturer>>> LoadListFromFileAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AnswerOutcome<bool>> SaveListToFileAsync()
    {
        throw new NotImplementedException();
    }
}
