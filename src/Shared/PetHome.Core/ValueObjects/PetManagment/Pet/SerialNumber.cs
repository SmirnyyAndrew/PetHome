
namespace PetHome.Core.ValueObjects.PetManagment.Pet;

public record SerialNumber
{
    public int Value { get; private set; }
    public SerialNumber(int value)
    {
        Value = value;
    }

    public static SerialNumber Create(int value)
    {
        return new SerialNumber(value);
    }
}