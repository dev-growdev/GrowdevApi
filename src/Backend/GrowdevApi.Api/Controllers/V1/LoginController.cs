using Asp.Versioning;
using GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace GrowdevApi.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[Produces("application/json")]
public sealed class LoginController : ControllerBase
{
    /// <summary>
    /// Logar no sistema.
    /// </summary>
    /// <response code="200">Token gerado com sucesso</response>
    /// <response code="401">Erro de validação dos dados de login</response>
    [HttpPost]
    [ProducesResponseType(typeof(RespostaLogin), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RespostaLogin>> Login(
        [FromServices] IFazerLoginUsuario fazerLogin,
        [FromBody] RequisicaoLoginUsuario requisicao,
        CancellationToken cancellationToken)
    {
        var resposta = await fazerLogin.Executar(requisicao, cancellationToken);
        return Ok(resposta);
    }
}
