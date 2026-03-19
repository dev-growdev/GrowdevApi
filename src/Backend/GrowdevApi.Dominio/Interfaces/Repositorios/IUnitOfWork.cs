namespace GrowdevApi.Dominio.Interfaces.Repositorios;

public interface IUnitOfWork
{
    Task SalvarAsync(CancellationToken cancellationToken);
}
