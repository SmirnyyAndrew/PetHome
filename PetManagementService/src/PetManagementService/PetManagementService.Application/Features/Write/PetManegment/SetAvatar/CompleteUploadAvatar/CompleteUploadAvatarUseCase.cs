using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetManagementService.Application.Database;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.SetAvatar.CompleteUploadAvatar;
public class CompleteUploadAvatarUseCase
    : ICommandHandler<FileLocationResponse, CompleteUploadAvatarCommand>
{ 
    private readonly IVolunteerRepository _repository;
    private readonly IPetManagementReadDbContext _readDbContext;
    private readonly IAmazonFilesHttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteUploadAvatarUseCase(
        IVolunteerRepository repository,
        IPetManagementReadDbContext _readDbContext,
        IAmazonFilesHttpClient httpClient,
         IUnitOfWork unitOfWork)
    { 
        _repository = repository;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
    }

    public async Task<Result<FileLocationResponse, ErrorList>> Execute(
        CompleteUploadAvatarCommand command,
        CancellationToken ct)
    {
        var getUserResult = await _repository.GetById(command.VolunteerId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        Volunteer volunteer = getUserResult.Value;

        Pet? getPetResult = volunteer.Pets.FirstOrDefault(p => p.Id  == command.PetId);
        if (getPetResult is null)
            return getUserResult.Error.ToErrorList(); 

        var completeMultipartUploadResult = await _httpClient.CompleteMultipartUpload(
            command.Key,
            command.CompleteMultipartRequest,
            ct);

        if (completeMultipartUploadResult.IsFailure)
            return Errors.Failure(completeMultipartUploadResult.Error).ToErrorList();

        FileLocationResponse location = completeMultipartUploadResult.Value; 
        MediaFile mediaFile = MediaFile.Create(command.CompleteMultipartRequest.BucketName, location.Key, FileType.image).Value;
        
        var transaction = await _unitOfWork.BeginTransaction(ct);
        getPetResult.SetAvatar(mediaFile);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();
         
        return location;
    }
}
