namespace Board.Application.Repositories;

public interface ITenantRepositoryFactory
{
    ITenantRepository GetTenant();
}
