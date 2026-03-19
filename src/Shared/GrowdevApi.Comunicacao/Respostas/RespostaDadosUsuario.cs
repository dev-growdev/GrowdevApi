namespace GrowdevApi.Comunicacao.Respostas;

/// <summary>
/// Representa uma resposta com os dados de um usuário.
/// </summary>
public class RespostaDadosUsuario
{
    /// <summary>
    /// Identificador único (Guid) do usuário.
    /// </summary>
    public string? Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    public string? Nome { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail do usuário.
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// Status atual do usuário.
    /// </summary>
    public string? Status { get; set; } = string.Empty;
}
