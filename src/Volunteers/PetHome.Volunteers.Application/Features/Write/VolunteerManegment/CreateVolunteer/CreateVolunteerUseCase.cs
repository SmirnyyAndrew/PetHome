using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.CreateVolunteer;

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
       [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork, 
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

        var createVolunteerAccountMessage = new CreatedVolunteerAccountEvent(
            command.Email,
            command.UserName,
            command.StartVolunteeringDate,
            command.Requisiteses.ToList(),
            command.Certificates.ToList());
        await _publisher.Publish(createVolunteerAccountMessage);
         
        var transaction = await _unitOfWork.BeginTransaction(ct); 
        var result = await _volunteerRepository.Add(volunteer, ct); 
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        _logger.LogInformation("Волонетёр с id = {0} был создан", volunteer.Id.Value);
        return volunteer.Id.Value;
    }
}
