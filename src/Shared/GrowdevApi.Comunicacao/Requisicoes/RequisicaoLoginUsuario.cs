namespace GrowdevApi.Comunicacao.Requisicoes;

/// <summary>
/// Representa uma requisição para fazer login no sistema.
/// </summary>
public class RequisicaoLoginUsuario
{
    /// <summary>
    /// Obrigatório - Endereço de e-mail do usuário.
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// Obrigatório - Senha do usuário.
    /// </summary>
    public string? Senha { get; set; } = string.Empty;
}
