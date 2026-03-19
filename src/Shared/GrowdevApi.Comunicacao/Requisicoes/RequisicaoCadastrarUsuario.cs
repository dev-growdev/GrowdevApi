namespace GrowdevApi.Comunicacao.Requisicoes;

/// <summary>
/// Representa uma requisição para cadastrar um novo usuário.
/// </summary>
public class RequisicaoCadastrarUsuario
{
    /// <summary>
    /// Obrigatório - Nome completo do usuário.
    /// </summary>
    public string? Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obrigatório - Endereço de e-mail do usuário. Deve ser um e-mail válido e não deve existir outro usuário cadastrado com o mesmo e-mail.
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// Obrigatório - Senha do usuário. Deve ter no mínimo 8 caracteres e deve ter pelo menos: uma letra maiúscula, uma letra minúscula, um número e um símbolo
    /// </summary>
    public string? Senha { get; set; } = string.Empty;
}