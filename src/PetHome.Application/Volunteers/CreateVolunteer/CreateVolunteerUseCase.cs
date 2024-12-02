using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.Shared.Error;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerUseCase
{
    private readonly IVolunteerRepository VolunteerRepository;

    public CreateVolunteerUseCase(IVolunteerRepository volunteerRepository)
    {
        VolunteerRepository = volunteerRepository;
    }

    public async Task<Result<Guid, Error>> Execute(CreateVolunteerRequest request, CancellationToken ct)
    {  
        VolunteerId id = VolunteerId.Create();

        FullName fullName = FullName.Create(request.firstName, request.lastName).Value;

        Email email = Email.Create(request.email).Value;

        Date startVolunteeringDate = Date.Create(request.startVolunteeringDate).Value;


        List<PhoneNumber> phoneNumberList = request.phoneNumbers
                .Select(x => PhoneNumber.Create(x).Value)
                .ToList();
        PhoneNumbersDetails phoneNumberDetails = PhoneNumbersDetails.Create(phoneNumberList);


        List<SocialNetwork> socialNetworkList = request.socialNetworks
                .Select(x => SocialNetwork.Create(x).Value)
                .ToList();
        SocialNetworkDetails socialNetworkDetails = SocialNetworkDetails.Create(socialNetworkList);


        List<Requisites> requisitesList = request.requisitesesDto
                 .Select(x => Requisites.Create(x.name, x.desc, x.paymentMethod).Value)
                 .ToList();
        RequisitesDetails requisitesDetails = RequisitesDetails.Create(requisitesList).Value;
          

        Volunteer volunteer = Volunteer.Create(
            id,
            fullName,
            email,
            request.description,
            startVolunteeringDate,
            phoneNumberDetails,
            socialNetworkDetails,
            requisitesDetails)
            .Value;

        var result = await VolunteerRepository.Add(volunteer);

        return volunteer.Id.Value;
    }
}
