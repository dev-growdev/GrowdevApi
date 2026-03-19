using GrowdevApi.Dominio.Enums;

namespace GrowdevApi.Dominio.Entidades;

public class Usuario : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Senha { get; set; } = null;
    public StatusUsuario Status { get; set; } = StatusUsuario.Indefinido;
}
