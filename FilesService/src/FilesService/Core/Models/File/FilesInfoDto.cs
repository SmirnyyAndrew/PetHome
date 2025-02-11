namespace FilesService.Core.Models.File;

public record FilesInfoDto(string BucketName, IEnumerable<string> FileNames);