namespace DiscussionService.Contracts.Dto;
public record DiscussionDto(
    Guid DiscussionId,
    Guid RelationId,
    string RelationName,
    IEnumerable<Guid> UsersIds,
    string DiscussionStatus); 