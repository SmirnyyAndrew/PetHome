namespace PetHome.Application.Features.Volunteers.PetManegment.ChangeSerialNumber;
public record ChangePetSerialNumberRequest(
    Guid VolunteerId, 
    ChangePetSerialNumberDto ChangeNumberDto);

public record ChangePetSerialNumberDto(
    Guid PetId, 
    int NewSerialNumber);
