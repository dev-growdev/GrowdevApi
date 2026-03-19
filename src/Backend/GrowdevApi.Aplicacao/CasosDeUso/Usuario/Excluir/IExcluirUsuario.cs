using GrowdevApi.Comunicacao.Requisicoes;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Excluir;

public interface IExcluirUsuario
{
    Task Executar(RequisicaoExcluirUsuario requisicao, CancellationToken cancellationToken);
}
