namespace FilesService.Core.Dto.File;

public record FilesInfoDto(string BucketName, IEnumerable<string> FileNames);