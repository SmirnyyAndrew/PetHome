using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.Volunteers.PetManegment.ChangeSerialNumber;

public record ChangePetSerialNumberRequest(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto)
{
    public static implicit operator ChangePetSerialNumberCommand(ChangePetSerialNumberRequest request)
    {
        return new ChangePetSerialNumberCommand(request.VolunteerId, request.ChangeNumberDto);
    }
} 