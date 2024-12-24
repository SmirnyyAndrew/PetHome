using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Database.Read;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.SetMainPhoto;
public class SetPetMainPhotoUseCase
    : ICommandHandler<SetPetMainPhotoCommand>
{
    private readonly IReadDBContext _readDBContext;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SetPetMainPhotoUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SetPetMainPhotoCommand> _validator;

    public SetPetMainPhotoUseCase(
        IReadDBContext readDBContext,
        IVolunteerRepository volunteerRepository,
        ILogger<SetPetMainPhotoUseCase> logger,
        IUnitOfWork unitOfWork,
        IValidator<SetPetMainPhotoCommand> validator)
    {
        _readDBContext = readDBContext;
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        SetPetMainPhotoCommand command,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is false)
        {
            return (ErrorList)validationResult.Errors;
        }


        VolunteerDto? volunteerDto = _readDBContext.Volunteers
                    .FirstOrDefault(v => v.Id == command.VolunteerId);
        if (volunteerDto == null)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", command.VolunteerId);
            return (ErrorList)Errors.NotFound($"Волонтёр с id = {command.VolunteerId}");
        }


        Volunteer volunteer = _volunteerRepository
            .GetById(command.VolunteerId, ct).Result.Value;
        Pet? pet = volunteer.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return (ErrorList)Errors.NotFound($"Питомец с id = {command.PetId}");
        }

        Media media = Media.Create(command.BucketName, command.FileName).Value;
        pet.SetMainPhoto(media);
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Главная фотография питомца = {command.PetId} успешно изменена";
            _logger.LogInformation(message);
            return Result.Success<ErrorList>();
        }
        catch (Exception)
        {
            transaction.Rollback();
            string message = $"Не удалось изменить главную фотографию питомца = {command.PetId}";
            _logger.LogError(message);
            return (ErrorList)Errors.Failure(message);
        }
    }
}
