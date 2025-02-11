namespace PetHome.Core.ValueObjects.File;
public record File
{
    public FileId Id { get; }
    public FileType Type { get; }
    public BucketName BucketName { get; }
    public StoragePath StoragePath { get; }
}
