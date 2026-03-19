using System.Net;

namespace GrowdevApi.Excecao;

public class ProibidoExcecao : ExcecaoBase
{
    public ProibidoExcecao(string message) : base(message) { }

    public override IList<string> PegarMensagensDeErro() => [Message];

    public override HttpStatusCode PegarStatusCode() => HttpStatusCode.Forbidden;
}
