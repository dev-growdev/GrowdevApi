namespace GrowdevApi.Dominio.Entidades;

public class EntidadeBase
{
    public Guid Id { get; set; }
    public DateTime DataCriacaoUtc { get; set; } = DateTime.UtcNow;
}
