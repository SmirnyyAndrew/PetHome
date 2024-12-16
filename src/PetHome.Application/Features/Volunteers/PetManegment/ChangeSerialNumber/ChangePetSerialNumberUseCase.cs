using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.ChangeSerialNumber;
public class ChangePetSerialNumberUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<ChangePetSerialNumberUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetSerialNumberUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<ChangePetSerialNumberUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string, Error>> Execute(
        ChangePetSerialNumberCommand request,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, ct);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error;

            Volunteer volunteer = volunteerResult.Value;

            Pet? pet = volunteer.Pets.Where(x => x.Id == request.ChangeNumberDto.PetId).FirstOrDefault();
            if (pet == null)
                return Errors.NotFound($"Питомец {request.ChangeNumberDto.PetId} у волонтёра {request.VolunteerId}");

            Pet.Pets = volunteer.Pets;

            var changeNumberResult = pet.ChangeSerialNumber(request.ChangeNumberDto.NewSerialNumber);
            if (changeNumberResult.IsFailure)
                return changeNumberResult.Error;

            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"У питомца с id {request.ChangeNumberDto.PetId} серийный номер изменён на {request.ChangeNumberDto.NewSerialNumber}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось изменить серийный номер питомца {0}", request.ChangeNumberDto.PetId);
            return Errors.Failure("Database.is.failed");
        }
    }
}
