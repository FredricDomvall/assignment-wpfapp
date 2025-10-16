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

    public AnswerOutcome<IEnumerable<Manufacturer>> GetAllManufacturersFromList()
    { 
        if (!_manufacturerList.Any())
            return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = false, Answer = "No manufacturers available.", Outcome = _manufacturerList };

        return new AnswerOutcome<IEnumerable<Manufacturer>> { Statement = true, Answer = "Success.", Outcome = _manufacturerList };
    }
    public async Task<AnswerOutcome<Manufacturer>> UpdateManufacturertInListByIdAsync(Guid manufacturerId, Manufacturer manufacturer)
    {
        var manufacturerToUpdate = _manufacturerList.FirstOrDefault(p => p.ManufacturerId == manufacturerId);
        if (manufacturerToUpdate == null)
            return new AnswerOutcome<Manufacturer> { Statement = false, Answer = "manufacturer with the specified ID does not exist." };

        var nameValidationResult = ValidationHelper.ValidateString(manufacturer.ManufacturerName!);
        var emailValidationResult = ValidationHelper.ValidateEmail(manufacturer.ManufacturerEmail!);
        var countryValidationResult = ValidationHelper.ValidateString(manufacturer.ManufacturerCountry!);

        if (nameValidationResult.Statement is true && emailValidationResult.Statement is true && countryValidationResult.Statement is true)
        {
            manufacturerToUpdate.ManufacturerName = manufacturer.ManufacturerName!;
            manufacturerToUpdate.ManufacturerCountry = manufacturer.ManufacturerCountry!;
            manufacturerToUpdate.ManufacturerEmail = manufacturer.ManufacturerEmail!;

            await SaveListToFileAsync();
            return new AnswerOutcome<Manufacturer> { Statement = true, Answer = "Success.", Outcome = manufacturerToUpdate };
        }
        else
        {
            string errorMessages = "";
            if (nameValidationResult.Statement is false)
                errorMessages += nameValidationResult.Answer + "\n";
            if (emailValidationResult.Statement is false)
                errorMessages += emailValidationResult.Answer + "\n";
            if (countryValidationResult.Statement is false)
                errorMessages += countryValidationResult.Answer + "\n";
            return new AnswerOutcome<Manufacturer> { Statement = false, Answer = errorMessages.Trim() };
        }
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

    public async Task<AnswerOutcome<Manufacturer>> UpdateManufacturerInListByIdAsync(Guid manufacturerId, Manufacturer manufacturerForm)
    {
        var manufacturerToUpdate = _manufacturerList.FirstOrDefault(m => m.ManufacturerId == manufacturerId);
        if (manufacturerToUpdate == null)
            return new AnswerOutcome<Manufacturer> { Statement = false, Answer = "Manufacturer with the specified ID does not exist." };

        var nameValidationResult = ValidationHelper.ValidateString(manufacturerForm.ManufacturerName!);
        var countryValidationResult = ValidationHelper.ValidateString(manufacturerForm.ManufacturerCountry!);
        var emailValidationResult = ValidationHelper.ValidateEmail(manufacturerForm.ManufacturerEmail!);

        if (nameValidationResult.Statement && countryValidationResult.Statement && emailValidationResult.Statement)
        {
            manufacturerToUpdate.ManufacturerName = manufacturerForm.ManufacturerName!;
            manufacturerToUpdate.ManufacturerCountry = manufacturerForm.ManufacturerCountry!;
            manufacturerToUpdate.ManufacturerEmail = manufacturerForm.ManufacturerEmail!;

            await SaveListToFileAsync();
            return new AnswerOutcome<Manufacturer> { Statement = true, Answer = "Success.", Outcome = manufacturerToUpdate };
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
            return new AnswerOutcome<Manufacturer> { Statement = false, Answer = errorMessages.Trim() };
        }
    }
}

