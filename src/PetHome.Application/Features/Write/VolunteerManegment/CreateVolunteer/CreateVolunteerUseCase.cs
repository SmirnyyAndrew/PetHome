using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.VolunteerManegment.CreateVolunteer;

public class CreateVolunteerUseCase
    : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerUseCase> logger,
        IUnitOfWork unitOfWork,
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        CreateVolunteerCommand createVolunteerCommand,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(createVolunteerCommand, ct);
        if (validationResult.IsValid is false)
            return (ErrorList)validationResult.Errors;

        VolunteerId id = VolunteerId.Create().Value;

        FullName fullName = FullName.Create(
            createVolunteerCommand.FullNameDto.FirstName,
            createVolunteerCommand.FullNameDto.LastName).Value;

        Email email = Email.Create(createVolunteerCommand.Email).Value;

        Description description = Description.Create(createVolunteerCommand.Description).Value;

        Date startVolunteeringDate = Date.Create(createVolunteerCommand.StartVolunteeringDate).Value;

        List<PhoneNumber> phoneNumberList = createVolunteerCommand.PhoneNumbers
                .Select(x => PhoneNumber.Create(x).Value)
                .ToList();

        List<SocialNetwork> socialNetworkList = createVolunteerCommand.SocialNetworks
                .Select(x => SocialNetwork.Create(x).Value)
                .ToList();

        List<Requisites> requisitesList = createVolunteerCommand.RequisitesesDto
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

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var result = await _volunteerRepository.Add(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Волонетёр с id = {0} был создан", volunteer.Id.Value);
            return volunteer.Id.Value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось создать волонтёра с id = {0}", volunteer.Id.Value);
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
