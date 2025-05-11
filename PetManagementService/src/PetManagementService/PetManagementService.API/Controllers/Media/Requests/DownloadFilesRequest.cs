using FilesService.Core.Dto.File;

namespace PetManagementService.API.Controllers.Media.Requests;

public record DownloadFilesRequest(FilesInfoDto FilesInfoDto,string FilePathToSave);
