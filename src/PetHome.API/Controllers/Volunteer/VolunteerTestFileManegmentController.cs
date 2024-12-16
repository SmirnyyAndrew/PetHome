using Microsoft.AspNetCore.Mvc;
using Minio;
using PetHome.API.Response;
using PetHome.Infrastructure.Providers.Minio;

namespace PetHome.API.Controllers.Volunteer;

public class VolunteerTestFileManegmentController : ParentController
{
    private readonly MinioProvider _minioProvider;
    public VolunteerTestFileManegmentController(IMinioClient minioClient, ILogger<MinioProvider> logger)
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
        var result = await _minioProvider.UploadFile(stream, bucketName, minioFileName, createBucketIfNotExist, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }


    [HttpPut]
    public async Task<IActionResult> DownloadFile(
        [FromBody] MinioFileName minioFileName,
        [FromQuery] string fileSavePath = "",
        CancellationToken ct = default)
    {
        var result = await _minioProvider.DownloadFile(minioFileName, fileSavePath, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }

    [HttpPut("presigned-path")]
    public async Task<IActionResult> GetFilePresignedPath(
        [FromBody] FileInfoDto fileInfoDto,
        CancellationToken ct = default)
    {
        var result = await _minioProvider.GetFilePresignedPath(fileInfoDto, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteFile(
        [FromBody] FileInfoDto fileInfoDto,
        CancellationToken ct = default)
    {
        var result = await _minioProvider.DeleteFile(fileInfoDto, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(result.Value);
    }
}
