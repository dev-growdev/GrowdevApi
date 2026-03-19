using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;

public interface ICadastrarUsuario
{
    Task<RespostaDadosUsuario> Executar(RequisicaoCadastrarUsuario requisicao, CancellationToken cancellationToken);
}
