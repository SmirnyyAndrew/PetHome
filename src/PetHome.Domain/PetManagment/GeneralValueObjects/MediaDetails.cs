using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.PetManagment.GeneralValueObjects;
public record MediaDetails
{
    public IReadOnlyList<Media> Values { get; }
    private MediaDetails() { }
    private MediaDetails(IEnumerable<Media> values)
    {
        Values = values.ToList();
    }

    public static Result<MediaDetails, Error> Create(IEnumerable<Media> values)
    {
        return new MediaDetails(values);
    }
}