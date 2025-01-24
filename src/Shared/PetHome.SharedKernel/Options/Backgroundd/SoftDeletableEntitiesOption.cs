namespace PetHome.SharedKernel.Options.Backgroundd;
public class SoftDeletableEntitiesOption
{
    public const string SECTION_NAME = "Background";

    public int DaysToHardDelete { get; init; }
    public int HoursToCheck { get; init; }
}
