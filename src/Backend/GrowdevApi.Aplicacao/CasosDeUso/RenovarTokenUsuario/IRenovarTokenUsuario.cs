using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;

namespace GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario;

public interface IRenovarTokenUsuario
{
    Task<RespostaTokens> Executar(RequisicaoNovoTokenUsuario requisicao, CancellationToken cancellationToken);
}
