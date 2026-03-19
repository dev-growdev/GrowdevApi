using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;

namespace GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;

public interface IFazerLoginUsuario
{
    Task<RespostaLogin> Executar(RequisicaoLoginUsuario requisicao, CancellationToken cancellationToken);
}
