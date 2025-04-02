
namespace PetManagementService.Domain.PetManagment.PetEntity;

public record SerialNumber
{
    public int Value { get; private set; }
    private SerialNumber(int value)
    {
        Value = value;
    }

    public static SerialNumber Create(int value)
    {
        return new SerialNumber(value);
    }
     
    public static implicit operator int(SerialNumber number) => number.Value;
}