namespace GrowdevApi.Comunicacao.Requisicoes;

/// <summary>
/// Representa uma requisição para cadastrar um novo usuário.
/// </summary>
public class RequisicaoExcluirUsuario
{
    /// <summary>
    /// Obrigatório - Identificador único (Guid) do usuário a ser excluído.
    /// </summary>
    public string? Id { get; set; } = string.Empty;
}
