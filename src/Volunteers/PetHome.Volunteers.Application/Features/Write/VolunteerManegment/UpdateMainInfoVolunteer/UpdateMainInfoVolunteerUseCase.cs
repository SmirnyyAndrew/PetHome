using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Framework.Database;
using PetHome.Species.Contracts.Messaging;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Contracts.Messaging;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCase
    : ICommandHandler<Guid, UpdateMainInfoVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoVolunteerCommand> _validator;
    private readonly IPublishEndpoint _publisher;

    public UpdateMainInfoVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoVolunteerUseCase> logger,
        IPublishEndpoint publisher,
       [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        UpdateMainInfoVolunteerCommand command,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        UpdateMainInfoVolunteerDto updateInfoDto = command.UpdateMainInfoDto;

        var transaction = await _unitOfWork.BeginTransaction(ct);

        var getVolunteerResult = await _volunteerRepository
            .GetById(command.Id, ct);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        Volunteer volunteer = getVolunteerResult.Value; 
        FullName fullName = FullName.Create(
            updateInfoDto.FullNameDto.FirstName,
            updateInfoDto.FullNameDto.LastName).Value;

        Description description = Description.Create(updateInfoDto.Description).Value;

        List<PhoneNumber> phoneNumbers = updateInfoDto.PhoneNumbers
            .Select(p => PhoneNumber.Create(p).Value)
            .ToList();

        Email email = Email.Create(updateInfoDto.Email).Value;

        volunteer.UpdateMainInfo(
            fullName,
            description,
            phoneNumbers,
            email);

        await _volunteerRepository.Update(volunteer, ct); 
        await _unitOfWork.SaveChanges(ct);

        UpdatedMainInfoVolunteerEvent createdSpeciesEvent = new UpdatedMainInfoVolunteerEvent(
            volunteer.Id);
        await _publisher.Publish(createdSpeciesEvent, ct);
        
        transaction.Commit();

        _logger.LogInformation("Обновлена информация волонтёра с id = {0}", command.Id);
        return command.Id;
    }
}
