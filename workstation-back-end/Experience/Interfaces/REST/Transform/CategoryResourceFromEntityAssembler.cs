using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Experience.Interfaces.REST.Resources;

namespace workstation_back_end.Experience.Interfaces.REST.Transform;

public static class CategoryResourceFromEntityAssembler
{
    public static CategoryResource ToResourceFromEntity(Category category)
    {
        return new CategoryResource(category.Id, category.Name);
    }
}