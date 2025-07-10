namespace workstation_back_end.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}