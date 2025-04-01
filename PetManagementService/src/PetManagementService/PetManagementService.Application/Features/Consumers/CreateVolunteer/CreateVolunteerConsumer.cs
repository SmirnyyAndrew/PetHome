using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Framework.Database;
using PetManagementService.Application.Database;
using PetManagementService.Application.Features.Write.VolunteerManegment.CreateVolunteer;
using PetManagementService.Contracts.Messaging.Volunteer;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Consumers.CreateVolunteer;
public class CreateVolunteerConsumer : IConsumer<CreatedVolunteerEvent>
{

    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatedVolunteerEvent> _validator;
    private readonly IPublishEndpoint _publisher;
    public CreateVolunteerConsumer(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerUseCase> logger,
        IPublishEndpoint publisher,
        IUnitOfWork unitOfWork,
        IValidator<CreatedVolunteerEvent> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
    }


    public async Task Consume(ConsumeContext<CreatedVolunteerEvent> context)
    {
        var command = context.Message;

        var validationResult = await _validator.ValidateAsync(command, CancellationToken.None);
        if (validationResult.IsValid is false)
        {
            _logger.LogError("Данные невалидны");
            return;
        }

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
                .Select(x => SocialNetwork.Create(x).Value)
                .ToList();

        List<Requisites> requisitesList = command.RequisitesesDto
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
         
        var createVolunteerMessage = new CreatedVolunteerEvent(
            command.FullNameDto,
            email,
            description,
            startVolunteeringDate,
            command.PhoneNumbers,
            command.SocialNetworks,
            command.RequisitesesDto);
        await _publisher.Publish(createVolunteerMessage);

        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None); 
        var result = await _volunteerRepository.Add(volunteer, CancellationToken.None); 
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        _logger.LogInformation("Волонетёр с id = {0} был создан", volunteer.Id.Value);
        return;
    }
}
