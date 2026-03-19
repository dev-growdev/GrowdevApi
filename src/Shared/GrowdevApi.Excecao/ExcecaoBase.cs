using System.Net;

namespace GrowdevApi.Excecao;

public abstract class ExcecaoBase : SystemException
{
    public ExcecaoBase(string message) : base(message) { }

    public abstract IList<string> PegarMensagensDeErro();
    public abstract HttpStatusCode PegarStatusCode();
}
