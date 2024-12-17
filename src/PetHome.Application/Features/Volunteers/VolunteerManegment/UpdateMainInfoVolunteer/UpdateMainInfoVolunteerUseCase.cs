using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMainInfoVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoVolunteerUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Execute(
        UpdateMainInfoVolunteerCommand request,
        CancellationToken ct)
    {
        UpdateMainInfoVolunteerDto updateInfoDto = request.UpdateMainInfoDto;


        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            Volunteer volunteer = _volunteerRepository.GetById(request.Id, ct).Result.Value;

            FullName fullName = FullName.Create(
                updateInfoDto.FullNameDto.FirstName,
                updateInfoDto.FullNameDto.LastName).Value;

            Description description = Description.Create(updateInfoDto.Description).Value;

            List<PhoneNumber> phoneNumbers = updateInfoDto.PhoneNumbers
                .Select(p => PhoneNumber.Create(p).Value)
                .ToList(); 

            Email email = Email.Create(updateInfoDto.Email).Value;

            volunteer.UpdateMainInfo(
                fullName,
                description,
                phoneNumbers,
                email);

            await _volunteerRepository.Update(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Обновлена информация волонтёра с id = {0}", request.Id);
            return request.Id;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось обнавить информацию волонтёра с id = {0}", request.Id);
            return Errors.Failure("Database.is.failed");
        }
    }
}
