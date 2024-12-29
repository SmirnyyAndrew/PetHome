using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.HardDelete;
public class HardDeletePetUseCase
    : ICommandHandler<HardDeletePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly IFilesProvider _filesProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetInfoUseCase> _logger;

    public HardDeletePetUseCase(
         IVolunteerRepository volunteerRepository,
         IVolunteerReadDbContext readDBContext,
         IFilesProvider filesProvider,
         IUnitOfWork unitOfWork,
         ILogger<ChangePetInfoUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _readDBContext = readDBContext;
        _filesProvider = filesProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> Execute(
        HardDeletePetCommand command,
        CancellationToken ct)
    {
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

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            volunteer.Pets.Remove(pet);
            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            if (pet.Medias.Count > 0)
            {
                List<MinioFileName> minioFileNames = pet.Medias
                    .Select(f => MinioFileName.Create(f.FileName).Value)
                    .ToList();
                string bucketName = pet.Medias.Select(f => f.BucketName).First();
                MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(bucketName, minioFileNames);
                await _filesProvider.DeleteFile(minioFileInfoDto, ct);
            }
            string message = $"Питомец = {command.PetId} успешно hard deleted!";
            _logger.LogInformation(message);
            return Result.Success<ErrorList>();
        }
        catch (Exception)
        {
            transaction.Rollback();
            string message = $"Не удалось hard delete питомца = {command.PetId}";
            _logger.LogError(message);
            return Errors.Failure(message).ToErrorList();
        }
    }
}
