namespace GrowdevApi.Dominio.Interfaces.Tokens;

public interface IGeradorTokenUsuario
{
    string Gerar(Guid idUsuario);
}
