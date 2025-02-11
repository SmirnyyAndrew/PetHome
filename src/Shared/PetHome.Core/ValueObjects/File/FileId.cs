using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.File;
public record FileId
{
    public Guid Value { get; }

    private FileId() { }
    private FileId(Guid value)
    {
        Value = value;
    }

    public static Result<FileId, Error> Create() => new FileId(Guid.NewGuid());
}
