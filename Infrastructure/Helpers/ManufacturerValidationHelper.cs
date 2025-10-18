using Infrastructure.Models;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers;

public static class ManufacturerValidationHelper
{
    public static AnswerOutcome<bool> ValidateManufacturerGuidId(Guid manufacturerId, List<Manufacturer> manufacturerList)
    {
        if (manufacturerId == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tId was not set properly." };

        if (manufacturerList.Any(m => m.ManufacturerId == manufacturerId))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tId already exists in the list." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for id passed successfully"};
    }
    public static AnswerOutcome<bool> ValidateManufacturerName(string manufacturerName)
    {
        if (string.IsNullOrWhiteSpace(manufacturerName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Name can not be left empty" };

        if (manufacturerName.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Names be at least 3 characters long." };

        if (manufacturerName.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Names can not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for name passed successfully" };
    }
    public static AnswerOutcome<bool> ValidateManufacturerEmail(string manufacturerEmail)
    {
        if (string.IsNullOrWhiteSpace(manufacturerEmail))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Email cannot be empty." };

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(manufacturerEmail, pattern))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Email format is invalid." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for email passed successfully" };
    }
    public static AnswerOutcome<bool> ValidateManufacturerCountry(string manufacturerCountry)
    {
        if (string.IsNullOrWhiteSpace(manufacturerCountry))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Country can not be left empty" };

        if (manufacturerCountry.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "Country be at least 3 characters long." };

        if (manufacturerCountry.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Country can not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for country passed successfully" };
    }
    public static AnswerOutcome<bool> ValidateManufacturerUnique(Manufacturer checkManufacturer, List<Manufacturer> manufacturerList)
    {
        var otherManufacturers = manufacturerList.Where(m => m.ManufacturerId != checkManufacturer.ManufacturerId);

        if (otherManufacturers.Any(m => m.ManufacturerName == checkManufacturer.ManufacturerName))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Manufacturer name must be unique." };

        if (otherManufacturers.Any(m => m.ManufacturerEmail == checkManufacturer.ManufacturerEmail))
            return new AnswerOutcome<bool> { Statement = false, Answer = "Manufacturer email must be unique." };

        return new AnswerOutcome<bool> { Statement = true, Answer = "All validation for unique passed succesfully"};
    }
    public static AnswerOutcome<bool> ManufacturerCreateValidationControl(Manufacturer checkManufacturer, List<Manufacturer> manufacturerList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();
        validationResults.Add(ValidateManufacturerGuidId(checkManufacturer.ManufacturerId, manufacturerList));
        validationResults.Add(ValidateManufacturerName(checkManufacturer.ManufacturerName!));
        validationResults.Add(ValidateManufacturerEmail(checkManufacturer.ManufacturerEmail));
        validationResults.Add(ValidateManufacturerCountry(checkManufacturer.ManufacturerCountry!));
        validationResults.Add(ValidateManufacturerUnique(checkManufacturer, manufacturerList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All Validation for create passed successfully." };

        else return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public static AnswerOutcome<bool> ManufacturerUpdateValidationControl(Manufacturer checkManufacturer, List<Manufacturer> manufacturerList)
    {
        List<AnswerOutcome<bool>> validationResults = new List<AnswerOutcome<bool>>();
        validationResults.Add(ValidateManufacturerName(checkManufacturer.ManufacturerName!));
        validationResults.Add(ValidateManufacturerEmail(checkManufacturer.ManufacturerEmail));
        validationResults.Add(ValidateManufacturerCountry(checkManufacturer.ManufacturerCountry!));
        validationResults.Add(ValidateManufacturerUnique(checkManufacturer, manufacturerList));

        var finalValidationResult = ValidateAllValidationResults(validationResults);
        if (finalValidationResult.Statement is true)
            return new AnswerOutcome<bool> { Statement = true, Answer = "All Validation for update passed successfully." };

        else return new AnswerOutcome<bool> { Statement = false, Answer = finalValidationResult.Answer };
    }
    public static AnswerOutcome<bool> ValidateAllValidationResults(List<AnswerOutcome<bool>> manufacturerServiceListResult)
    {
        string errorMessages = "";

        bool allValid = manufacturerServiceListResult.All(r => r.Statement is true);
        if (allValid is not true)
        {
            foreach (var item in manufacturerServiceListResult)
                if (item.Statement is false)
                    errorMessages += item.Answer + "\n";

            return new AnswerOutcome<bool> { Statement = false, Answer = errorMessages };
        }

        return new AnswerOutcome<bool> { Statement = true, Answer = "All Validation controls passed successfully." };
    }
}
