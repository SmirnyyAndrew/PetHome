using PetHome.Application.Features.Volunteers.VolunteerDtos;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Dtos;
public record PetMainInfoDto(
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
            DateTime ProfileCreateDate,
            IEnumerable<RequisitesesDto> Requisites);
