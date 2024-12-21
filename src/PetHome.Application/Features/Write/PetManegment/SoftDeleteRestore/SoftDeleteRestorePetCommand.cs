namespace PetHome.Application.Features.Write.PetManegment.SoftDelete;
public record SoftDeleteRestorePetCommand(
    Guid VolunteerId, 
    Guid PetId, 
    bool ToDelete);
