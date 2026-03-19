using GrowdevApi.Dominio.Interfaces.Repositorios;

namespace GrowdevApi.Infraestrutura.Repositorios;

public class UnitOfWork : IUnitOfWork
{
    private readonly GrowdevApiDbContext _dbContext;

    public UnitOfWork(GrowdevApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SalvarAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
