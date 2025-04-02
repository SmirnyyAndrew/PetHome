using PetHome.SharedKernel.Enums;

namespace PetHome.SharedKernel.ValueObjects.User;
public record UserFilterDto(UserFilter FilterType, string Filter);
