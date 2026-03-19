namespace GrowdevApi.Comunicacao.Requisicoes;

/// <summary>
/// Representa uma requisição para atualizar o token do usuário.
/// </summary>
public class RequisicaoNovoTokenUsuario
{
    /// <summary>
    /// Obrigatório - Refresh Token recebido no login.
    /// </summary>
    public string? RefreshToken { get; set; } = string.Empty;
}
