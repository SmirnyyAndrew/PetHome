using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;
using PetManagementService.Application.Features.Write.PetManegment.ChangePetInfo;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.HardDelete;
public class HardDeletePetUseCase
    : ICommandHandler<HardDeletePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly IMinioFilesHttpClient _filesProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetInfoUseCase> _logger;

    public HardDeletePetUseCase(
         IVolunteerRepository volunteerRepository,
         IPetManagementReadDbContext readDBContext,
         IMinioFilesHttpClient filesProvider,
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

        var getVolunteerResult = await _volunteerRepository
            .GetById(command.VolunteerId, ct);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        Volunteer volunteer = getVolunteerResult.Value;
        Pet? pet = volunteer.Pets
            .ToList()
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return Errors.NotFound($"Питомец с id = {command.PetId}").ToErrorList();
        }

        var transaction = await _unitOfWork.BeginTransaction(ct);

        volunteer.Pets.Remove(pet);
        await _volunteerRepository.Update(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        if (pet.Photos.Count > 0)
        {
            List<MinioFileName> minioFileNames = pet.Photos
                .Select(f => MinioFileName.Create(f.FileName).Value)
                .ToList();
            string bucketName = pet.Photos.Select(f => f.BucketName).First();
            MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(bucketName, minioFileNames);
            await _filesProvider.DeleteFile(minioFileInfoDto, ct);
        }
        string message = $"Питомец = {command.PetId} успешно hard deleted!";
        _logger.LogInformation(message);
        return Result.Success<ErrorList>();
    }
}
