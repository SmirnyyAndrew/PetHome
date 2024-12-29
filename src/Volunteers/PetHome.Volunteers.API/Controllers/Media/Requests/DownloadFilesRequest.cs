namespace PetHome.API.Controllers.PetManegment.Media;

public record DownloadFilesRequest(FilesInfoDto FilesInfoDto,string FilePathToSave);
