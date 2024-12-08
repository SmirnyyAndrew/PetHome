using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Features.Volunteers.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.CreateVolunteer;

public class CreateVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerUseCase> _logger;

    public CreateVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Guid> Execute(
        CreateVolunteerRequest request,
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
        PhoneNumbersDetails phoneNumberDetails = PhoneNumbersDetails.Create(phoneNumberList);


        List<SocialNetwork> socialNetworkList = request.SocialNetworks
                .Select(x => SocialNetwork.Create(x).Value)
                .ToList();
        SocialNetworkDetails socialNetworkDetails = SocialNetworkDetails.Create(socialNetworkList);


        List<Requisites> requisitesList = request.RequisitesesDto
                 .Select(x => Requisites.Create(x.Name, x.Desc, x.PaymentMethod).Value)
                 .ToList();
        RequisitesDetails requisitesDetails = RequisitesDetails.Create(requisitesList).Value;


        Volunteer volunteer = Volunteer.Create(
            id,
            fullName,
            email,
            description,
            startVolunteeringDate,
            phoneNumberDetails,
            socialNetworkDetails,
            requisitesDetails)
            .Value;

        var result = await _volunteerRepository.Add(volunteer, ct);

        _logger.LogInformation("Волонетёр с id={0} был создан", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}
