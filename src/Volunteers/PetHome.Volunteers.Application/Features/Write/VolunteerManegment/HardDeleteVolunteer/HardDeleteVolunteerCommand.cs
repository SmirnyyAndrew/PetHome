using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public record HardDeleteVolunteerCommand(VolunteerId VolunteerId) : ICommand;
