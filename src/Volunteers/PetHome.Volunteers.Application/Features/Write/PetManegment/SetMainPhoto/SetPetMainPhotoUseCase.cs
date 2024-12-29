using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
public class SetPetMainPhotoUseCase
    : ICommandHandler<SetPetMainPhotoCommand>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SetPetMainPhotoUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SetPetMainPhotoCommand> _validator;

    public SetPetMainPhotoUseCase(
        IVolunteerReadDbContext readDBContext,
        IVolunteerRepository volunteerRepository,
        ILogger<SetPetMainPhotoUseCase> logger,
        [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
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
            return validationResult.Errors.ToErrorList();
        }


        VolunteerDto? volunteerDto = _readDBContext.Volunteers
                    .FirstOrDefault(v => v.Id == command.VolunteerId);
        if (volunteerDto == null)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", command.VolunteerId);
            return Errors.NotFound($"Волонтёр с id = {command.VolunteerId}").ToErrorList();
        }


        Volunteer volunteer = _volunteerRepository
            .GetById(command.VolunteerId, ct).Result.Value;
        Pet? pet = volunteer.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return Errors.NotFound($"Питомец с id = {command.PetId}").ToErrorList();
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
            return Errors.Failure(message).ToErrorList();
        }
    }
}
