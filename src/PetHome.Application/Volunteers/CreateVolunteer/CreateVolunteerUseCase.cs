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
        CreateVolunteerRequestDto dto = request.CreateVolunteerDto;

        VolunteerId id = VolunteerId.Create();

        FullName fullName = FullName.Create(dto.firstName, dto.lastName).Value;

        Email email = Email.Create(dto.email).Value;

        VO_Date startVolunteeringDate = VO_Date.Create(dto.startVolunteeringDate).Value;


        PhoneNumbersDetails phoneNumberDetails = null;
        if (dto.phoneNumberList != null)
        {
            List<PhoneNumber> phoneNumberList = dto.phoneNumberList
                .Select(x => PhoneNumber.Create(x).Value)
                .ToList();
            phoneNumberDetails = PhoneNumbersDetails.Create(phoneNumberList);
        }


        SocialNetworkDetails socialNetworkDetails = null;
        if (dto.socialNetworkList != null)
        {
            List<SocialNetwork> socialNetworkList = dto.socialNetworkList
                .Select(x => SocialNetwork.Create(x).Value)
                .ToList();
            socialNetworkDetails = SocialNetworkDetails.Create(socialNetworkList);
        }


        RequisitesDetails requisitesDetails = null;
        if (dto.requisitesList != null)
        {
            List<Requisites> requisitesList = dto.requisitesList
                 .Select(x => Requisites.Create(x.name, x.desc, x.paymentMethod).Value)
                 .ToList();
            requisitesDetails = RequisitesDetails.Create(requisitesList).Value;
        }


        Volunteer volunteer = Volunteer.Create(
            id,
            fullName,
            email,
            dto.description,
            startVolunteeringDate,
            phoneNumberDetails,
            socialNetworkDetails,
            requisitesDetails)
            .Value;


        var result = await VolunteerRepository.Add(volunteer);

        return volunteer.Id.Value;
    }
}
