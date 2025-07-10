namespace workstation_back_end.Inquiry.Domain.Models.Queries;

public class GetAllInquiriesByExperienceQuery
{
    public int ExperienceId { get; set; }
    public GetAllInquiriesByExperienceQuery(int id) => ExperienceId = id;
}