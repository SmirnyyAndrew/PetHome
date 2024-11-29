namespace PetHome.Domain.GeneralValueObjects;

public record PhoneNumbersDetails
{
    public IReadOnlyList<PhoneNumber> Values { get; }

    private PhoneNumbersDetails() { }
    public PhoneNumbersDetails(IReadOnlyList<PhoneNumber> values)
    {
        Values = values;
    }

    public static PhoneNumbersDetails Create(List<PhoneNumber> values)
    {
        return new PhoneNumbersDetails(values);
    }
}