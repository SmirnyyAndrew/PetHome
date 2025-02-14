using FilesService.Core.Models;

namespace FilesService.Core.Request;
public record CompleteMultipartRequest(
   string BucketName,
   string UploadId,
   List<PartETagInfo> Parts);