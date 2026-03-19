using System.Net;

namespace GrowdevApi.Excecao;

public class SessaoExpiradaExcecao : ExcecaoBase
{
    public SessaoExpiradaExcecao() : base(MensagensExcecao.SESSAO_EXPIRADA) { }

    public override IList<string> PegarMensagensDeErro() => [Message];

    public override HttpStatusCode PegarStatusCode() => HttpStatusCode.Unauthorized;
}