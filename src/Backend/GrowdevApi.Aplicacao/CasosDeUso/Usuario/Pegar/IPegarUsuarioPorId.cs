using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;

public interface IPegarUsuarioPorId
{
    Task<RespostaDadosUsuario> Executar(RequisicaoPegarUsuario requisicao, CancellationToken cancellationToken);
}
