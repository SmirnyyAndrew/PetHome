namespace PetHome.Accounts.Application.Database.Dto;
public record UserDto(
    Guid Id, 
    string UserName, 
    string Email, 
    string RoleName,
    DateTime DateTime);