using Asp.Versioning;
using GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace GrowdevApi.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[Produces("application/json")]
public sealed class TokenController : ControllerBase
{
    /// <summary>
    /// Renovar o token do usuário
    /// </summary>
    /// <response code="200">Token renovado com sucesso</response>
    /// <response code="401">Erro de validação do refresh token</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(RespostaTokens), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RespostaTokens>> NovoTokenUsuario(
        [FromServices] IRenovarTokenUsuario renovarToken,
        [FromBody] RequisicaoNovoTokenUsuario requisicao,
        CancellationToken cancellationToken)
    {
        var resposta = await renovarToken.Executar(requisicao, cancellationToken);
        return Ok(resposta);
    }
}
