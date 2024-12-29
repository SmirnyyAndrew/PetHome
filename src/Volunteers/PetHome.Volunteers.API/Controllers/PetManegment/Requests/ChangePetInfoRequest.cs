using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Volunteers.API.Controllers.PetManegment.Requests;
public record ChangePetInfoRequest(
        Guid PetId,
        string Name,
        Guid SpeciesId,
        string Description,
        Guid BreedId,
        string Color,
        Guid ShelterId,
        double Weight,
        bool IsCastrated,
        DateTime BirthDate,
        bool IsVaccinated,
        PetStatusEnum Status,
        Guid VolunteerId,
        IReadOnlyList<RequisitesesDto> Requisiteses)
{
    public static implicit operator ChangePetInfoCommand(ChangePetInfoRequest request)
    {
        return new ChangePetInfoCommand(
            request.PetId,
            request.Name,
            request.SpeciesId,
            request.Description,
            request.BreedId,
            request.Color,
            request.ShelterId,
            request.Weight,
            request.IsCastrated,
            request.BirthDate,
            request.IsVaccinated,
            request.Status,
            request.VolunteerId,
            request.Requisiteses);
    }
}
