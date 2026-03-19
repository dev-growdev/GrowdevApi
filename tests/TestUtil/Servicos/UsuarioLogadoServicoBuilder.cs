using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Servicos;
using Moq;

namespace TestUtil.Servicos;

public class UsuarioLogadoServicoBuilder
{
    public static IUsuarioLogadoServico Build(Usuario usuario, CancellationToken cancellationToken)
    {
        var UsuarioLogadoMock = new Mock<IUsuarioLogadoServico>();
        UsuarioLogadoMock.Setup(usuarioLogado => usuarioLogado.PegarUsuarioLogado(cancellationToken)).ReturnsAsync(usuario);

        return UsuarioLogadoMock.Object;
    }
}
