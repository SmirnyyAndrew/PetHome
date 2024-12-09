using Microsoft.AspNetCore.Http;
using PetHome.Application.Features.Dtos;
using PetHome.Application.Features.Volunteers.VolunteerDtos;

namespace PetHome.Application.Features.Volunteers.CreatePetVolunteer;
public record VolunteerCreatePetRequest(Guid VolunteerId, PetMainInfoDto PetMainInfoDto);