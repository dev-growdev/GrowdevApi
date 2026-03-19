namespace GrowdevApi.Dominio.Interfaces.Tokens;

public interface IGeradorToken
{
    string GerarToken(int tamanhoEmBytes = 32);
}
