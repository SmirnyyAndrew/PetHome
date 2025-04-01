using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetManagementService.Application.Database;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.AmazonUploadPetMediaFiles.CompleteUploadAvatar;
public class CompleteUploadAvatarUseCase
    : ICommandHandler<IEnumerable<MediaFile>, CompleteUploadAvatarCommand>
{
    private readonly IVolunteerRepository _repository; 
    private readonly IAmazonFilesHttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteUploadAvatarUseCase(
        IVolunteerRepository repository, 
        IAmazonFilesHttpClient httpClient,
         IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<MediaFile>, ErrorList>> Execute(
        CompleteUploadAvatarCommand command,
        CancellationToken ct)
    {
        var getUserResult = await _repository.GetById(command.VolunteerId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        Volunteer volunteer = getUserResult.Value;

        Pet? getPetResult = volunteer.Pets.FirstOrDefault(p => p.Id == command.PetId);
        if (getPetResult is null)
            return getUserResult.Error.ToErrorList();

        List<MediaFile> addedMediaFiles = new List<MediaFile>();
        var keys = command.Keys.ToList();
        var completeRequests = command.CompleteMultipartRequests.ToList();
        for (int i = 0; i < command.CompleteMultipartRequests.Count(); i++)
        {
            var completeMultipartUploadResult = await _httpClient.CompleteMultipartUpload(
                keys[i],
                completeRequests[i],
                ct);

            if (completeMultipartUploadResult.IsSuccess)
            { 
                MediaFile mediaFile = MediaFile.Create(completeRequests[i].BucketName, keys[i], FileType.image).Value;
                addedMediaFiles.Add(mediaFile);
            }
        }
         
        var transaction = await _unitOfWork.BeginTransaction(ct);
        getPetResult.UploadMedia(addedMediaFiles);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return addedMediaFiles;
    }
}
