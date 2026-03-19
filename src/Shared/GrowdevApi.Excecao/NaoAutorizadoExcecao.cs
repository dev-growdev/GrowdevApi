using System.Net;

namespace GrowdevApi.Excecao;

public class NaoAutorizadoExcecao : ExcecaoBase
{
    public NaoAutorizadoExcecao(string message) : base(message) { }

    public override IList<string> PegarMensagensDeErro() => [Message];

    public override HttpStatusCode PegarStatusCode() => HttpStatusCode.Unauthorized;
}
