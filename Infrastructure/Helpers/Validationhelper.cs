using Infrastructure.Models;

namespace Infrastructure.Helpers;
public static class Validationhelper
{
    public static AnswerOutcome<bool> ValidateString(string stringInputs, List<Product> productList)
    {
        if (string.IsNullOrWhiteSpace(stringInputs))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tText-inputs can not be left empty" };
        if (stringInputs.Length < 3)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tText-inputs be at least 3 characters long." };
        if (stringInputs.Any(char.IsDigit))
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tText-inputs not contain numbers." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateDecimalPrice(string decimalPrice)
    {
        var result = decimal.TryParse(decimalPrice, out decimal parsedPrice);
        if (!result)
        {
            if (string.IsNullOrWhiteSpace(decimalPrice))
                return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice can not be left empty." };
            if (decimalPrice is null)
                return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice can not be null." };

            return new AnswerOutcome<bool> { Statement = false, Answer = "\tprice must be a decimal value." };
        }
        else if (parsedPrice <= 0)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tProduct price must be greater than zero." };

        return new AnswerOutcome<bool> { Statement = true };
    }
    public static AnswerOutcome<bool> ValidateGuidId<T>(Guid Id)
    {
        if (Id == Guid.Empty)
            return new AnswerOutcome<bool> { Statement = false, Answer = "\tProduct Id was not set properly." };

        return new AnswerOutcome<bool> { Statement = true };
    }
}
