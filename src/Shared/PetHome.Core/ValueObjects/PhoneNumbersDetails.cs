namespace PetHome.Domain.PetManagment.GeneralValueObjects;

public record PhoneNumbersDetails
{
    public IReadOnlyList<PhoneNumber> Values { get; }

    private PhoneNumbersDetails() { }
    public PhoneNumbersDetails(IReadOnlyList<PhoneNumber> values)
    {
        Values = values;
    }

    public static PhoneNumbersDetails Create(IEnumerable<PhoneNumber> values)
    {
        return new PhoneNumbersDetails(values.ToList());
    }
}