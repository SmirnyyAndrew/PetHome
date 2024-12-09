using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoVolunteerUseCase> _logger;

    public UpdateMainInfoVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoVolunteerUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Execute(
        UpdateMainInfoVolunteerRequest request,
        CancellationToken ct)
    {
        UpdateMainInfoVolunteerDto updateInfoDto = request.UpdateMainInfoDto;

        Volunteer volunteer = _volunteerRepository.GetById(request.Id, ct).Result.Value;

        FullName fullName = FullName.Create(
            updateInfoDto.FullNameDto.FirstName,
            updateInfoDto.FullNameDto.LastName).Value;

        Description description = Description.Create(updateInfoDto.Description).Value;

        IEnumerable<PhoneNumber> phoneNumbers = updateInfoDto.PhoneNumbers
            .ToList()
            .Select(p => PhoneNumber.Create(p).Value);
        PhoneNumbersDetails phoneNumbersDetails = PhoneNumbersDetails.Create(phoneNumbers);

        Email email = Email.Create(updateInfoDto.Email).Value;

        volunteer.UpdateMainInfo(
            fullName,
            description,
            phoneNumbersDetails,
            email);

        await _volunteerRepository.Update(volunteer, ct);

        _logger.LogInformation("Обновлена информация волонтёра с id = {0}", request.Id);

        return request.Id;
    }
}
