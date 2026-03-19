namespace GrowdevApi.Comunicacao.Respostas;

/// <summary>
/// Representa uma resposta de erro.
/// </summary>
public class RespostaErro
{
    /// <summary>
    /// Lista de mensagens de erros ocorridos.
    /// </summary>
    public IList<string> MensagensDeErro { get; set; }

    /// <summary>
    /// Indicador se o token está expirado.
    /// </summary>
    public bool TokenEstaExpirado { get; set; }

    public RespostaErro(IList<string> mensagensDeErro) => MensagensDeErro = mensagensDeErro;

    public RespostaErro(string mensagemDeErro) => MensagensDeErro = [mensagemDeErro];
}