using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.Responses.ErrorManagement.Envelopes;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace PetHome.Core.Application.Validation;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails == null)
            throw new InvalidOperationException();


        List<Error> errors = new();
        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            foreach (var error in validationErrors)
            {
                errors.Add(Errors.Validation(error));
            }
        }

        ResponseEnvelope envelope = ResponseEnvelope.Error(errors.ToArray());

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}
