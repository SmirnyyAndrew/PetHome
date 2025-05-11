
namespace PetManagementService.Application.Database.Dto;
public class VolunteerDto
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Email { get; private set; }
    public string Description { get; private set; }
    public DateTime StartVolunteeringDate { get; private set; } 
}
