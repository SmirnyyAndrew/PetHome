using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.CreateVolunteer;

public class CreateVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Execute(
        CreateVolunteerCommand request,
        CancellationToken ct)
    {
        VolunteerId id = VolunteerId.Create().Value;

        FullName fullName = FullName.Create(
            request.FullNameDto.FirstName,
            request.FullNameDto.LastName).Value;

        Email email = Email.Create(request.Email).Value;

        Description description = Description.Create(request.Description).Value;

        Date startVolunteeringDate = Date.Create(request.StartVolunteeringDate).Value;
         
        List<PhoneNumber> phoneNumberList = request.PhoneNumbers
                .Select(x => PhoneNumber.Create(x).Value)
                .ToList(); 
         
        List<SocialNetwork> socialNetworkList = request.SocialNetworks
                .Select(x => SocialNetwork.Create(x).Value)
                .ToList(); 
         
        List<Requisites> requisitesList = request.RequisitesesDto
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
            return Errors.Failure("Database.is.failed");
        }
    }
}
