namespace GrowdevApi.Comunicacao.Respostas;

/// <summary>
/// Representa uma resposta de um login.
/// </summary>
public class RespostaLogin
{
    /// <summary>
    /// Nome completo do usuário que logou.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Tokens de acesso.
    /// </summary>
    public RespostaTokens Tokens { get; set; } = default!;
}
