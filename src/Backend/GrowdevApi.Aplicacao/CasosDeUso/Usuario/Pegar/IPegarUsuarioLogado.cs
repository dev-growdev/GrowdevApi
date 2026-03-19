using GrowdevApi.Comunicacao.Respostas;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;

public interface IPegarUsuarioLogado
{
    Task<RespostaDadosUsuario> Executar(CancellationToken cancellationToken);
}