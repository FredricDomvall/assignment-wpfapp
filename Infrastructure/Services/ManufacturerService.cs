using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services;
public class ManufacturerService : IManufacturerService
{
    private List<Manufacturer> _manufacturerList = new List<Manufacturer>();
    private readonly IJsonFileRepository<Manufacturer> _jsonFileRepository;
    private readonly string _filePath;
    public ManufacturerService(IJsonFileRepository<Manufacturer> jsonFileRepository, FileSources filePath)
    {
        _jsonFileRepository = jsonFileRepository;
        _filePath = filePath.ManufacturerFileSource;
    }

    public async Task<AnswerOutcome<Manufacturer>> AddManufacturerToListAsync(Manufacturer manufacturer)
    {
        manufacturer.ManufacturerId = GeneratorHelper.GenerateGuidId();
        
        var validationResult = ManufacturerValidationHelper.ManufacturerCreateValidationControl(manufacturer, _manufacturerList);

        if (validationResult.Statement is true)
        {
            _manufacturerList.Add(manufacturer);
            await SaveListToFileAsync();
            return new AnswerOutcome<Manufacturer> { Statement = true, Answer = "Success.", Outcome = manufacturer };
        }
        
        return new AnswerOutcome<Manufacturer> { Statement = false, Answer = validationResult.Answer };     
    }

    public AnswerOutcome<IEnumerable<Manufacturer>> GetAllManufacturersFromList()
    { 
        if (!_manufacturerList.Any())
            return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = false, Answer = "No manufacturers available.", Outcome = _manufacturerList };

        return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = true, Answer = "Success.", Outcome = _manufacturerList };
    }
    public async Task<AnswerOutcome<Manufacturer>> UpdateManufacturerInListByIdAsync(Manufacturer manufacturer)
    {
        var manufacturerToUpdate = _manufacturerList.FirstOrDefault(p => p.ManufacturerId == manufacturer.ManufacturerId);
        if (manufacturerToUpdate == null)
            return new AnswerOutcome<Manufacturer> { Statement = false, Answer = "Manufacturer with the specified ID does not exist." };

        var originalManufacturer = new Manufacturer
        {
            ManufacturerId = manufacturerToUpdate.ManufacturerId,
            ManufacturerName = manufacturerToUpdate.ManufacturerName,
            ManufacturerCountry = manufacturerToUpdate.ManufacturerCountry,
            ManufacturerEmail = manufacturerToUpdate.ManufacturerEmail
        };

        var validationResult = ManufacturerValidationHelper.ManufacturerUpdateValidationControl(manufacturer, _manufacturerList);

        if (validationResult.Statement is true)
        {
            manufacturerToUpdate.ManufacturerName = manufacturer.ManufacturerName!;
            manufacturerToUpdate.ManufacturerCountry = manufacturer.ManufacturerCountry!;
            manufacturerToUpdate.ManufacturerEmail = manufacturer.ManufacturerEmail!;

            await SaveListToFileAsync();
            return new AnswerOutcome<Manufacturer> { Statement = true, Answer = "Successfully updated manufacturer." };
        }

        return new AnswerOutcome<Manufacturer> { Statement = false, Answer = validationResult.Answer, Outcome = originalManufacturer };
        
    }
    public async Task<AnswerOutcome<bool>> DeleteManufacturerFromListByIdAsync(Guid manufacturerId)
    {
        if (!_manufacturerList.Any(m => m.ManufacturerId == manufacturerId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Manufacturer with the specified ID does not exist." };

        _manufacturerList.RemoveAll(m => m.ManufacturerId == manufacturerId);
        await SaveListToFileAsync();
        return new AnswerOutcome<bool> { Statement = true, Answer = "Manufacturer deleted successfully." };
    }
    public async Task<AnswerOutcome<IEnumerable<Manufacturer>>> LoadListFromFileAsync()
    {
        var manufacturersFromFile = await _jsonFileRepository.ReadFromJsonFileAsync(_filePath);

        if (manufacturersFromFile.Outcome == null || manufacturersFromFile.Outcome.Count == 0)
            return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = false, Outcome = manufacturersFromFile.Outcome };

        _manufacturerList = manufacturersFromFile.Outcome;
        return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = true };
    }

    public async Task<AnswerOutcome<bool>> SaveListToFileAsync()
    {
        if (!_manufacturerList.Any())
            return new AnswerOutcome<bool> { Statement = false };

        await _jsonFileRepository.WriteToJsonFileAsync(_filePath, _manufacturerList);
        return new AnswerOutcome<bool> { Statement = true };
    }
}

