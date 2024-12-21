namespace PetHome.Application.Features.Write.PetManegment.SoftDelete;
public record SoftDeleteRestoreCommand(
    Guid VolunteerId, 
    Guid PetId, 
    bool ToDelete);
