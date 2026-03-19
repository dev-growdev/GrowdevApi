namespace GrowdevApi.Comunicacao.Respostas;

/// <summary>
/// Representa uma resposta de tokens de acesso.
/// </summary>
public class RespostaTokens
{
    /// <summary>
    /// Token do usuário.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token para requisição de novo token.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
