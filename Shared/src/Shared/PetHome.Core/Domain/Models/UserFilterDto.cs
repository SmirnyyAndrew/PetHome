using PetHome.SharedKernel.Enums;

namespace PetHome.Core.Domain.Models;
public record UserFilterDto(UserFilter FilterType, string Filter);
