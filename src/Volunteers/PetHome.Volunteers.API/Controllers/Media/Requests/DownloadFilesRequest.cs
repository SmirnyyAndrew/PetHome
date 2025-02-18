using FilesService.Core.Dto.File;

namespace PetHome.Volunteers.API.Controllers.Media.Requests;

public record DownloadFilesRequest(FilesInfoDto FilesInfoDto,string FilePathToSave);
