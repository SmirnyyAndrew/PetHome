namespace PetHome.Application.Features.Write.PetManegment.HardDelete;
public record HardDeleteCommand(
    Guid VolunteerId,
    Guid PetId);
