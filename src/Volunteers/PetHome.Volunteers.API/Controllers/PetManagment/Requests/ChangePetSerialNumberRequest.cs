using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangeSerialNumber;

public record ChangePetSerialNumberRequest(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto)
{
    public static implicit operator ChangePetSerialNumberCommand(ChangePetSerialNumberRequest request)
    {
        return new ChangePetSerialNumberCommand(request.VolunteerId, request.ChangeNumberDto);
    }
} 