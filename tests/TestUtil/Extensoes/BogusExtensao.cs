using Bogus;

namespace TestUtil.Extensoes;

public static class BogusExtensao
{
    public static string AlphaNumericUrl(this Randomizer randomizer, int tamanho)
    {
        const string urlSafeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
        return randomizer.String2(tamanho, urlSafeChars);
    }
}
