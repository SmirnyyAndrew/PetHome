namespace FilesService.Core.Models.File;

public record MinioFilesInfoDto(string BucketName, IEnumerable<MinioFileName> FileNames);