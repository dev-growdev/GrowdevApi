namespace GrowdevApi.Comunicacao.Requisicoes;

/// <summary>
/// Representa uma requisição para alterar os dados de um usuário.
/// </summary>
public class RequisicaoAlterarUsuario
{
    /// <summary>
    /// Obrigatório - Identificador único (Guid) do usuário a ser alterado.
    /// </summary>
    public string? Id { get; set; } = string.Empty;

    /// <summary>
    /// Obrigatório - Nome completo do usuário.
    /// </summary>
    public string? Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obrigatório - Endereço de e-mail do usuário. Deve ser um e-mail válido.
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// Obrigatório - Status atual do usuário. Ativo ou Inativo.
    /// </summary>
    public string? Status { get; set; } = string.Empty;
}