using AccountService.Application.Database.Repositories;
using AccountService.Domain.Aggregates;
using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Interfaces;
using FilesService.Core.Response;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace AccountService.Application.Features.Write.SetAvatar.CompleteUploadAvatar;
public class CompleteUploadAvatarUseCase
    : ICommandHandler<FileLocationResponse, CompleteUploadAvatarCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IAmazonFilesHttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteUploadAvatarUseCase(
        IAuthenticationRepository repository,
        IAmazonFilesHttpClient httpClient,
        [FromKeyedServices(Constants.Database.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
    }

    public async Task<Result<FileLocationResponse, ErrorList>> Execute(
        CompleteUploadAvatarCommand command,
        CancellationToken ct)
    {
        var getUserResult = await _repository.GetUserById(command.UserId, ct);
        if (getUserResult.IsFailure)
            return getUserResult.Error.ToErrorList();

        User user = getUserResult.Value;

        var completeMultipartUploadResult = await _httpClient.CompleteMultipartUpload(
            command.Key,
            command.CompleteMultipartRequest,
            ct);

        if (completeMultipartUploadResult.IsFailure)
            return Errors.Failure(completeMultipartUploadResult.Error).ToErrorList();

        FileLocationResponse location = completeMultipartUploadResult.Value;
        MediaFile mediaFile = MediaFile.Create(command.CompleteMultipartRequest.BucketName, location.Key, FileType.image).Value;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        user.SetAvatar(mediaFile);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return location;
    }
}
