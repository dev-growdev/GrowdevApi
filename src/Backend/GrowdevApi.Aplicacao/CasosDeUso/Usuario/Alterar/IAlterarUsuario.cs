using GrowdevApi.Comunicacao.Requisicoes;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;

public interface IAlterarUsuario
{
    Task Executar(RequisicaoAlterarUsuario requisicao, CancellationToken cancellationToken);
}
