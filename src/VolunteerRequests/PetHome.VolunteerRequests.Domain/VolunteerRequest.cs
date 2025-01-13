using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Domain;
public class VolunteerRequest
{
    public VolunteerRequestId Id { get; private set; }
    public UserId? AdminId { get; private set; }
    public UserId UserId { get; private set; }
    public VolunteerInfo? VolunteerInfo { get; private set; }
    public VolunteerRequestStatus? Status { get; private set; }
    public Date CreatedAt { get; private set; }
    public RequestComment? RejectedComment { get; private set; }
    public DiscussionId? DiscussionId { get; private set; }

    public VolunteerRequest(
        UserId userId,
        VolunteerInfo? volunteerInfo)
    {
        Id = VolunteerRequestId.Create().Value;
        CreatedAt = Date.Create().Value;
        Status = VolunteerRequestStatus.RevisionRequired;
        VolunteerInfo = volunteerInfo;
    }

    public static VolunteerRequest Create(
        UserId userId,
        VolunteerInfo? volunteerInfo)
    {
        return new VolunteerRequest(userId, volunteerInfo);
    }

    public void SetOnReview(
        UserId adminId,
        DiscussionId? discussionId)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.OnReview;
        DiscussionId = discussionId;
    }

    public void SetRevisionRequired(
        UserId adminId,
        RequestComment rejectedComment)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.RevisionRequired;
        RejectedComment = rejectedComment;
    }

    public void SetRejected(
        UserId adminId,
        RequestComment rejectedComment)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.Rejected;
        RejectedComment = rejectedComment;
    }

    public void SetApproved(
        UserId adminId)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.Approved;
    }

    public void SetSubmitted(
        UserId adminId)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.Submitted;
    }
}
