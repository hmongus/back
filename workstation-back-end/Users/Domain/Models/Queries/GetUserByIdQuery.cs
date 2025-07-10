namespace workstation_back_end.Users.Domain.Models.Queries;


public record GetUserByIdQuery
{
    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }
}