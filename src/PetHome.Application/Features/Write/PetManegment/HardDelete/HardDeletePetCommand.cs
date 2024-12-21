namespace PetHome.Application.Features.Write.PetManegment.HardDelete;
public record HardDeletePetCommand(
    Guid VolunteerId,
    Guid PetId);
