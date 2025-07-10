namespace workstation_back_end.Inquiry.Domain.Models.Queries;

public class GetResponseByInquiryIdQuery
{
    public int InquiryId { get; set; }

    public GetResponseByInquiryIdQuery(int inquiryId) => InquiryId = inquiryId;

}