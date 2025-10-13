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
        var nameValidationResult = ValidationHelper.ValidateString(manufacturer.ManufacturerName);
        var countryValidationResult = ValidationHelper.ValidateString(manufacturer.ManufacturerCountry);
        var emailValidationResult = ValidationHelper.ValidateEmail(manufacturer.ManufacturerEmail);
        var guidValidationResult = ValidationHelper.ValidateGuidId<Manufacturer>(manufacturer.ManufacturerId);
        var uniqueValidationResult = ValidationHelper.ValidateManufacturerUnique(manufacturer, _manufacturerList);

        if (nameValidationResult.Statement && countryValidationResult.Statement &&
            emailValidationResult.Statement && guidValidationResult.Statement && uniqueValidationResult.Statement)
        {
            await LoadListFromFileAsync();
            _manufacturerList.Add(manufacturer);
            await SaveListToFileAsync();
            return new AnswerOutcome<Manufacturer> { Statement = true, Answer = "Success.", Outcome = manufacturer };
        }
        else
        {
            string errorMessages = "";
            if (!nameValidationResult.Statement)
                errorMessages += nameValidationResult.Answer + "\n";
            if (!countryValidationResult.Statement)
                errorMessages += countryValidationResult.Answer + "\n";
            if (!emailValidationResult.Statement)
                errorMessages += emailValidationResult.Answer + "\n";
            if (!guidValidationResult.Statement)
                errorMessages += guidValidationResult.Answer + "\n";
            if (!uniqueValidationResult.Statement)
                errorMessages += uniqueValidationResult.Answer + "\n";
            return new AnswerOutcome<Manufacturer> { Statement = false, Answer = errorMessages.Trim() };
        }
    }

    public async Task<AnswerOutcome<IEnumerable<Manufacturer>>> GetAllManufacturersFromListAsync()
    {
        var result = await LoadListFromFileAsync();
        if (!result.Statement || !_manufacturerList.Any())
            return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = false, Answer = "No manufacturers available.", Outcome = _manufacturerList };

        return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = true, Answer = "Success.", Outcome = _manufacturerList };
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

