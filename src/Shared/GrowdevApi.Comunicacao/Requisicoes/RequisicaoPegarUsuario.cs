namespace GrowdevApi.Comunicacao.Requisicoes;

/// <summary>
/// Representa uma requisição para pegar os dados de um usuário.
/// </summary>
public class RequisicaoPegarUsuario
{
    /// <summary>
    /// Obrigatório - Identificador único (Guid) do usuário a ser localizado.
    /// </summary>
    public string? Id { get; set; } = string.Empty;
}
