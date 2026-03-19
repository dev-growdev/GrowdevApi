namespace GrowdevApi.Dominio.Extensoes;

public static class ExtensaoEnumerable
{
    public static string ListaSepadadaPorVirgula(this IEnumerable<string> lista)
    {
        if (lista == null || !lista.Any())
            return string.Empty;

        return string.Join(", ", lista.Where(nome => !string.IsNullOrWhiteSpace(nome)));
    }
}
