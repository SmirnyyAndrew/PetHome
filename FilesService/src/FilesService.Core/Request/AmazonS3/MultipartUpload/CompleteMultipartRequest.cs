using FilesService.Core.Models;

namespace FilesService.Core.Request.AmazonS3.MultipartUpload;
public record CompleteMultipartRequest(
   string BucketName,
   string UploadId,
   List<PartETagInfo> Parts);