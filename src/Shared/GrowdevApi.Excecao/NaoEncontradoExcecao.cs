using System.Net;

namespace GrowdevApi.Excecao;

public class NaoEncontradoExcecao : ExcecaoBase
{
    public NaoEncontradoExcecao(string message) : base(message) { }

    public override IList<string> PegarMensagensDeErro() => [Message];

    public override HttpStatusCode PegarStatusCode() => HttpStatusCode.NotFound;
}
