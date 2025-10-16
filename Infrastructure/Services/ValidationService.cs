using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.Services;

public class ValidationService
{
    private List<AnswerOutcome<bool>> _validationResults = new();

    public AnswerOutcome<List<AnswerOutcome<bool>>> ProductValidationResults(List<AnswerOutcome<bool>> productserviceListResult )
    {

        return new AnswerOutcome<List<AnswerOutcome<bool>>> { Statement = true, Answer = "Validation results retrieved.", Outcome = productserviceListResult };
    }
}
