using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using PetHome.API.Extentions;
using PetHome.API.Response;
using PetHome.Application.Features.Dtos;
using PetHome.Application.Features.Volunteers.CreatePetVolunteer;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Controllers.Volunteer;

public class VolunteerPetManegmentController : ParentController
{
    [HttpPost("{volunteerId:guid}/pets")]
    public async Task<IActionResult> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromBody] PetInfoDto petInfoDto,
        IFormFile file,
        [FromServices] VolunteerCreatePetUseCase createPetUseCase,
        [FromServices] IValidator<VolunteerCreatePetRequest> validator,
        CancellationToken ct = default)
    {
        VolunteerCreatePetRequest createPetRequest = new VolunteerCreatePetRequest(
                        volunteerId,
                        petInfoDto.PetMainInfoDto,
                        petInfoDto.PhotoDetailsDto);

        var validationResult = await validator.ValidateAsync(createPetRequest, ct);
        if (validationResult.IsValid == false)
            return BadRequest(ResponseEnvelope.Error(validationResult.Errors));


        var result = await createPetUseCase.Execute(createPetRequest, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result);
    }   
}
