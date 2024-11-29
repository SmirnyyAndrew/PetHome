using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerUseCase
{
    private readonly IVolunteerRepository VolunteerRepository;

    public CreateVolunteerUseCase(IVolunteerRepository volunteerRepository)
    {
        VolunteerRepository = volunteerRepository;
    }

    public async Task<Result<Guid>> Execute(CreateVolunteerRequest request, CancellationToken ct)
    {
        CreateVolunteerRequestDto dto = request.CreateVolunteerDto;

        FullName fullName = FullName.Create(dto.firstName, dto.lastName);

        Email email = Email.Create(dto.email).Value;

        List<PhoneNumber> phoneNumberList = dto.phoneNumberList
            .Select(x => PhoneNumber.Create(x).Value)
            .ToList();
        PhoneNumbersDetails phoneNumberDetails = PhoneNumbersDetails.Create(phoneNumberList);

        List<SocialNetwork> socialNetworkList = dto.socialNetworkList
            .Select(x => SocialNetwork.Create(x).Value)
            .ToList();
        SocialNetworkDetails socialNetworkDetails = SocialNetworkDetails.Create(socialNetworkList);

        List<Requisites> requisitesList = dto.requisitesList
            .Select(x => Requisites.Create(x.name, x.desc, x.paymentMethod).Value)
            .ToList();
        RequisitesDetails requisitesDetails = RequisitesDetails.Create(requisitesList).Value;


        Volunteer volunteer = Volunteer.Create(
            fullName,
            email,
            dto.description,
            dto.startVolunteeringDate,
            phoneNumberDetails,
            socialNetworkDetails,
            requisitesDetails)
            .Value;


        await VolunteerRepository.Add(volunteer);

        return volunteer.Id.Value;
    }
}
