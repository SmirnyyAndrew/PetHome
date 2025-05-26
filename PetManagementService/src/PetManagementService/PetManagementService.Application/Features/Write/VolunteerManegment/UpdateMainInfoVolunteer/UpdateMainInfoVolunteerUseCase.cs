using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetManagementService.Application.Database;
using PetManagementService.Contracts.Messaging.Volunteer;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCase
    : ICommandHandler<Guid, UpdateMainInfoVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoVolunteerCommand> _validator;
    private readonly IHostEnvironment _env;
    private readonly IPublishEndpoint _publisher;

    public UpdateMainInfoVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoVolunteerUseCase> logger,
        IPublishEndpoint publisher,
        IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoVolunteerCommand> validator,
        IHostEnvironment env)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _env = env;
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

        if (!_env.IsTestEnvironment())
        {
            UpdatedMainInfoVolunteerEvent createdSpeciesEvent = new UpdatedMainInfoVolunteerEvent(
            volunteer.Id);
            await _publisher.Publish(createdSpeciesEvent, ct);
        }

        transaction.Commit();

        _logger.LogInformation("Обновлена информация волонтёра с id = {0}", command.Id);
        return command.Id;
    }
}
