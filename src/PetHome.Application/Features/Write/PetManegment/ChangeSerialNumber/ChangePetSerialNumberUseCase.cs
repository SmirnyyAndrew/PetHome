using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
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

    public async Task<Result<string, ErrorList>> Execute(
        ChangePetSerialNumberCommand command,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, ct);
            if (volunteerResult.IsFailure)
                return (ErrorList)volunteerResult.Error;

            Volunteer volunteer = volunteerResult.Value;

            Pet? pet = volunteer.Pets.Where(x => x.Id == command.ChangeNumberDto.PetId).FirstOrDefault();
            if (pet == null)
                return (ErrorList)Errors.NotFound($"Питомец {command.ChangeNumberDto.PetId} у волонтёра {command.VolunteerId}");

            Pet.Pets = volunteer.Pets;

            var changeNumberResult = pet.ChangeSerialNumber(command.ChangeNumberDto.NewSerialNumber);
            if (changeNumberResult.IsFailure)
                return (ErrorList)changeNumberResult.Error;

            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"У питомца с id {command.ChangeNumberDto.PetId} серийный номер изменён на {command.ChangeNumberDto.NewSerialNumber}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось изменить серийный номер питомца {0}", command.ChangeNumberDto.PetId);
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
