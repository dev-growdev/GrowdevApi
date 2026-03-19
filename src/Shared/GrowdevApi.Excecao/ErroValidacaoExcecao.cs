using System.Net;

namespace GrowdevApi.Excecao;

public class ErroValidacaoExcecao : ExcecaoBase
{
    private readonly IList<string> _mensagensDeErro;

    public ErroValidacaoExcecao(IList<string> mensagensDeErro) : base(string.Empty)
    {
        _mensagensDeErro = mensagensDeErro;
    }

    public override IList<string> PegarMensagensDeErro() => _mensagensDeErro;

    public override HttpStatusCode PegarStatusCode() => HttpStatusCode.BadRequest;
}
