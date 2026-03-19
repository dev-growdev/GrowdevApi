using GrowdevApi.Dominio.Interfaces.Repositorios;
using Moq;

namespace TestUtil.Repositorios;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}
