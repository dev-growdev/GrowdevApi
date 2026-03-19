using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.Validadores;

public static class IdValidador
{
    public static Guid ValidarId(string? id, string? mensagem = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                mensagem = MensagensExcecao.ID_VAZIO;
            throw new ErroValidacaoExcecao([mensagem]);
        }

        if (!Guid.TryParse(id, out Guid idValido))
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                mensagem = MensagensExcecao.ID_INVALIDO;
            throw new ErroValidacaoExcecao([mensagem]);
        }
        return idValido;
    }

    public static bool IdEstaValido(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return false;

        if (Guid.TryParse(id, out Guid _))
        {
            return true;
        }
        return false;
    }
}
