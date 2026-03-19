using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Tokens;
using GrowdevApi.Excecao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace GrowdevApi.Api.Filtros;

public class FiltroUsuarioAutenticado : IAsyncAuthorizationFilter
{
    private readonly IValidadorTokenUsuario _validadorTokenUsuario;
    private readonly IRepositorioUsuario _repositorioUsuario;

    public FiltroUsuarioAutenticado(
        IValidadorTokenUsuario validadorTokenUsuario,
        IRepositorioUsuario usuarioRepository)
    {
        _validadorTokenUsuario = validadorTokenUsuario;
        _repositorioUsuario = usuarioRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext contexto)
    {
        var cancellationToken = contexto.HttpContext.RequestAborted;

        try
        {
            var token = ExtrairTokenDoCabecalho(contexto);

            var idUsuario = _validadorTokenUsuario.ValidarEPegarIdUsuario(token);

            if (!await _repositorioUsuario.ExisteUsuarioAtivoComId(idUsuario, cancellationToken))
                throw new NaoAutorizadoExcecao(MensagensExcecao.TOKEN_INVALIDO);
        }
        catch (SecurityTokenExpiredException)
        {
            Log.Warning("Erro na autenticação: {mensagem}", MensagensExcecao.TOKEN_EXPIRADO);
            contexto.Result = CriarResultadoNaoAutorizado(MensagensExcecao.TOKEN_EXPIRADO, true);
        }
        catch (ExcecaoBase ex)
        {
            Log.Warning("Erro na autenticação: {mensagem}", ex.Message);
            contexto.Result = CriarResultadoNaoAutorizado(ex.Message);
        }
        catch
        {
            Log.Warning("Erro na autenticação: {mensagem}", MensagensExcecao.TOKEN_INVALIDO);
            contexto.Result = CriarResultadoNaoAutorizado(MensagensExcecao.TOKEN_INVALIDO);
        }
    }

    private static string ExtrairTokenDoCabecalho(AuthorizationFilterContext contexto)
    {
        if (!contexto.HttpContext.Request.Headers.TryGetValue("Authorization", out var valoresCabecalho))
            throw new NaoAutorizadoExcecao(MensagensExcecao.SEM_TOKEN);

        var token = valoresCabecalho.ToString();
        if (string.IsNullOrWhiteSpace(token))
            throw new NaoAutorizadoExcecao(MensagensExcecao.SEM_TOKEN);

        const string prefixoBearer = "Bearer ";
        if (!token.StartsWith(prefixoBearer, StringComparison.OrdinalIgnoreCase))
            throw new NaoAutorizadoExcecao(MensagensExcecao.TOKEN_INVALIDO);

        return token.AsSpan(prefixoBearer.Length).Trim().ToString();
    }

    private static UnauthorizedObjectResult CriarResultadoNaoAutorizado(string mensagem, bool tokenExpirado = false) =>
        new(new RespostaErro(mensagem) { TokenEstaExpirado = tokenExpirado });
}
