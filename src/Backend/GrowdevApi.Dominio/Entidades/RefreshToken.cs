namespace GrowdevApi.Dominio.Entidades;

public class RefreshToken : EntidadeBase
{
    public string Valor { get; set; } = string.Empty;
    public Guid IdUsuario { get; set; }
    public Usuario Usuario { get; set; } = default!;
}
