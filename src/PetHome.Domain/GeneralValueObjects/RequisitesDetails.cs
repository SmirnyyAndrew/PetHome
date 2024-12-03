using CSharpFunctionalExtensions;

namespace PetHome.Domain.GeneralValueObjects;

public record RequisitesDetails 
{
    public IReadOnlyList<Requisites> Values { get; }
     
    private RequisitesDetails() { }
    public RequisitesDetails(IEnumerable<Requisites> values)
    {
        Values = values.ToList();
    }

    public static Result<RequisitesDetails> Create(IEnumerable<Requisites> values)
    {
        return new RequisitesDetails(values.ToList());
    }
}