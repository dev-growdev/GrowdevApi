using Asp.Versioning;
using GrowdevApi.Api.Atributos;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Excluir;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace GrowdevApi.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [UsuarioAutenticado]
    [Produces("application/json")]
    public sealed class UsuarioController : ControllerBase
    {
        /// <summary>
        /// Pegar dados de um usuário por ID
        /// </summary>
        /// <response code="200">Dados obtidos com sucesso</response>
        /// <response code="400">Erro de validação nos dados enviados</response>
        /// <response code="401">Erro de validação do token do usuário</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RespostaDadosUsuario), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RespostaDadosUsuario>> PegarUsuarioPorId(
            [FromServices] IPegarUsuarioPorId pegarUsuarioPorId,
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            var requisicao = new RequisicaoPegarUsuario
            {
                Id = id
            };
            var resposta = await pegarUsuarioPorId.Executar(requisicao, cancellationToken);
            return Ok(resposta);
        }

        /// <summary>
        /// Pegar os dados do usuário logado
        /// </summary>
        /// <response code="200">Dados obtidos com sucesso</response>
        /// <response code="401">Erro de validação do token do usuário</response>
        [HttpGet]
        [ProducesResponseType(typeof(RespostaDadosUsuario), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RespostaDadosUsuario>> PegarUsuarioLogado(
            [FromServices] IPegarUsuarioLogado pegarUsuarioLogado,
            CancellationToken cancellationToken)
        {
            var resposta = await pegarUsuarioLogado.Executar(cancellationToken);
            return Ok(resposta);
        }

        /// <summary>
        /// Cadastrar usuário
        /// </summary>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Erro de validação nos dados enviados</response>
        /// <response code="401">Erro de validação do token do usuário</response>
        [HttpPost]
        [ProducesResponseType(typeof(RespostaDadosUsuario), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RespostaDadosUsuario>> Cadastrar(
            [FromServices] ICadastrarUsuario cadastrarUsuario,
            [FromBody] RequisicaoCadastrarUsuario requisicao,
            CancellationToken cancellationToken)
        {
            var resposta = await cadastrarUsuario.Executar(requisicao, cancellationToken);
            return Created(string.Empty, resposta);
        }

        /// <summary>
        /// Alterar usuário
        /// </summary>
        /// <response code="204">Usuário alterado com sucesso</response>
        /// <response code="400">Erro de validação nos dados enviados</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="401">Erro de validação do token do usuário</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Alterar(
            [FromServices] IAlterarUsuario casoDeUso,
            [FromBody] RequisicaoAlterarUsuario requisicao,
            CancellationToken cancellationToken)
        {
            await casoDeUso.Executar(requisicao, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Excluir usuário
        /// </summary>
        /// <response code="204">Usuário excluído com sucesso</response>
        /// <response code="400">Erro na validação dos dados enviados</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="403">Usuário sem permissão para executar esta operação</response>
        /// <response code="401">Erro de validação do token do usuário</response>
        [UsuarioAutenticado]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaErro), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Excluir(
            [FromServices] IExcluirUsuario casoDeUso,
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            var requisicao = new RequisicaoExcluirUsuario
            {
                Id = id
            };
            await casoDeUso.Executar(requisicao, cancellationToken);
            return NoContent();
        }
    }
}
