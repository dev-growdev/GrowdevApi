using System.Net;

namespace GrowdevApi.Excecao;

public class LoginUsuarioInvalidoExcecao : ExcecaoBase
{
    public LoginUsuarioInvalidoExcecao() : base(MensagensExcecao.EMAIL_OU_SENHA_INVALIDOS) { }

    public override IList<string> PegarMensagensDeErro() => [Message];

    public override HttpStatusCode PegarStatusCode() => HttpStatusCode.Unauthorized;
}
