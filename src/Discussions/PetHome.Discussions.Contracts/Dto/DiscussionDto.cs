namespace PetHome.Discussions.Contracts.Dto;
public record DiscussionDto(
    Guid DiscussionId,
    Guid RelationId,
    string RelationName,
    IEnumerable<Guid> UsersIds,
    string DiscussionStatus); 