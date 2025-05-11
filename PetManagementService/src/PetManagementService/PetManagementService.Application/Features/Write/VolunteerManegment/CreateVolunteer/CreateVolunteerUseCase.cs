using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetManagementService.Application.Database;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.CreateVolunteer;

public class CreateVolunteerUseCase
    : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerUseCase> logger,
        IPublishEndpoint publisher,
        IUnitOfWork unitOfWork,
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        CreateVolunteerCommand command,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        VolunteerId id = VolunteerId.Create().Value;

        FullName fullName = FullName.Create(
            command.FullNameDto.FirstName,
            command.FullNameDto.LastName).Value;

        Email email = Email.Create(command.Email).Value;

        Description description = Description.Create(command.Description).Value;

        Date startVolunteeringDate = Date.Create(command.StartVolunteeringDate).Value;

        List<PhoneNumber> phoneNumberList = command.PhoneNumbers
                .Select(x => PhoneNumber.Create(x).Value)
                .ToList();

        List<SocialNetwork> socialNetworkList = command.SocialNetworks
                .Select(x => SocialNetwork.Create(x.url).Value)
                .ToList();

        List<Requisites> requisitesList = command.Requisiteses
                 .Select(x => Requisites.Create(x.Name, x.Desc, x.PaymentMethod).Value)
                 .ToList();

        Volunteer volunteer = Volunteer.Create(
            id,
            fullName,
            email,
            description,
            startVolunteeringDate,
            phoneNumberList,
            requisitesList,
            socialNetworkList)
            .Value;

        //TODO: применить event из нового shared
        //CreatedVolunteerAccountEvent createVolunteerAccountMessage = new CreatedVolunteerAccountEvent(
        //    id,
        //    email,
        //    command.UserName,
        //    command.StartVolunteeringDate,
        //    command.Requisiteses.ToList(),
        //    command.Certificates.ToList());
        //await _publisher.Publish(createVolunteerAccountMessage);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        var result = await _volunteerRepository.Add(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        _logger.LogInformation("Волонетёр с id = {0} был создан", volunteer.Id.Value);
        return volunteer.Id.Value;
    }
}
