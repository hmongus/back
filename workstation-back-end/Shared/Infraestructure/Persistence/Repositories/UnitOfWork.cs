using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;

namespace workstation_back_end.Shared.Infraestructure.Persistence.Repositories;

public class UnitOfWork(TripMatchContext context):IUnitOfWork
{
    /// <inheritdoc />
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}