using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Criptografia;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Tokens;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;

public class FazerLoginUsuario : IFazerLoginUsuario
{
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IRepositorioRefreshToken _repositorioRefreshToken;
    private readonly IEncriptadorSenha _encriptadorSenha;
    private readonly IGeradorTokenUsuario _geradorTokenUsuario;
    private readonly IGeradorRefreshToken _geradorRefreshToken;
    private readonly IUnitOfWork _unitOfWork;

    public FazerLoginUsuario(
        IRepositorioUsuario repositorioUsuario,
        IEncriptadorSenha encriptadorSenha,
        IGeradorTokenUsuario jwtTokenService,
        IGeradorRefreshToken geradorRefreshToken,
        IRepositorioRefreshToken repositorioRefreshToken,
        IUnitOfWork unitOfWork)
    {
        _repositorioUsuario = repositorioUsuario;
        _encriptadorSenha = encriptadorSenha;
        _geradorTokenUsuario = jwtTokenService;
        _geradorRefreshToken = geradorRefreshToken;
        _repositorioRefreshToken = repositorioRefreshToken;
        _unitOfWork = unitOfWork;
    }

    public async Task<RespostaLogin> Executar(RequisicaoLoginUsuario requisicao, CancellationToken cancellationToken)
    {
        await Validar(requisicao, cancellationToken);

        var usuario = await _repositorioUsuario.PegarUsuarioPorEmail(requisicao.Email!, cancellationToken);
        if (usuario == null || !_encriptadorSenha.SenhaValida(requisicao.Senha!, usuario.Senha!))
        {
            throw new LoginUsuarioInvalidoExcecao();
        }

        var token = _geradorTokenUsuario.Gerar(usuario.Id);

        return new RespostaLogin
        {
            Nome = usuario.Nome,
            Tokens = new RespostaTokens
            {
                AccessToken = token,
                RefreshToken = await CriarESalvarRefreshToken(usuario, cancellationToken)
            }
        };
    }

    private static async Task Validar(RequisicaoLoginUsuario requisicao, CancellationToken cancellationToken)
    {
        var validador = new FazerLoginUsuarioValidador();

        var resultado = await validador.ValidateAsync(requisicao, cancellationToken);

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
            throw new ErroValidacaoExcecao(mensagensDeErro);
        }
    }

    private async Task<string> CriarESalvarRefreshToken(Dominio.Entidades.Usuario usuario, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            Valor = _geradorRefreshToken.Gerar(),
            IdUsuario = usuario.Id
        };

        await _repositorioRefreshToken.SalvarNovoRefreshToken(refreshToken, cancellationToken);
        await _unitOfWork.SalvarAsync(cancellationToken);

        return refreshToken.Valor;
    }
}
