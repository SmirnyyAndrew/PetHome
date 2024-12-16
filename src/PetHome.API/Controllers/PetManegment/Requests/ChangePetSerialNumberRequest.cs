using PetHome.Application.Features.Volunteers.PetManegment.ChangeSerialNumber;

public record ChangePetSerialNumberRequest(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto)
{
    public static implicit operator ChangePetSerialNumberCommand(ChangePetSerialNumberRequest request)
    {
        return new ChangePetSerialNumberCommand(request.VolunteerId, request.ChangeNumberDto);
    }
} 