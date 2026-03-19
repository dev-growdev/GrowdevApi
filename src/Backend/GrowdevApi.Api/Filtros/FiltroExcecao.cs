using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Extensoes;
using GrowdevApi.Excecao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace GrowdevApi.Api.Filtros;

public class FiltroExcecao : IExceptionFilter
{
    public void OnException(ExceptionContext contexto)
    {
        var exception = contexto.Exception;
        if (exception is ExcecaoBase excecaoBase)
        {
            Log.Warning("Erro personalizado do tipo {ExceptionType}: {mensagens}", excecaoBase.GetType().Name, excecaoBase.PegarMensagensDeErro().ListaSepadadaPorVirgula());
            HandleProjectException(excecaoBase, contexto);
            return;
        }
        HandleUnknownException(contexto);
    }

    private static void HandleProjectException(ExcecaoBase excecao, ExceptionContext contexto)
    {
        contexto.HttpContext.Response.StatusCode = (int)excecao.PegarStatusCode();
        contexto.Result = new ObjectResult(new RespostaErro(excecao.PegarMensagensDeErro()));
    }

    private static void HandleUnknownException(ExceptionContext contexto)
    {
        var excecao = contexto.Exception;
        var mensagemErro = excecao.InnerException != null
            ? $"{excecao.Message} - {excecao.InnerException.Message}"
            : excecao.Message;

        Log.Error("Erro não tratado - {ErrorMessage}", mensagemErro);
        contexto.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        contexto.Result = new ObjectResult(new RespostaErro(MensagensExcecao.ERRO_DESCONHECIDO));
    }
}
