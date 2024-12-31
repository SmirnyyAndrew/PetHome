using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio;
using PetHome.Core.Controllers;
using PetHome.Core.Response.ErrorManagment.Envelopes;
using PetHome.SharedKernel.Providers.Minio;
using PetHome.Volunteers.API.Controllers.Media.Requests;
namespace PetHome.Volunteers.API.Controllers.Volunteer;

public class VolunteerTestFileManegmentController : ParentController
{
    private readonly MinioProvider _minioProvider;
    public VolunteerTestFileManegmentController(
        IMinioClient minioClient,
        ILogger<MinioProvider> logger)
    {
        _minioProvider = new MinioProvider(minioClient, logger);
    }

    [HttpPost]
    public async Task<IActionResult> UdloadFile(
        IFormFile file,
        [FromQuery] string bucketName = "photos",
        [FromQuery] bool createBucketIfNotExist = false,
        CancellationToken ct = default)
    {
        //Загрузить файл
        await using Stream stream = file.OpenReadStream();

        MinioFileName minioFileName = MinioFileName.Create(file.FileName).Value;
        MinioFileInfoDto minioFileInfoDto = new MinioFileInfoDto(bucketName, minioFileName);
        var result = await _minioProvider.UploadFile(
            stream,
            minioFileInfoDto,
            createBucketIfNotExist,
            ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }


    [HttpPut]
    public async Task<IActionResult> DownloadFile(
        [FromBody] DownloadFilesRequest request,
        [FromQuery] string fileSavePath = "",
        CancellationToken ct = default)
    {
        List<MinioFileName> minioFileNames = request.FilesInfoDto.FileNames
            .Select(f => MinioFileName.Create(f).Value).ToList();
        MinioFilesInfoDto minioFilesInfoDto = new MinioFilesInfoDto(
            request.FilesInfoDto.BucketName,
            minioFileNames);

        var result = await _minioProvider.DownloadFiles(
            minioFilesInfoDto,
            request.FilePathToSave,
            ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }

    [HttpPut("presigned-path")]
    public async Task<IActionResult> GetFilePresignedPath(
        [FromBody] FilesInfoDto filesInfoDto,
        CancellationToken ct = default)
    {
        MinioFilesInfoDto minioFilesInfoDto = new MinioFilesInfoDto(
            filesInfoDto.BucketName,
            filesInfoDto.FileNames.Select(f => MinioFileName.Create(f).Value).ToList());
        var result = await _minioProvider.GetFilePresignedPath(minioFilesInfoDto, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteFile(
        [FromBody] MinioFilesInfoDto fileInfoDto,
        CancellationToken ct = default)
    {
        var result = await _minioProvider.DeleteFile(fileInfoDto, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }
}
