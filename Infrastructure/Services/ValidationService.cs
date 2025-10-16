using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.Services;

public class ValidationService
{
    public <AnswerOutcome<string> ProductValidationResults(List<AnswerOutcome<bool>> productserviceListResult )
    {
        string errorMessages = "";

        bool allValid = productserviceListResult.All(r => r.Statement is true);
        if(allValid is not true)
        {
            foreach (var item in productserviceListResult)
                if (item.Statement is false)
                    errorMessages += item.Answer + "\t";
            return new AnswerOutcome<string> { Statement = false, Answer = "One or more Validationcontrols failed.", Outcome = errorMessages };
        }
            
        return new AnswerOutcome<string> { Statement = true, Answer = "All Validationcontrols passed successfully." };
    }
}
