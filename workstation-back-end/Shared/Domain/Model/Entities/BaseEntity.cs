namespace workstation_back_end.Shared.Domain.Model.Entities;

public class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}