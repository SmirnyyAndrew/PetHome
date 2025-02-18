namespace FilesService.Core.Dto.File;

public record MinioFilesInfoDto(string BucketName, IEnumerable<MinioFileName> FileNames);